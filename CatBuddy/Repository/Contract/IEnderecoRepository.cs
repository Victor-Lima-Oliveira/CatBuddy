using CatBuddy.Models;

namespace CatBuddy.Repository.Contract
{
    public interface IEnderecoRepository
    {
        public void Cadastrar(Endereco endereco);
        public void Atualizar(Endereco endereco);
        public void Excluir(int id);
        Endereco ObtemEndereco(int id);
        List<ViewEndereco> ObtemEnderecos(int id);

    }
}
