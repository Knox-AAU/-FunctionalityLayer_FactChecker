using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace FactChecker.Controllers.Exceptions
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
                if (context.Exception is PassageNotFoundFilteredException alreadyexistsexception)
                    TriggerExceptionFiltered(context, alreadyexistsexception);
                else if (context.Exception is ArticleNotFoundFilteredException articleNotFoundFilteredException)
                    TriggerExceptionFiltered(context, articleNotFoundFilteredException);
                else if (context.Exception is EvidenceNotFoundFilteredException evidenceNotFoundFilteredException)
                    TriggerExceptionFiltered(context, evidenceNotFoundFilteredException);
                else if (context.Exception is PassageRetrievalFailedFilteredException passageRetrievalFailedFilteredException)
                    TriggerExceptionFiltered(context, passageRetrievalFailedFilteredException);
                else
                    Fallback(context);
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // we need to do nothing here
        }

        private static void Fallback(ActionExecutedContext context)
        {
            context.Result = new ObjectResult(new FilteredException("Unhandled exception: " + (context?.Exception?.GetType()?.ToString())))
            {
                StatusCode = 500,
                Value = context?.Exception.ToString()
            };
        }

        private static void TriggerExceptionFiltered(ActionExecutedContext context, FilteredException exception)
        {
            context.Result = new ObjectResult(exception.InnerException)
            {
                StatusCode = exception.StatusCode,
                Value = exception.ToString(),
            };
            context.ExceptionHandled = true;
            
        }
    }
    public interface ITrackChangeException
    {
        public int StatusCode { get; }
        public HttpResponse Value { get; set; }
    }
}
