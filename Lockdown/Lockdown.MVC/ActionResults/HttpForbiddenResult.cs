using System;
using System.Web.Mvc;

namespace Lockdown.MVC.ActionResults
{
    public class HttpForbiddenResult : ContentResult
    {
        public HttpForbiddenResult()
        {
            Content = "403 Forbidden";
            ContentType = "text/plain";
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.HttpContext.Response.StatusCode = 403;
            base.ExecuteResult(context);
        }
    }
}