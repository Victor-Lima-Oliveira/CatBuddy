using System.ComponentModel.DataAnnotations;

namespace CatBuddy.Models
{
    public class Colaborador
    {

        [Display(Name = "Código do Cliente")]
        public int cod_id_colaborador { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O nome é obrigatório!")]
        [MaxLength(50, ErrorMessage = "Digite um  Nome com no máximo 50 caracteres")]
        public string nomeColaborador { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório!")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Digite um CPF válido")]
        public string CPF { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "O Email é obrigatório")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Celular")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Digite um Telefone válido!")]
        public string Telefone { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "A Senha é obrigatória!")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Display(Name = "Confirme a senha")]
        [Required(ErrorMessage = "Confirme a sua Senha!")]
        [Compare("Senha", ErrorMessage = "A Senhas são diferentes")]
        public string confirmaSenha { get; set; }

        [Required(ErrorMessage = "Selecione um Gênero")]
        public int codGenero { get; set; }

        [Display(Name = "Nível de Acesso")]
        [Required(ErrorMessage = "O Nível de Acesso é obrigatório!")]
        public  int NivelDeAcesso { get; set; }

        public  bool  IsColaboradorAtivo { get; set; }
    }
}
