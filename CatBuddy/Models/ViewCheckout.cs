namespace CatBuddy.Models
{
    public class ViewCheckout
    {
        public List<Produto> ListProduto { get; set; }
        public Endereco Endereco { get; set; }
        public int codPagamento { get; set; }

        public string getTotal(bool IsFrete = false)
        {
            double precoaux = 0;
            foreach (var item in ListProduto)
            {
                precoaux += item.Preco.Value;
            }

            if(IsFrete)
            {
                precoaux += 10;
            }

            return $"R$ {precoaux:N}";
        }
    }
}
