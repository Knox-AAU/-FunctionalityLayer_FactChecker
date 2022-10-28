using FactChecker.EF;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.WordcountDB
{
    public class Article
    {
        private readonly KnoxFactCheckingTestDbContext context;
        public Article(KnoxFactCheckingTestDbContext context)
        {
            this.context = context;
        }
        public ArticleItem FetchDB(int id)
        {
            return context.article.Where(p => p.id == id).First();
        }
    }
}
