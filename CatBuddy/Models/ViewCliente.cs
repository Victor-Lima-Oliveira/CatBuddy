using System.ComponentModel.DataAnnotations;

namespace CatBuddy.Models
{
    public class ViewCliente
    {
        public Cliente cliente {  get; set; }

        [Display(Name ="Gênero")]
        public List<Genero> ListGenero { get; set; }
    }
}
