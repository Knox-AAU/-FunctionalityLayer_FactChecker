using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FactChecker.APIs.KnowledgeGraphAPI;
using FactChecker.Controllers.Exceptions;
using FactChecker.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactChecker.PassageRetrieval
{
    public class PassageRetrievalHandler : IPassageRetrieval
    {
        public int PassageLength { get; set; } = 80;
        private string FullText { get; set; }
        public int PassageOverlap { get; set; } = 20;

        /// <summary>
        /// Method used to create passages from the best ranked articles.
        /// </summary>
        /// <returns>A list of passages</returns>
        private List<string> GetPassages(string text)
        {
            if (PassageOverlap > PassageLength)
                throw new PassageRetrievalFailedFilteredException("'PassageOverlap' can not be higher than 'PassageLength'");
            FullText = text;
            // errors when splitting since some texts does not have a space '"universe.A"'
            List<string> passages = new();
            List<string> splitText = FullText.Split(' ').ToList();
            string passage = "";
            int length = splitText.Count;
            int count = 0;


            for (int i = 0; i < length; i++)
            {
                if (i == length - 1) //If the end of splitText is found, add the rest of splitText to a passage
                {
                    passage += " " + splitText[i];
                    passages.Add(passage);
                }else if (count == PassageLength) //If count succesfully reaches PassageLength, add passage of correct length
                {
                    passage += " " + splitText[i];
                    passages.Add(passage);
                    passage = "";
                    i -= PassageOverlap; //Go back the amount you wish to overlap and start counting again
                    count = 0;
                }
                else //Make sure there are spaces between each word in the passage.
                {
                    if(count > 1)
                    {
                        passage += " ";
                    }
                    passage += splitText[i];
                }
                count++;
            }
            return passages;
        }
        
        public List<string> GetPassage_new(string text)
        {
            if (PassageOverlap > PassageLength)
                throw new PassageRetrievalFailedFilteredException("'PassageOverlap' can not be higher than 'PassageLength'");
            List<string> passages = new();
            List<string> new_text = text.Split(" ").ToList();
            for (int i = 0; i < new_text.Count; i += PassageLength)
                passages.Add(string
                    .Join(" ", new_text
                    .Skip(i - PassageOverlap <= 0 ? 0 : i - PassageOverlap)
                    .Take(i + PassageLength > new_text.Count - 1 ? new_text.Count - 1 : i + PassageLength)
                    .ToArray()));
            return passages;
        }
        
        public List<string> GetPassage_new_V2(string text)
        {
            if (PassageOverlap > PassageLength)
                throw new PassageRetrievalFailedFilteredException("'PassageOverlap' can not be higher than 'PassageLength'");
            List<string> passages = new();
            List<string> new_text = text.Split(" ").ToList();
            for (int i = 0; i < new_text.Count; i += PassageLength - PassageOverlap)
            {
                if (i > 0)
                    new_text.RemoveRange(0, PassageOverlap);
                passages.Add(string
                    .Join(" ", new_text.Skip(i <= 0 ? 0 : i).Take(i + PassageLength > new_text.Count - 1 ? new_text.Count - 1 : i + PassageLength)
                    .ToArray()));
            }
            return passages;
        }
        public List<string> GetPassage_new_V3(string text)
        {
            if (PassageOverlap > PassageLength)
                throw new PassageRetrievalFailedFilteredException("'PassageOverlap' can not be higher than 'PassageLength'");
            List<string> passages = new();
            List<string> new_text = text.Split(" ").ToList();
            int i = 0;
            while (i < new_text.Count)
            {
                if (i > 0)
                    new_text.RemoveRange(0, Math.Min(PassageOverlap, new_text.Count));
                StringBuilder sb = new();
                for (int j = Math.Max(0, i); j < Math.Min(new_text.Count - 1, i + PassageLength); j++)
                {
                    sb.Append(' ');
                    sb.Append(new_text[j]);
                }
                passages.Add(sb.ToString());
                i += PassageLength - PassageOverlap;
            }
            return passages;
        }

       
    public IEnumerable<Passage> GetPassages(Article article) =>
            GetPassage_new_V3(article.FullText).Select(p => new Passage
                { FullPassage = p });
    }
}