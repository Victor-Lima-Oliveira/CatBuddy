using Microsoft.AspNetCore.Antiforgery;

namespace CatBuddy.LibrariesSessao.Middleware
{
    public class ValidateAntiForgeryTokenMiddleware
    {
        private RequestDelegate _requestDelegate;
        private IAntiforgery _antiforgery;

        public ValidateAntiForgeryTokenMiddleware(RequestDelegate requestDelegate, IAntiforgery antiforgery)
        {
            _requestDelegate = requestDelegate;
            _antiforgery = antiforgery;
        }   

        public async Task Invoke(HttpContext context)
        {
            if(HttpMethods.IsPost(context.Request.Method))
            {
                await _antiforgery.ValidateRequestAsync(context);
            }

            await _requestDelegate(context);
        }
    }
}
