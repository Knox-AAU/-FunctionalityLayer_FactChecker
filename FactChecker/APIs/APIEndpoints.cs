namespace FactChecker.APIs
{
    //This class includes a reference to all needed KNOX-endpoints
    public static class APIEndpoints
    {

        //Search API
        public static string wordcount = "http://knox-node02.srv.aau.dk/wordcount/";
        public static string wordratio = "http://knox-node02.srv.aau.dk/wordratio/";

        //Lemmatizer
        public static string lemmatizer = "http://knox-master01.srv.aau.dk/lemmatizer";

    }
}
