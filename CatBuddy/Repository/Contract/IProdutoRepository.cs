using CatBuddy.Models;

namespace CatBuddy.Repository.Contract
{
    public interface IProdutoRepository
    {
        public void insereProduto(Produto produto);
        public void deletaProduto(int id);
        public void atualizaProduto(Produto produto);
        public Produto retornaProduto(int id);
        public IEnumerable<Produto> retornaProdutos();
        public void VendeProduto(int codProduto, int qtdEstoque);
        public InfoNutricionais RetornaInformacacoesNutricionais(int codProduto);

    }
}
