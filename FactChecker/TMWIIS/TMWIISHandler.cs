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
        public List<int> ArticleID;
        public KnowledgeGraphItem KGItem;
        public TMWIISHandler(List<int> articleID, KnowledgeGraphItem KGitem)
        {
            ArticleID = articleID;
            KGItem = KGitem;
        }
        public List<TMWIISItem> Evidence()
        {
            List<TMWIISItem> RankedPassages = new List<TMWIISItem>();
            WordcountDB.Article articleText = new WordcountDB.Article();

            for(int j = 0; j < ArticleID.Count; j++)
            {
                WordcountDB.ArticleItem Article = articleText.FetchDB(ArticleID[j]);
                List<string> passages = GetPassages(Article.Text);
                for (int i = 0; i < passages.Count; i++)
                {
                    int passageLength = PassageLength(passages[i]);

                    int SourcePassage = WordOccurrence(KGItem.s, passages[i]);
                    int SourceDocument = WordOccurrence(KGItem.s, Article.Text);

                    int RelationPassage = WordOccurrence(KGItem.r, passages[i]);
                    int RelationDocument = WordOccurrence(KGItem.r, Article.Text);

                    int TargetPassage = WordOccurrence(KGItem.t, passages[i]);
                    int TargetDocument = WordOccurrence(KGItem.t, Article.Text);

                    double EvidenceS = EvidenceCalculator(passageLength, Article.Lenght, Article.UniqueLenght, SourcePassage, SourceDocument);
                    double EvidenceR = EvidenceCalculator(passageLength, Article.Lenght, Article.UniqueLenght, RelationPassage, RelationDocument);
                    double EvidenceT = EvidenceCalculator(passageLength, Article.Lenght, Article.UniqueLenght, TargetPassage, TargetDocument);
                    double PassageScore = EvidenceS * EvidenceR * EvidenceT;
                    RankedPassages.Add(new TMWIISItem(PassageScore, passages[i], Article.Link));
                }
            }
            RankedPassages.Sort((p, q) => q.score.CompareTo(p.score));
            return RankedPassages.Take(maxPassages).ToList();
        }
        public List<string> GetPassages(string text)
        {
            PassageRetrievalHandler pr = new PassageRetrievalHandler(text);
            return pr.GetPassages();
        }
        public double EvidenceCalculator(int passageLength, int ArticleLength, int UniqueLength, int passageOccurrence, int DocumentOccurrence)
        {
            //WordcountDB.Article article = new WordcountDB.Article();
            //WordcountDB.ArticleItem item = article.FetchDB(ArticleID);

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
        public int WordOccurrence(string Entity, string passage)
        {
            List<string> passageWords = passage.Split(" ").ToList();
            List<string> EntityList = Entity.Split(" ").ToList();
            int length = passageWords.Count;
            int occurrences = 0;
            for(int j = 0; j < EntityList.Count; j++)
            {
                for (int i = 0; i < length; i++)
                {
                    if (passageWords[i] == EntityList[j])
                    {
                        occurrences++;
                    }
                }
            }
            return occurrences;
        }
    }
}
