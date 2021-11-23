using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.Generic;

namespace FactChecker.APIs.KnowledgeGraphAPI
{
    public static class KnowledgeGraphHandler
    {
        public static string knowledgeGraphURL = "https://query.wikidata.org/bigdata/namespace/wdq/sparql";
        static HttpClient client = new HttpClient();
    
        public static async Task<List<KnowledgeGraphItem>> GetTriplesBySparQL(string s, string t, int limit)
        {
            client.DefaultRequestHeaders.Add("User-Agent", "FactChecker/0.0 (kontakt@magnusaxelsen.dk) generic-library/0.0");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            List<KnowledgeGraphItem> triples = new List<KnowledgeGraphItem>();
            try
            {
                HttpResponseMessage response = await client.GetAsync(knowledgeGraphURL + "?query=SELECT ?r WHERE {wd:" + s + " ?r wd:" + t + "} limit " + limit);           
                if (response.IsSuccessStatusCode)
                {
                    XDocument xdoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
                    StringReader sr = new StringReader(xdoc.ToString());
                    DataSet ds = new DataSet();

                    ds.ReadXml(sr);

                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            foreach (object item in row.ItemArray)
                            {
                                if(item.ToString().Contains("http://"))
                                {
                                    String[] splitted = item.ToString().Split('/');
                                    triples.Add(new KnowledgeGraphItem(s, splitted[splitted.Length - 1], t));
                                }
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
