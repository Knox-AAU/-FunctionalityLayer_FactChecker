namespace FactChecker.DataBase.WordRatios
{
    public class WordRatio
    {
        public Document Document { get; set; }
        public string Word { get; set; }
        public int Occurence { get; set; }
        public float Percent { get; set; }
        public int Rank { get; set; }
        public float ClusteringScore { get; set; }
    }
}
