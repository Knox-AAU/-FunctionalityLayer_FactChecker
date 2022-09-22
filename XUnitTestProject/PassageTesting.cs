using FactChecker.Controllers.Exceptions;
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
        [Fact]
        public void TestNumberOfPassagesFound2()
        {
            PassageRetrievalHandler pi = new();

            pi.PassageLength = 10;
            pi.PassageOverlap = 2;
            Article article = new() { FullText = "The air quality in Singapore is monitored through a network of air monitoring stations located in different parts of the island" };
            Assert.Equal(3, pi.GetPassages(article).ToList().Count);
        }
        [Fact]
        public void TestNumberOfPassagesFound3()
        {
            PassageRetrievalHandler pi = new()
            {
                PassageLength = 15,
                PassageOverlap = 5
            };
            Article article = new() { FullText = "The air quality in Singapore is monitored through a network of air monitoring stations located in different parts of the island" };
            var old_p = pi.GetPassages(article).ToList();
            var new_p = pi.GetPassage_new(article.FullText).ToList();
            var new_p_V2 = pi.GetPassage_new_V2(article.FullText).ToList();
            Assert.Equal(2, old_p.Count);
            Assert.Equal(2, new_p.Count);
        }
        [Fact]
        public void TestOverLapHigherThanLength()
        {
            PassageRetrievalHandler pi = new() { PassageOverlap = 81 };
            Article article = new () { FullText = "lorem ipsum casper dipsum" };
            Assert.Throws<PassageRetrievalFailedFilteredException>(() => pi.GetPassages(article).ToList().Count);
        }
        [Fact]
        public void TestOverLapLowerThanLength()
        {
            PassageRetrievalHandler pi = new();
            pi.PassageOverlap = 79;
            Article article = new() { FullText = "lorem ipsum casper dipsum" };
            pi.GetPassages(article);
        }

    }
}
