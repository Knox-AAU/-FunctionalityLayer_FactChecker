    using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FactChecker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TripleController : ControllerBase
    {
        public static TestData.WikiDataTriples WikiDataTriples = new TestData.WikiDataTriples();

        [HttpGet]
        public IEnumerable<APIs.KnowledgeGraphAPI.KnowledgeGraphItem> Get()
        {
            return WikiDataTriples.triples;
        }

        [HttpPost]
        public APIs.KnowledgeGraphAPI.KnowledgeGraphItem Post(APIs.KnowledgeGraphAPI.KnowledgeGraphItem item)
        {
            TFIDF.TFIDFHandler tFIDFHandler = new TFIDF.TFIDFHandler();
            string search = item.s + item.r + item.t;
            List<string> searchList = new List<string>();
            foreach(string s in search.Split(' '))
            {
                searchList.Add(s);
            }
            List<TFIDF.TFIDFItem> tFIDFItems = tFIDFHandler.CalculateTFIDF(searchList);
            
            item.passage = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";  
            return item;
        }
    }
}
