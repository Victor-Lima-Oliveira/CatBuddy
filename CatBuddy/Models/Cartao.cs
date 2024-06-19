using System.ComponentModel.DataAnnotations;

namespace CatBuddy.Models
{
    public class Cartao
    {

        public int? cod_id_pagamento { get; set; }

        public int cod_cliente { get; set; }

        [Display(Name = "Titular")]
        [Required(ErrorMessage = "Digite o titular do cartão")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Digite um nome entre 6 e 50 caracteres")]
        public string nomeTitular { get; set; }

        [Display(Name = "Número do cartão")]
        [Required(ErrorMessage = "Digite o número do cartão")]
        [StringLength(19, MinimumLength = 18, ErrorMessage = "Número de cartão inválido!")]
        public string numeroCartaoCred { get; set; }

        [Display(Name = "Data de validade")]
        [Required(ErrorMessage = "Digite a data de validade do cartão")]
        [StringLength(5, MinimumLength = 4, ErrorMessage = "Digite uma data válida")]
        public string dataDeValidade { get; set; }

        [Display(Name = "Código de segurança")]
        [Required(ErrorMessage = "Digite o código de segurança!")]
        [StringLength(3, MinimumLength = 2, ErrorMessage = "Digite um código válido!")]
        public string codSeguranca { get; set; }
    }
}
