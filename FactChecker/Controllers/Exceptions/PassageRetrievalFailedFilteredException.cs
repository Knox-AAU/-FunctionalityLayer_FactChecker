namespace FactChecker.Controllers.Exceptions
{
    public class PassageRetrievalFailedFilteredException : FilteredException
    {
        public PassageRetrievalFailedFilteredException(string message, int statusCode = 512) : base($"'{message}' ... See inner exception for more information", statusCode)
        {
        }
    }
}
