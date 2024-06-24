using CatBuddy.LibrariesSessao.Login;
using CatBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CatBuddy.LibrariesSessao.Filtro
{
    public class ColaboradorAutorizacaoAttribute : Attribute, IAuthorizationFilter
    {
        LoginColaborador _loginColaborador;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Recupera o serviço de login
            _loginColaborador = (LoginColaborador) context.HttpContext.RequestServices.GetService(typeof(LoginColaborador));

            // Recupera os dados do usuario 
            Colaborador colaborador = _loginColaborador.GetColaborador();

            // Se não receber o colaborador
            if(colaborador == null) 
            {
                context.Result = new RedirectToActionResult("Login", "Colaborador", null);
            }
            
        }
    }
}
