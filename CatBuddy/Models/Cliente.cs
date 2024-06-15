using System.ComponentModel.DataAnnotations;

namespace CatBuddy.Models
{
    public class Cliente
    {
        [Display(Name = "Código do Cliente")]
        public int? cod_id_cliente { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O nome é obrigatório!")]
        [MaxLength(50, ErrorMessage = "Digite um  Nome com no máximo 50 caracteres")]
        public string nomeUsuario { get; set; }

        [Display(Name = "Data de nascimento")]
        [Required(ErrorMessage = "A Data de Nascimento é obrigatória!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtNascimento { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório!")]
        [StringLength(14, MinimumLength = 13, ErrorMessage = "Digite um CPF válido")]
        public string CPF { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "O Email é obrigatório")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Celular")]
        [StringLength(15, MinimumLength = 14, ErrorMessage = "Digite um Telefone válido!")]    
        public  string? Telefone { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "A Senha é obrigatória!")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Display(Name = "Confirme a senha")]
        [Required(ErrorMessage = "Confirme a sua Senha!")]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "A Senhas são diferentes")]
        public string confirmaSenha { get; set; }

        [Required(ErrorMessage = "Selecione um Gênero")]
        public int codGenero { get; set; }

        public bool? IsClienteAtivo { get; set; }
    }
}
