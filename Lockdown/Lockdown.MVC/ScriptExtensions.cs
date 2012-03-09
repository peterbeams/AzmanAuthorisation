using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Lockdown.MVC.Client;
using Lockdown.MVC.Tokens;

namespace Lockdown.MVC
{
    public static class ScriptExtensions
    {
        internal static IAuthorizationClientFactory ClientFactory;
        internal static ITokenFactory TokenFactory;
        internal static string AppName;

        public static IHtmlString LockdownScriptBlock(this HtmlHelper html)
        {
            var ops = new List<string>(OperationStore.Current(AppName, ClientFactory, TokenFactory).Values);
            
            foreach (var o in ops.Where(o => o.EndsWith(".Index", StringComparison.InvariantCultureIgnoreCase) || o.EndsWith(".Index[POST]", StringComparison.InvariantCultureIgnoreCase)).ToArray())
            {
                ops.Add(o.Replace(".Index", string.Empty));
            }

            var block = new StringBuilder();

            var urls = ops.Select(op => string.Concat("/", op.Replace("Controller", ".").Replace(".", "/")).Replace("//", "/"));

            var forms = urls.Where(u => u.EndsWith("[POST]")).Select(u => u.Replace("[POST]", string.Empty)).ToArray();
            var links = urls.Where(u => !u.EndsWith("[POST]")).ToArray();

            block.AppendLine(@"<script language=""javascript"" type=""text/javascript"">");
            block.AppendLine(@"var formUrls = new Array();");
            block.AppendLine(@"var links = new Array();");
            for (var i = 0; i < forms.Length; i++)
            {
                if (forms[i].EndsWith("/"))
                {
                    forms[i] = forms[i].Substring(0, forms[i].Length - 1);
                }
                block.AppendLine(string.Format(@"formUrls[{0}] = ""{1}"";", i, forms[i]));
            }
            for (var i = 0; i < links.Length; i++)
            {
                if (links[i].EndsWith("/"))
                {
                    links[i] = links[i].Substring(0, links[i].Length - 1);
                }
                block.AppendLine(string.Format(@"links[{0}] = ""{1}"";", i, links[i]));
            }
            block.AppendLine("</script>");

            return html.Raw(block.ToString());
        }
    }
}
