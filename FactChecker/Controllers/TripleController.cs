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
    }
}
