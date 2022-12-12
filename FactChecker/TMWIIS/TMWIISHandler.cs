using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactChecker.TFIDF;
using FactChecker.PassageRetrieval;
using FactChecker.APIs.KnowledgeGraphAPI;
using FactChecker.Interfaces;
using FactChecker.Controllers.Exceptions;
using FactChecker.WordcountDB;

namespace FactChecker.TMWIIS
{
    public class TMWIISHandler
    {
        private readonly WordcountDB.WordCount wordCount;
        private readonly stopwords sw;
        private IEnumerable<string> stopwordslist;

        public TMWIISHandler(WordCount wordCount, WordcountDB.stopwords sw)
        {
            this.wordCount = wordCount;
            this.sw = sw;
            stopwordslist = sw.GetStopwords().Select(p => p.word);

        }

        readonly float lambda1 = 0.9f * 100000, lambda2 = 0.05f * 100000, lambda3 = 0.05f * 100000;
        public float CalculateScore(KnowledgeGraphItem knowledgeGraphItem, Passage passage, string FullText, int FulltText_Unique)
        {
            int sourceTotalOccurence = GetNumberOfOccurencesInAllDocuments(knowledgeGraphItem.s);
            int relationTotalOccurence = GetNumberOfOccurencesInAllDocuments(knowledgeGraphItem.r);
            int targetTotalOccurence = GetNumberOfOccurencesInAllDocuments(knowledgeGraphItem.t);

            int sourceDocumentOccurence = WordOccurrence(knowledgeGraphItem.s, FullText);
            int relationDocumentOccurence = WordOccurrence(knowledgeGraphItem.r, FullText);
            int targetDocumentOccurence = WordOccurrence(knowledgeGraphItem.t, FullText);
            int passageLength = PassageLength(passage.FullPassage);

            int sourcePassageOccurence = WordOccurrence(knowledgeGraphItem.s, passage.FullPassage);
            int relationPassageOccurence = WordOccurrence(knowledgeGraphItem.r, passage.FullPassage);
            int targetPassageOccurence = WordOccurrence(knowledgeGraphItem.t, passage.FullPassage);
                    
            float evidenceSource = EvidenceCalculator(passageLength, FulltText_Unique, sourcePassageOccurence, sourceDocumentOccurence, sourceTotalOccurence);
            float evidenceRelation = EvidenceCalculator(passageLength, FulltText_Unique, relationPassageOccurence, relationDocumentOccurence, relationTotalOccurence);
            float evidenceTarget = EvidenceCalculator(passageLength, FulltText_Unique, targetPassageOccurence, targetDocumentOccurence, targetTotalOccurence);

            float passageScore = lambda1 * evidenceSource + lambda2 * evidenceRelation + lambda3 * evidenceTarget;
            return Math.Abs(passageScore);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="passageLength"></param>
        /// <param name="uniqueLength"></param>
        /// <param name="passageOccurrence">Passage word Occurence</param>
        /// <param name="documentOccurrence">Document word Occurence</param>
        /// <param name="totalOccurrence">Total docuement Occurences</param>
        /// <returns></returns>
        private float EvidenceCalculator(int passageLength, int uniqueLength, int passageOccurrence, int documentOccurrence, int totalOccurrence)
        {
            float passageSource = (passageOccurrence + 1) / (float)(passageLength + uniqueLength);
            float documentSource = (documentOccurrence + 1) / (float)(passageLength + uniqueLength);
            float collectionSource = totalOccurrence / (float) uniqueLength;
            return  passageSource  * documentSource * collectionSource;
        }

        private int GetNumberOfOccurencesInAllDocuments (string word)
        {
            List<string> splitted = word.Split(' ').ToList();           
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
                {
                    if (passageWords[i] == entityList[j] && !stopwordslist.Contains(passageWords[i]))
                        occurrences++;
                }

            return occurrences;
        }
    }
}
