using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.TestData
{
    public class WikiDataEntities
    {
        public List<string> entities = new List<string>();
        
        public WikiDataEntities ()
        {
            GetEntitiesFromFile();
        }

        public async void GetEntitiesFromFile ()
        {
            IO.FileStreamHandler fileStreamHandler = new IO.FileStreamHandler();
            entities = await fileStreamHandler.ReadFile("C:/P5/FunctionalityLayer_FactChecker/FactChecker/TestData/entities.txt");
        }
    }
}
