using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.PassageRetrieval
{
    public class PassageItem
    {
        private int passageLength = 80;
        private string _fullText;

        public string FullText
        {
            get
            {
                return _fullText;
            }
            set
            {
                if(value.Split(' ').Length < 80)
                {
                    throw new ArgumentOutOfRangeException("The text can not be empty");
                }
                _fullText = value;
            }
        }

        public PassageItem(string text)
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
            string[] splittedText = FullText.Split(' ');
            string passage = "";
            int length = splittedText.Length;


            for (int i = 0; i < length; i++)
            {
                if (i == length - 1)
                {
                    passage += splittedText[i];
                    passages.Add(passage);
                }
                if (i % passageLength == 0)
                {
                    passage += splittedText[i];
                    passages.Add(passage);
                    passage = "";
                }
                else
                {
                    passage += splittedText[i];
                }
            }

            return passages;
        }
    }
}