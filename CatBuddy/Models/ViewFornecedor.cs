using System.ComponentModel.DataAnnotations;

namespace CatBuddy.Models
{
    public class ViewFornecedor
    {
        public Fornecedor Fornecedor { get; set; }

        [Display(Name = "Logradouro")]
        public List<Logradouro> ListLogradouro { get; set; }
        public string? nomeLogradouro { get; set; }

        public string getEnderecoCompleto()
        {
            return $"{nomeLogradouro} {Fornecedor.endereco}";
        }
    }
}
