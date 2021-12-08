using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactChecker.TFIDF;
using FactChecker.PassageRetrieval;
using FactChecker.APIs.KnowledgeGraphAPI;

namespace FactChecker.TMWIIS
{
    public class TMWIISHandler
    {
        private int maxPassages = 5;
        public List<int> articleIDs;
        public KnowledgeGraphItem knowledgeGraphItem;
        public TMWIISHandler(List<int> articleID, KnowledgeGraphItem KGitem)
        {
            articleIDs = articleID;
            knowledgeGraphItem = KGitem;
        }
        public List<TMWIISItem> Evidence()
        {
            List<TMWIISItem> rankedPassages = new List<TMWIISItem>();
            WordcountDB.Article articleHandler = new WordcountDB.Article();

            for(int j = 0; j < articleIDs.Count; j++)
            {
                WordcountDB.ArticleItem article = articleHandler.FetchDB(articleIDs[j]);
                List<string> passages = GetPassages(article.Text);
                for (int i = 0; i < passages.Count; i++)
                {
                    int passageLength = PassageLength(passages[i]);

                    int sourcePassageOccurence = WordOccurrence(knowledgeGraphItem.s, passages[i]);
                    int sourceDocumentOccurence = WordOccurrence(knowledgeGraphItem.s, article.Text);

                    int relationPassageOccurence = WordOccurrence(knowledgeGraphItem.r, passages[i]);
                    int relationDocumentOccurence = WordOccurrence(knowledgeGraphItem.r, article.Text);

                    int targetPassageOccurence = WordOccurrence(knowledgeGraphItem.t, passages[i]);
                    int targetDocumentOccurence = WordOccurrence(knowledgeGraphItem.t, article.Text);

                    double evidenceSource = EvidenceCalculator(passageLength, article.Lenght, article.UniqueLenght, sourcePassageOccurence, sourceDocumentOccurence);
                    double evidenceRelation = EvidenceCalculator(passageLength, article.Lenght, article.UniqueLenght, relationPassageOccurence, relationDocumentOccurence);
                    double evidenceTarget = EvidenceCalculator(passageLength, article.Lenght, article.UniqueLenght, targetPassageOccurence, targetDocumentOccurence);
                    double passageScore = evidenceSource * evidenceRelation * evidenceTarget;
                    rankedPassages.Add(new TMWIISItem(passageScore, passages[i], article.Link));
                }
            }
            rankedPassages.Sort((p, q) => q.score.CompareTo(p.score));
            return rankedPassages.Take(maxPassages).ToList();
        }
        public List<string> GetPassages(string text)
        {
            PassageRetrievalHandler pr = new PassageRetrievalHandler(text);
            return pr.GetPassages();
        }
        public double EvidenceCalculator(int passageLength, int ArticleLength, int UniqueLength, int passageOccurrence, int DocumentOccurrence)
        {
            //calculates the evidence for the Source Entity
            double passageSource, documentSource, collectionSource;
            double lambda1 = 0.4, lambda2 = 0.4, lambda3 = 0.2;

            //passage
            passageSource = (passageOccurrence + 1) / (passageLength + UniqueLength);
            //document
            documentSource = (DocumentOccurrence + 1) / (passageLength + UniqueLength);
            //collection
            collectionSource = DocumentOccurrence / UniqueLength;
            //probability
            return lambda1 * passageSource * lambda2 * documentSource
                * lambda3 * collectionSource;
        }
        public int PassageLength(string passage)
        {
            int length = passage.Split(' ').ToList().Count;
            return length;
        }
        public int WordOccurrence(string entity, string passage)
        {
            List<string> passageWords = passage.Split(" ").ToList();
            List<string> entityList = entity.Split(" ").ToList();
            int length = passageWords.Count;
            int occurrences = 0;
            for(int j = 0; j < entityList.Count; j++)
            {
                for (int i = 0; i < length; i++)
                {
                    if (passageWords[i] == entityList[j])
                    {
                        occurrences++;
                    }
                }
            }
            return occurrences;
        }
    }
}
