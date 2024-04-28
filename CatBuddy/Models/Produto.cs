using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace CatBuddy.Models
{
    public class Produto
    {
        [Required]
        public int CodIdProduto { get; set; }

        public string Descricao { get; set; }
        public int QtdEstoque { get; set; }

        [MaxLength(13)]
        public string Idade { get; set; }
        [MaxLength(45)]
        public string Sabor { get; set; }
        [
            Display(Name = "Informções Nutricionais")
        ]
        public string InformacoesNutricionais { get; set; }
        [MaxLength(45)]
        public string Cor { get; set; }
        [MaxLength(45)]
        public string MedidasAproximadas { get; set; }
        [MaxLength(45)]
        public string Material { get; set; }
        public string Composição { get; set; }
        [Required]
        public float Preco { get; set; }
        [MaxLength(200)]
        public string ImgPath { get; set; }
        [MaxLength(40)]
        public string NomeProduto { get; set; }
        public int QtdDeProduto { get; set; }
        public float Subtotal { get; set; }
        [
         Required
        ]
        public int CodFornecedor { get; set; }
        [
         Display(Name = "Marca"),
        ]
        public string NomeFornecedor { get; set; }
        [Required]
        public int CodCategoria { get; set; }
        public string NomeCategoria { get; set; }
    }
}
