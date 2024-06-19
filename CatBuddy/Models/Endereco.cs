using System.ComponentModel.DataAnnotations;

namespace CatBuddy.Models
{
    public class Endereco
    {
        public int? cod_id_endereco { get; set; }

        [Display(Name = "Endereço")]
        [Required(ErrorMessage = "Digite o endereço!")]
        [StringLength(35, MinimumLength = 3, ErrorMessage = "Digite um endereço entre 3 e 35 caracteres!")]
        public string enderecoUsuario { get; set; }

        [Display(Name = "Bairro")]
        [Required(ErrorMessage = "Digite o bairro!")]
        [StringLength(35, MinimumLength = 3, ErrorMessage = "Digite um bairro entre 3 e 35 caracteres!")]
        public string bairroUsuario { get; set; }

        [Display(Name = "CEP")]
        [Required(ErrorMessage = "Digite um CEP!")]
        [StringLength(9, MinimumLength = 8, ErrorMessage = "CEP inválido")]
        public string cepUsuario { get; set; }

        [Display(Name = "Logradouro")]
        [Required(ErrorMessage = "Selecione uma opção")]
        public int cod_logradouro { get; set; }

        [Display(Name = "Apelido")]
        [Required(ErrorMessage = "Digite um apelido")]
        [StringLength(35, MinimumLength = 2, ErrorMessage = "Digite um apelido entre 2 e 35 caracteres")]
        public string nomeEnderecoUsuario { get; set; }

        [Required]
        public int cod_cliente { get; set; }
    }
}
