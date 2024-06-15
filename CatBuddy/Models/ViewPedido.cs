using System.ComponentModel.DataAnnotations;

namespace CatBuddy.Models
{
    public class ViewPedido
    {
        public Pedido Pedido { get; set; }

        public string nomeUsuario {  get; set; }

        [Display(Name = "Forma de pagamento")]
        public string nomepagamento { get; set; }
        public int qtdProdutos { get; set; }
    }
}
