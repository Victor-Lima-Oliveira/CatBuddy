using CatBuddy.Models;

namespace CatBuddy.Repository.Contract
{
    public interface IClienteRepository
    {
        // Login
        Cliente Login(string Email, string Senha);

        // CRUD
        void Cadastrar(Cliente cliente);
        void Atualizar(Cliente cliente);
        void Excluir(int id);
        Cliente ObterCliente(int id);
        IEnumerable<Cliente> ObterTodosClientes();
    }
}
