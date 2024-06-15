using CatBuddy.Models;

namespace CatBuddy.Repository.Contract
{
    public interface IProdutoRepository
    {
        public int insereProduto(Produto produto);
        public void deletaProduto(int id);
        public void atualizaProduto(Produto produto);
        public Produto retornaProduto(int id);
        public IEnumerable<Produto> retornaProdutos(Produto produto = null);
        public void VendeProduto(int codProduto, int qtdEstoque);
        public List<Categoria> RetornaCategorias();
        public List<Fornecedor> RetornaFornecedores();

    }
}
