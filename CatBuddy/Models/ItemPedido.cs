namespace CatBuddy.Models
{
    public class ItemPedido
    {
        public int cod_produto { get; set; }
        public int cod_pedido { get; set; }
        public int cod_cliente { get; set; }
        public int qtd { get; set; }
        public double subtotal { get; set; }
    }
}
