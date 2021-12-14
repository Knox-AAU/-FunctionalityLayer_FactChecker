using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.PassageRetrieval
{
    public class PassageRetrievalHandler
    {
        private int _passageLength = 80;
        private int _passageOverlap = 20;
        private string _fullText;

        public string FullText
        {
            get
            {
                return _fullText;
            }
            set
            {
                _fullText = value;
            }
        }

        public int PassageLength
        {
            get
            {
                return _passageLength;
            }
            set
            {
                _passageLength = value;
            }
        }

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
                _passageOverlap = _passageLength - (_passageLength - value);
            }
        }

        public PassageRetrievalHandler(string text)
        {
            FullText = text;
        }

        public override string ToString()
        {
            return FullText;
        }

        public List<string> GetPassages()
        {
            List<string> passages = new List<string>();
            List<string> splittedText = FullText.Split(' ').ToList();
            string passage = "";
            int length = splittedText.Count;
            int count = 0;


            for (int i = 0; i < length; i++)
            {
                if (i == length - 1)
                {
                    passage += " " + splittedText[i];
                    if (passage.Split(' ').Length == PassageLength)
                    {
                        passages.Add(passage);
                    }
                    else
                    {
                        i -= PassageLength;
                        count = 0;
                        passage = "";
                    }
                }
                else if (count == PassageLength)
                {
                    passage += " " + splittedText[i];
                    passages.Add(passage);
                    passage = "";
                    i -= PassageOverlap;
                    count = 0;
                }
                else
                {
                    if(count > 1)
                    {
                        passage += " ";
                    }
                    passage += splittedText[i];
                }
                count++;
            }

            return passages;
        }
    }
}