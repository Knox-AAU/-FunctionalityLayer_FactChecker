    using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TripleController : ControllerBase
    {
        public List<string> triples = new List<string>(); 
        
        public TripleController ()
        {
            GetTriplesFromFile();
        } 

        private async void GetTriplesFromFile ()
        {
            Console.WriteLine("Im startistarti");
            IO.FileStreamHandler fileStreamHandler = new IO.FileStreamHandler();
            foreach (string s in await fileStreamHandler.ReadFile("./TestData/relations.txt"))
            {
                triples.Add(s);
            }
            Console.WriteLine("im donidoni");
        }


        [HttpGet]
        public List<string> Get()
        {
            return triples;
        }
    }
}
