using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTestProject
{
    public class KnowledgeGraphTesting
    {
        [Fact]
        public async void TestFetchKnowledgeGraphLimit()
        {
            FactChecker.APIs.KnowledgeGraphAPI.KnowledgeGraphHandler knowledgeGraphHandler = new FactChecker.APIs.KnowledgeGraphAPI.KnowledgeGraphHandler();
            List<FactChecker.APIs.KnowledgeGraphAPI.KnowledgeGraphItem> items = await knowledgeGraphHandler.GetTriplesBySparQL("Q1060165", 2);

            Assert.Equal(2, items.Count);
        }

        [Fact]
        public async void TestFetchKnowledgeGraphObject()
        {
            FactChecker.APIs.KnowledgeGraphAPI.KnowledgeGraphHandler knowledgeGraphHandler = new FactChecker.APIs.KnowledgeGraphAPI.KnowledgeGraphHandler();
            List<FactChecker.APIs.KnowledgeGraphAPI.KnowledgeGraphItem> items = await knowledgeGraphHandler.GetTriplesBySparQL("Q1060165", 2);

            Assert.True(items[0].r != null && items[0].s != null && items[0].t != null);
        }
    }
}
