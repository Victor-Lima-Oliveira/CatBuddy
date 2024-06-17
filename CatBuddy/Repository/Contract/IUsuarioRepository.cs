using CatBuddy.Models;

namespace CatBuddy.Repository.Contract
{
    public interface IUsuarioRepository
    {
        public List<Genero> RetornaGenero();

        public List<Logradouro> RetornaLogradouro();
    }
}
