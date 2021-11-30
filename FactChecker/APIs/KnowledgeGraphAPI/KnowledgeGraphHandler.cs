using System;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.Generic;

namespace FactChecker.APIs.KnowledgeGraphAPI
{
    public class KnowledgeGraphHandler
    {

        public string knowledgeGraphURL = "https://query.wikidata.org/bigdata/namespace/wdq/sparql";
        HttpClient client = new HttpClient();

        public async Task<List<KnowledgeGraphItem>> GetTriplesBySparQL(string s, int limit)
        {
            client.DefaultRequestHeaders.Add("User-Agent", "FactChecker/0.0 (kontakt@magnusaxelsen.dk) generic-library/0.0");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            List<KnowledgeGraphItem> triples = new List<KnowledgeGraphItem>();
            try
            {
                HttpResponseMessage response = await client.GetAsync(knowledgeGraphURL + "?query=SELECT ?r ?t WHERE {wd:" + s + " ?r ?t}ORDER BY ?t limit " + limit);           
                if (response.IsSuccessStatusCode)
                {
                    XDocument xdoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
                    StringReader sr = new StringReader(xdoc.ToString());
                    DataSet ds = new DataSet();
                    ds.ReadXml(sr);

                    foreach (DataTable table in ds.Tables)
                    {
                        string rSplit = "";
                        string tSplit = "";
                        foreach (DataRow row in table.Rows)
                        {
                            foreach (object item in row.ItemArray)
                            {
                                if(item.ToString().Contains("http://"))
                                {
                                    String[] splitted = item.ToString().Split('/');
                                    if(splitted[splitted.Length - 1].Contains("P"))
                                    {
                                        rSplit = splitted[splitted.Length - 1];
                                    }else if(splitted[splitted.Length -1].Contains("Q"))
                                    {
                                        tSplit = splitted[splitted.Length - 1];
                                    }
                                }
                            }
                            if(rSplit != "" && tSplit != "")
                            {
                                triples.Add(new KnowledgeGraphItem(s, rSplit, tSplit));
                                rSplit = "";
                                tSplit = "";
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return triples;
        }
    }

}

