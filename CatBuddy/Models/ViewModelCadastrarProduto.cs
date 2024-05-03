namespace CatBuddy.Models
{
    public class ViewModelCadastrarProduto
    {
        public Produto produto { get; set; }
        public InfoNutricionais? infoNutricionais { get; set; }
        public List<Categoria>? listCategoria { get; set; }
        public List<Fornecedor>? listFornecedor { get; set; }
    }
}
