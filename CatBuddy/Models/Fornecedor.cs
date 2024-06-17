using System.ComponentModel.DataAnnotations;

namespace CatBuddy.Models
{
    public class Fornecedor
    {
        public int? codFornecedor { get; set; }

        [Required(ErrorMessage = "Digite um nome de um fornecedor!")]
        [Display(Name = "Fornecedor")]
        [MaxLength(35, ErrorMessage = "Digite um nome com no máximo 35 caracteres!")]
        public string nomeFornecedor { get; set; }

        [Required(ErrorMessage = "Digite um CNPJ!")]
        [StringLength(18, MinimumLength = 17, ErrorMessage = "Digite um CNPJ válido!")]
        [Display(Name = "CNPJ")]
        public string cnpj {  get; set; }

        [Display(Name = "Celular")]
        [StringLength(15, MinimumLength = 14, ErrorMessage = "Digite um Telefone válido!")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "Digite um endereço")]
        [Display(Name = "Endereço")]
        [MaxLength(100, ErrorMessage = "Digite um endereço com no máximo 100 caracteres!")]
        public string endereco { get; set; }

        [Required(ErrorMessage = "Digite um bairro!")]
        [Display(Name = "Bairro")]
        [MaxLength(35, ErrorMessage = "Digite um bairro com no máximo 35 caracteres!")]
        public string bairro { get; set; }

        [Required(ErrorMessage = "Digite um CEP!")]
        [Display(Name = "CEP")]
        [StringLength(9, MinimumLength = 8, ErrorMessage = "Digite um CEP válido!")]
        public string cep { get; set; }

        [Required(ErrorMessage = "Selecione um logradouro!")]
        public int CodLogradouro { get; set; }

        [Required(ErrorMessage = "Digite um município!")]
        [Display(Name = "Município")]
        [MaxLength(30, ErrorMessage = "Digite um município com no máximo 30 caracteres")]
        public string municipio { get; set; }

        public bool? IsFornecedorAtivo { get; set; }
    }
}
