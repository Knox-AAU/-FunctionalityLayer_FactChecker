namespace FactChecker.Controllers.Exceptions
{
    public class ArticleNotFoundFilteredException : FilteredException
    {
        public ArticleNotFoundFilteredException(string article, int statusCode = 453) : base($"Article '{article}' was not found", statusCode)
        {
        }
    }
}
