namespace CatBuddy.Models
{
    public class ViewCheckout
    {
        public List<Produto> ListProduto { get; set; }
        public Endereco Endereco { get; set; }

        public string getTotal()
        {
            double precoaux = 0;
            foreach (var item in ListProduto)
            {
                precoaux += item.Preco.Value;
            }

            return $"R$ {precoaux:N}";
        }
    }
}
