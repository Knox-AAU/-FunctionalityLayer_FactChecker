using System;
using System.Collections.Generic;

namespace FactChecker.DataBase.WordRatios
{
    public class Document
    {
        public Category Category { get; set; }
        public DataSource Publisher { get; set; }
        public int Id { get; set; }
        public string Headline { get; set; }
        public string Author { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Path { get; set; }
        public int TotalWords { get; set; }
        public string Summary { get; set; }

        public ICollection<WordRatio> WordRatios { get; set; }
        public ICollection<DocumentContent> DocumentContents { get; set; }
        public ICollection<NearestDocument> SimilarDocuments { get; set; }

        public Document()
        {
            this.WordRatios = new HashSet<WordRatio>();
            this.DocumentContents = new HashSet<DocumentContent>();
            this.SimilarDocuments = new HashSet<NearestDocument>();
        }

    }
}
