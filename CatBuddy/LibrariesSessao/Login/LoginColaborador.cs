using CatBuddy.Models;
using Newtonsoft.Json;

namespace CatBuddy.LibrariesSessao.Login
{
    public class LoginColaborador
    {

        private string _Key = "Login.Colaborador";
        private Sessao _sessao;
        public LoginColaborador(Sessao sessao)
        {
            _sessao = sessao;
        }

        public void Login(Colaborador colaborador)
        {
            // Transforma a model em string para salvar na sessão
            string colaboradorJSONString = JsonConvert.SerializeObject(colaborador);

            // Cadastra o usuario na sessao
            _sessao.Cadastar(_Key, colaboradorJSONString);
        }

        public Colaborador GetColaborador()
        {
            // Se existe sessão
            if (_sessao.Existe(_Key))
            {
                // Recupera o colaborador da sessão
                string colaboradorJSONString = _sessao.Consultar(_Key);

                // Retorna a model da sessão
                return JsonConvert.DeserializeObject<Colaborador>(colaboradorJSONString);
            }
            else
            {
                return null;
            }
        }

        public void Logout()
        {
            // Remove o login
            _sessao.RemoverTodos();
        }
    }
}
