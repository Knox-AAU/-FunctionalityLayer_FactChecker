using FactChecker.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.WordcountDB
{
    public class stopwords
    {
        private readonly KnoxFactCheckingTestDbContext context;
        public stopwords(KnoxFactCheckingTestDbContext context)
        {
            this.context = context;
        }

        public List<StopWordItem> GetStopwords(Stopwords.Stopwords_Language lang = Stopwords.Stopwords_Language.en)
        {
            return context.stopwords.Where(p => p.lang == lang.ToString()).ToList();
        }

        public async Task UploadAllStopWords()
        {
            List<Stopwords.Stopwords_Language> langs = new() { Stopwords.Stopwords_Language.da, Stopwords.Stopwords_Language.en };
            foreach (var lang in langs)
            {
                var s = new Stopwords.Stopwords(lang);
                await s.GetStopWords();
                foreach (var word in s.stopwords_hashset)
                {
                    if (context.stopwords.FirstOrDefault(p => p.word == word && p.lang == lang.ToString()) == null)
                        context.stopwords.Add(new StopWordItem() { lang = lang.ToString(), word = word });
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
