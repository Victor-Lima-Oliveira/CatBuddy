namespace CatBuddy.Models
{
    public class ViewModelProduto
    {
        public Produto produto { get; set; }
        public List<Categoria>? listCategoria { get; set; }
        public List<Fornecedor>? listFornecedor { get; set; }
    }
}
