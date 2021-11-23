using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FactChecker.APIs.KnowledgeGraphAPI
{
    public static class KnowledgeGraphHandler
    {
        public static string knowledgeGraphURL = "https://query.wikidata.org/bigdata/namespace/wdq/sparql";
        static HttpClient client = new HttpClient();
    
        public static async Task<KnowledgeGraphItem[]> GetTriplesBySparQL(string entityA, string entityB, int limit)
        {
            client.DefaultRequestHeaders.Add("User-Agent", "FactChecker/0.0 (kontakt@magnusaxelsen.dk) generic-library/0.0");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            KnowledgeGraphItem[] triples = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync(knowledgeGraphURL + "?query=SELECT ?r WHERE {wd:" + entityA + " ?r wd:" + entityB + "} limit " + limit);           
                if (response.IsSuccessStatusCode)
                {
                    XDocument xdoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
                    StringReader sr = new StringReader(xdoc.ToString());

                    DataSet ds = new DataSet();

                    ds.ReadXml(sr);
                    Console.WriteLine(ds.CreateDataReader());
                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            foreach (object item in row.ItemArray)
                            {
                                Console.WriteLine(item);
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return triples;
        }
    }
}
