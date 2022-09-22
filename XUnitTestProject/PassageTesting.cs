using FactChecker.Intefaces;
using FactChecker.PassageRetrieval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestProject
{
    public class PassageTesting
    {
        [Fact]
        public void TestNumberOfPassagesFound()
        {
            PassageRetrievalHandler pi = new();

            pi.PassageLength = 2;
            pi.PassageOverlap = 1;
            Article article = new Article() { FullText = "lorem ipsum casper dipsum" };
            Assert.Equal(2, pi.GetPassages(article).ToList().Count);
        }
    }
}
