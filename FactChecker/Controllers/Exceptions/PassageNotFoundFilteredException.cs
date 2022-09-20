namespace FactChecker.Controllers.Exceptions
{
    public class PassageNotFoundFilteredException : FilteredException
    {
        public PassageNotFoundFilteredException(string passage, int statusCode = 452) : base($"Passage '{passage}' was not found", statusCode)
        {
        }
    }
}
