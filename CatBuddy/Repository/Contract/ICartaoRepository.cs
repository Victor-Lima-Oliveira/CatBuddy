using CatBuddy.Models;

namespace CatBuddy.Repository.Contract
{
    public interface ICartaoRepository
    {
        public void Cadastrar(Cartao cartao);
        public void Atualizar(Cartao cartao);
        public void Excluir(int idPagamento);
        Cartao ObtemCartao(int id);
        List<Cartao> ObtemCartoes(int idCliente);
    }
}
