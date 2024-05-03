using System.ComponentModel.DataAnnotations;

namespace CatBuddy.Models
{
    public class InfoNutricionais
    {
        public int? cod_id_info {get; set;}
        public int? cod_produto { get; set; }

        [Display(Name = "Tamanho ou porção")]
        public string? TamanhoOuPorcao { get; set; }

        [Display(Name = "Caloria por porção")]
        public string? caloriaPorPorcao { get; set; }
        
        [Display(Name = "Proteinas")]
        public string? proteinas { get; set; }

        [Display(Name = "Carboidratos")]
        public string? carboidratos { get; set; }

        [Display(Name = "Vitaminas")]
        public string? vitaminas { get; set; }

        [Display(Name = "Mineirais")]
        public string? mineirais { get; set; }

        [Display(Name = "Fibra Diétrica")]
        public string? fibraDietrica { get; set; }

        [Display(Name = "Colesterol")]
        public string? Colesterol { get; set; }

        [Display(Name = "Sódio")]
        public string? Sodio { get; set; }
    }
}
