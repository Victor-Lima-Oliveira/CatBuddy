using CatBuddy.Models;

namespace CatBuddy.Repository.Contract
{
    public interface IColaboradorRepository
    {
        // Login Colaborador
        Colaborador Login(string Email, string Senha);

        // CRUD
        void Cadastrar(Colaborador colaborador);
        void Atualizar(Colaborador colaborador);
        void AtualizarSenha(Colaborador colaborador);
        void Excluir(int Id);

        Colaborador ObterColaborador(int Id);
        List<Colaborador> ObterColaboradorPorEmail(string Email);
        IEnumerable<Colaborador> ObterTodosColaboradores();

        public List<NivelAcesso> ObtemNivelDeAcesso();
    }
}
