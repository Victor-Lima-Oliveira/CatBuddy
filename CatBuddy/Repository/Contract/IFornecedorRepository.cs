using CatBuddy.Models;

namespace CatBuddy.Repository.Contract
{
    public interface IFornecedorRepository
    {
        void Cadastrar(Fornecedor fornecedor);
        void Atualizar(Fornecedor fornecedor);
        void Excluir(int Id);
        ViewFornecedor ObterFornecedor(int Id);
        List<ViewFornecedor> ObterFornecedores();
    }
}
