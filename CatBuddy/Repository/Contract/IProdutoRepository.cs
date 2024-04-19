using CatBuddy.Models;

namespace CatBuddy.Repository.Contract
{
    public interface IProdutoRepository
    {
        void insereProduto(Produto produto);
        void deletaProduto(int id);
        void atualizaProduto(Produto produto);
        Produto retornaProduto(int id);
        IEnumerable<Produto> retornaProdutos();
    }
}
