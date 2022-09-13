using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.PassageRetrieval
{
    public class PassageRetrievalHandler
    {
        private int _passageOverlap = 20;
        public int PassageLength { get; set; } = 80;
        public string FullText { get; set; }

        public int PassageOverlap
        {
            get
            {
                return _passageOverlap;
            }
            set
            {
                if(value > PassageLength)
                {
                    throw new ArgumentOutOfRangeException("Passage overlap can not be greater than passage length");
                }
                _passageOverlap = PassageLength - (PassageLength - value);
            }
        }

        /// <summary>
        /// Contructor taking one argument of type <paramref name="string"/>. Used for creating new passages.
        /// </summary>
        /// <param name="text"></param>
        public PassageRetrievalHandler(string text)
        {
            FullText = text;
        }

        public override string ToString()
        {
            return FullText;
        }

        /// <summary>
        /// Method used to create passages from the best ranked articles.
        /// </summary>
        /// <returns>A list of passages</returns>
        public List<string> GetPassages()
        {
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
    }
}