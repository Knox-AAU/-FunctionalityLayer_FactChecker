using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Data.SQLite;
using FactChecker.APIs.KnowledgeGraphAPI;

namespace XUnitTestProject
{
    public class TFIDFTesting
    {
        [Fact]
        public void TestTermFrequency ()
        {
            FactChecker.TFIDF.TFIDFHandler tFIDFHandler = new();
            float tf = tFIDFHandler.CalculateTermFrequency(20);

            Assert.Equal(2.3, Math.Round(tf, 2));
        }

        [Fact]
        public void TestInverseDocumentFrequenct()
        {
            FactChecker.TFIDF.TFIDFHandler tFIDFHandler = new();
            float idf = tFIDFHandler.CalculateInverseDocumentFrequency(10, 5);

            Assert.Equal(0.3, Math.Round(idf, 2));
        }
        [Fact]
        public void TestTF()
        {
            var tfIfdHandler = new FactChecker.TFIDF.TFIDFHandler();
            var res = tfIfdHandler.GetArticles(new KnowledgeGraphItem("human", "", ""));
        }
    }
}
