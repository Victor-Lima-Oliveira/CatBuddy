using System.ComponentModel.DataAnnotations;

namespace CatBuddy.Models
{
    public class ItemPedido
    {
        public int cod_produto { get; set; }
        public int cod_pedido { get; set; }
        public int cod_cliente { get; set; }
        [Display(Name = "Quantidade")]
        public int qtd { get; set; }

        [Display(Name = "Subtotal")]
        public double subtotal { get; set; }

        public string getSubtotal()
        {
            return subtotal.ToString("F2");
        }
        public string getPrecoUnitario()
        {
            return (subtotal / qtd).ToString("F2");
        }
    }
}
