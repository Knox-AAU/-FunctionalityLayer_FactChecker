using System.Collections.Generic;

namespace FactChecker.DataBase.WordRatios
{
    public class DataSource
    {
        public int Id { get; set; }
        public string Publication { get; set; }
        public string Publisher { get; set; }


        public ICollection<Document> Documents { get; set; }

        public DataSource()
        {
            this.Documents = new HashSet<Document>();
        }

    }
}
