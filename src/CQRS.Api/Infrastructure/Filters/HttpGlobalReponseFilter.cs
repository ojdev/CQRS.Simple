using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CQRS.Api.Infrastructure.Filters
{
    public class HttpGlobalReponseFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult @object)
            {
                context.Result = new ObjectResult(new ResponseWrap(@object?.Value));
            }
            else if (context.Result is EmptyResult empty)
            {
                context.Result = new ObjectResult(new ResponseWrap());
            }
            else if (context.Result is JsonResult json)
            {
                context.Result = new ObjectResult(new ResponseWrap(json?.Value));
            }
            else if (context.Result is ContentResult content)
            {
                context.Result = new ObjectResult(new ResponseWrap(content?.Content));
            }
            else if (context.Result is StatusCodeResult status)
            {
                context.Result = new ObjectResult(new ResponseWrap());
            }
        }
    }

}
