using System.ComponentModel.DataAnnotations;

namespace CatBuddy.Models
{
    public class ViewColaborador
    {
        public Colaborador Colaborador { get; set; }

        [Display(Name = "Gênero")]
        public List<Genero> ListGenero { get; set; }

        [Display(Name = "Nível de Acesso")]
        public List<NivelAcesso> ListNivelAcesso { get; set; }
    }
}
