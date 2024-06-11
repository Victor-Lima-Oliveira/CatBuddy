using CatBuddy.LibrariesSessao.Login;
using CatBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CatBuddy.LibrariesSessao.Filtro
{
    public class ClienteAutorizacaoAttribute : Attribute, IAuthorizationFilter
    {
        LoginCliente _loginCliente;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Recupera o contexto do login para a classe 
            _loginCliente = (LoginCliente)context.HttpContext.RequestServices.GetService(typeof(LoginCliente));

            // Obtem os dados do usuário
            Cliente cliente = _loginCliente.ObterCliente();

            // Se não tiver usuário logado, retorna um contexto de erro
            if(cliente == null)
            {
                context.Result = new ContentResult() { Content = "Acesso negado." };
            }
        }
    }
}
