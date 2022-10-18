using System.Collections.Generic;

namespace FactChecker.DataBase.WordRatios
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Document> Documents { get; set; }

        public Category()
        {
            Documents = new HashSet<Document>();
        }
    }
}
