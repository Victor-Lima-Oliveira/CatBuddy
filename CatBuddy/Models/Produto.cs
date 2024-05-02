using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace CatBuddy.Models
{
    public class Produto
    {
        [Required]
        public int CodIdProduto { get; set; }


        [MaxLength(40), Display(Name ="Nome do produto")]
        public string NomeProduto { get; set; }


        [Display(Name = "Quantidade em Estoque"), Required]
        public int QtdEstoque { get; set; }


        [Required, Display(Name = "Preço")]
        public float Preco { get; set; }


        [MaxLength(30)]
        public string Idade { get; set; }


        [MaxLength(45)]
        public string Sabor { get; set; }


        [MaxLength(45)]
        public string Cor { get; set; }


        [MaxLength(45), Display(Name = "Medidas aproximadas")]
        public string MedidasAproximadas { get; set; }


        [MaxLength(45)]
        public string Material { get; set; }

        [Display(Name = "Composição")]
        public string Composicao { get; set; }


        [MaxLength(200)]
        public string ImgPath { get; set; }


        public int QtdDeProduto { get; set; }


        public float Subtotal { get; set; }


        [Required]
        public int CodFornecedor { get; set; }


        [Display(Name = "Marca"),]
        public string NomeFornecedor { get; set; }


        [Required]
        public int CodCategoria { get; set; }


        [Display(Name = "Categoria")]
        public string NomeCategoria { get; set; }


        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

    }
}
