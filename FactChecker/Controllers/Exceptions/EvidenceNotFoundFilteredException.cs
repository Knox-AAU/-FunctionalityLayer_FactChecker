namespace FactChecker.Controllers.Exceptions
{
    public class EvidenceNotFoundFilteredException : FilteredException
    {
        public EvidenceNotFoundFilteredException(string evidence, int statusCode = 454) : base($"Evidence '{evidence}' was not found", statusCode)
        {
        }
    }
}
