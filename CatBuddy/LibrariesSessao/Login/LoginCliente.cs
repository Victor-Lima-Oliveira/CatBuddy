using CatBuddy.Models;
using MySqlX.XDevAPI;
using Newtonsoft.Json;

namespace CatBuddy.LibrariesSessao.Login
{
    public class LoginCliente
    {
        private string _key = "Login.Cliente";
        private Sessao _sessao;

        /// <summary>
        /// Construtor que recupera os dados da sessão
        /// </summary>
        public LoginCliente(Sessao sessao)
        {
            _sessao = sessao;
        }

        public void Login(Cliente cliente)
        {
            // Converte os dados do cliente para string 
            string clienteJsonString = JsonConvert.SerializeObject(cliente);

            // Cadastra os dados do usuário na sessão
            _sessao.Cadastar(_key, clienteJsonString);
        }

        public Cliente ObterCliente()
        {
            string clienteJsonString;

            // Se possui sessão ativa
            if (_sessao.Existe(_key))
            {
                // Recupera os dados da sessão
                clienteJsonString = _sessao.Consultar(_key);

                // Retorna o objeto
                return JsonConvert.DeserializeObject<Cliente>(clienteJsonString);
            }
            else
            {
                return null;
            }
        }

        public void Logout()
        {
            _sessao.RemoverTodos();
        }
    }
}
