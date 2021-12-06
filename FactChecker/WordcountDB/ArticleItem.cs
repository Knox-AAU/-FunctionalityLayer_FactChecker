using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.WordcountDB
{
    public class ArticleItem
    {
        public int ID { get; set; }
        public string Link { get; set; }
        public int Lenght { get; set; }
        public int UniqueLenght { get; set; }
        public string Text { get; set; }


        public ArticleItem(int id, string link, int lenght, int unique_lenght, string text)
        {
            ID = id;
            Link = link;
            Lenght = lenght;
            UniqueLenght = unique_lenght;
            Text = text;
        }
        public override string ToString()
        {
            return "ID:" + ID + " Link:" + Link + " Lenght:" + Lenght + " Unique Lenght:" + UniqueLenght + " Text:" + Text;
        }
    }
}
