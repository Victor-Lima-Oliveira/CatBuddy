namespace CatBuddy.Models
{
    public class ViewItensPedido
    {
        public ItemPedido ItemPedido { get; set; }
        public string nomeProduto { get; set; }
        public double valorTotal { get; set; }
        public DateTime datapedido {  get; set; }
        public string imgPath { get; set; }
        public bool isprodutoativo { get; set; }
    }
}
