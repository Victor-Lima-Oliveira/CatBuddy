﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CatBuddy.LibrariesSessao.Filtro
{
    public class ValidateHttpRefererAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            string referer = context.HttpContext.Request.Headers["Referer"].ToString();

            if (String.IsNullOrEmpty(referer))
            {
                context.Result = new ContentResult() { Content = "Acesso negado!" };
            }
            else
            {
                Uri uri = new Uri(referer);

                string hostReferer = uri.Host;
                string hostServidor = context.HttpContext.Request.Host.Host;

                if(hostReferer != hostServidor)
                {
                    context.Result = new ContentResult() { Content = "Acesso negado!" };
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
