using System.ComponentModel.DataAnnotations;

namespace CatBuddy.Models
{
    public class Pedido
    {
        [Display(Name = "Código do pedido")]
        public int cod_id_pedido { get; set; }
        public int cod_cliente { get; set; }
        public int cod_pagamento { get; set; }
        [Display(Name = "Total")]
        public double valorTotal { get; set; }
        [Display(Name = "Data da compra")]
        public DateTime dataPedido { get; set; }
        public List<ItemPedido> listItensPedido { get; set; }
    }
}
