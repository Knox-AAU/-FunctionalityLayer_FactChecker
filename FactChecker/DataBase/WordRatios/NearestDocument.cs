namespace FactChecker.DataBase.WordRatios
{
    public class NearestDocument
    {
        public Document Main { get; set; }
        public Document Similar { get; set; }
        public float Similarity { get; set; }
    }
}
