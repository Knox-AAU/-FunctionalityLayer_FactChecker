namespace FactChecker.APIs
{
    /// <summary>
    /// A class containing fields for everything retrievable from Knox articles. 
    /// </summary>
    public class SearchItem
    {
        public int articleId;
        public string word;
        public int count;
        public string title;
        public string filePath;
        public int totalWords;
        public string publisherName;
        public float percent;
    }
}
