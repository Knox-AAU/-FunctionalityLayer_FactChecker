using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Data.SQLite;


namespace XUnitTestProject
{
    public class TFIDFTesting
    {
        [Fact]
        public void TestTermFrequency ()
        {
            FactChecker.TFIDF.TFIDFHandler tFIDFHandler = new FactChecker.TFIDF.TFIDFHandler();
            float tf = tFIDFHandler.CalculateTermFrequency(20);

            Assert.Equal(2.3, Math.Round(tf, 2));
        }

        [Fact]
        public void TestInverseDocumentFrequenct()
        {
            FactChecker.TFIDF.TFIDFHandler tFIDFHandler = new FactChecker.TFIDF.TFIDFHandler();
            float idf = tFIDFHandler.CalculateInverseDocumentFrequency(10,5);

            Assert.Equal(0.3, Math.Round(idf, 2));
        }
    }
}
