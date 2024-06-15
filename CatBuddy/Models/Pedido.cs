namespace CatBuddy.Models
{
    public class Pedido
    {
        public int cod_id_pedido { get; set; }
        public int cod_cliente { get; set; }
        public int cod_pagamento { get; set; }
        public double valorTotal { get; set; }
        public DateTime dataPedido { get; set; }
    }
}
