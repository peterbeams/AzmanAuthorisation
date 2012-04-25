using System;
using System.Web;
using System.Web.Mvc;

namespace Lockdown.MVC.ActionResults
{
    public class HttpForbiddenResult : ContentResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            throw new HttpException(403, "Forbidden");
        }
    }
}