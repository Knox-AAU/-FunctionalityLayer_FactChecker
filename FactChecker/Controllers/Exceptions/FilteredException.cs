using System;

namespace FactChecker.Controllers.Exceptions
{
    public class FilteredException : Exception
    {
        public int StatusCode { get; set; }
        public FilteredException(string message, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
