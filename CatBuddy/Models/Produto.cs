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
        [Required]
        public int CodCategoria { get; set; }
        public string Descricao { get; set; }
        public int QtdEstoque { get; set; }
        [
            Display(Name = "Marca"),
            Required
        ]
        public int CodFornecedor { get; set; }
        [MaxLength(13)]
        public string Idade { get; set; }
        [MaxLength(45)]
        public string Sabor { get; set; }
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
        public string DsNome { get; set; }
    }
}
