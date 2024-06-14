namespace CatBuddy.Repository.Contract
{
    public interface IPedidoRepository
    {
        public int CadastrarPedido(int codUsuario, float valorTotal, int codPagamento);
        public void CadastrarItemPedido(int codPedido, int codProduto, int codCliente, int Qtd, float subtotal);
        public void FinalizarPedido(int codPedido, int codPagamento);
    }
}
