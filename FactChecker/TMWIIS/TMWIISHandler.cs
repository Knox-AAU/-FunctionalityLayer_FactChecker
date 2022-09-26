using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactChecker.TFIDF;
using FactChecker.PassageRetrieval;
using FactChecker.APIs.KnowledgeGraphAPI;
using FactChecker.Intefaces;
using FactChecker.Controllers.Exceptions;

namespace FactChecker.TMWIIS
{
    public class TMWIISHandler : IEvidenceRetrieval
    {
        private readonly int maxPassages = 5;
        private List<Article> Articles;
        private KnowledgeGraphItem knowledgeGraphItem;
        private Stopwords.Stopwords stopwords = new();
        readonly float lambda1 = 0.9f, lambda2 = 0.05f, lambda3 = 0.05f;
        private List<Passage> Evidence()
        {
            List<TMWIISItem> rankedPassages = new();
            WordcountDB.Article articleHandler = new();

            int sourceTotalOccurence = GetNumberOfOccurencesInAllDocuments(knowledgeGraphItem.s);
            int relationTotalOccurence = GetNumberOfOccurencesInAllDocuments(knowledgeGraphItem.r);
            int targetTotalOccurence = GetNumberOfOccurencesInAllDocuments(knowledgeGraphItem.t);

            for (int j = 0; j < Articles.Count; j++)
            {
                WordcountDB.ArticleItem article = articleHandler.FetchDB(Articles[j].Id);
                List<string> passages = GetPassages(article.Text);
                int sourceDocumentOccurence = WordOccurrence(knowledgeGraphItem.s, article.Text);
                int relationDocumentOccurence = WordOccurrence(knowledgeGraphItem.r, article.Text);
                int targetDocumentOccurence = WordOccurrence(knowledgeGraphItem.t, article.Text);
                for (int i = 0; i < passages.Count; i++)
                {
                    int passageLength = PassageLength(passages[i]);

                    int sourcePassageOccurence = WordOccurrence(knowledgeGraphItem.s, passages[i]);
                    int relationPassageOccurence = WordOccurrence(knowledgeGraphItem.r, passages[i]);
                    int targetPassageOccurence = WordOccurrence(knowledgeGraphItem.t, passages[i]);
                    
                    
                    float evidenceSource = EvidenceCalculator(passageLength, article.UniqueLenght, sourcePassageOccurence, sourceDocumentOccurence, sourceTotalOccurence);
                    float evidenceRelation = EvidenceCalculator(passageLength, article.UniqueLenght, relationPassageOccurence, relationDocumentOccurence, relationTotalOccurence);
                    float evidenceTarget = EvidenceCalculator(passageLength, article.UniqueLenght, targetPassageOccurence, targetDocumentOccurence, targetTotalOccurence);
                    float passageScore = lambda1 * evidenceSource + lambda2 * evidenceRelation + lambda3 * evidenceTarget;
                    rankedPassages.Add(new TMWIISItem(passageScore, passages[i], article.Link));
                }
            }
            rankedPassages.Sort((p, q) => q.score.CompareTo(p.score));
            var list_of_passages =  rankedPassages.Select(p => new Passage
            {
                Text = p.passage,
                Score = p.score
            }).ToList();
            if (list_of_passages.Count == 0) throw new PassageNotFoundFilteredException(knowledgeGraphItem.s);
            return list_of_passages;
        }
        private List<string> GetPassages(string text)
        {
            IPassageRetrieval pr = new PassageRetrievalHandler();
            return pr.GetPassages(new Article() { FullText=text }).Select(p => p.Text).ToList();
        }
        private float EvidenceCalculator(int passageLength, int uniqueLength, int passageOccurrence, int documentOccurrence, int totalOccurrence)
        {
            float passageSource, documentSource, collectionSource; 

            passageSource = (passageOccurrence + 1) / ((float)passageLength + (float)uniqueLength);
            documentSource = (documentOccurrence + 1) / ((float)passageLength + (float)uniqueLength);
            collectionSource = (totalOccurrence) /(float) uniqueLength;
            return  passageSource  * documentSource * collectionSource;
        }

        private int GetNumberOfOccurencesInAllDocuments (string word)
        {
            List<string> splitted = word.Split(' ').ToList();           
            WordcountDB.WordCount wordCount = new ();
            int sum = 0;

            foreach(string s in splitted)
                sum += wordCount.FetchSumOfOccurences(s);
            return sum;
        }
        private int PassageLength(string passage)
        {
            int length = passage.Split(' ').ToList().Count;
            return length;
        }
        private int WordOccurrence(string entity, string passage)
        {
            List<string> passageWords = passage.Split(" ").ToList();
            List<string> entityList = entity.Split(" ").ToList();
            int length = passageWords.Count;
            int occurrences = 0;
            for(int j = 0; j < entityList.Count; j++)
                for (int i = 0; i < length; i++)
                    if (passageWords[i] == entityList[j] && !stopwords.stopwords.ContainsKey(passageWords[i]))
                        occurrences++;
            return occurrences;
        }

        /// <summary>
        /// This could potetially give wrong results, needs a fix
        /// </summary>
        /// <param name="articles"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public IEnumerable<Passage> GetEvidence(List<Article> articles, List<KnowledgeGraphItem> items)
        {
            Articles = articles;
            List<Passage> res_passages = new();
            foreach (var item in items)
            {
                knowledgeGraphItem = item;
                var evidence = Evidence();
                if (res_passages.Count == 0)
                    res_passages.AddRange(evidence);
                else
                    for (int i = 0; i < evidence.Count; i++)
                        res_passages[i].Score += evidence[i].Score;
            }
            return res_passages.OrderByDescending(p => p.Score).Take(50);
        }
        public IEnumerable<Passage> GetEvidence(List<Article> articles, KnowledgeGraphItem item)
        {
            return GetEvidence(articles, new List<KnowledgeGraphItem>() { item });
        }
    }
}
