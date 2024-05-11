using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace CatBuddy.Models
{
    public class Produto
    {
        [Required]
        public int CodIdProduto { get; set; }


        [MaxLength(40), Display(Name ="Nome do produto*"), Required(ErrorMessage = "O Nome do Produto é obrigatório")]
        public string NomeProduto { get; set; }


        [Display(Name = "Quantidade em Estoque*"), Required(ErrorMessage = "A quantidade no Estoque é obrigatório"),
            Range(1, int.MaxValue, ErrorMessage = "O campo Quantidade deve ser um número inteiro positivo.")]
        public int QtdEstoque { get; set; } = 0;


        [Display(Name = "Preço*"), Required(ErrorMessage = "O Preço é obrigatório")]
           [ Range(0.01, double.MaxValue, ErrorMessage = "O campo Preço deve ser um número positivo.")]
        public float Preco { get; set; } = 0;


        [MaxLength(30)]
        public string? Idade { get; set; }


        [MaxLength(45)]
        public string? Sabor { get; set; }


        [MaxLength(45)]
        public string? Cor { get; set; }


        [MaxLength(45), Display(Name = "Medidas aproximadas")]
        public string? MedidasAproximadas { get; set; }


        [MaxLength(45)]
        public string? Material { get; set; }

        [Display(Name = "Composição")]
        public string? Composicao { get; set; }


        [MaxLength(200), Display(Name = "Foto*")]
        public string ImgPath { get; set; }

        [MaxLength(200), Display(Name = "Foto Informações nutricionais")]
        public string imgPathinfoNutricionais { get; set; }


        public int QtdDeProduto { get; set; }


        public float Subtotal { get; set; }


        [Required]
        public int CodFornecedor { get; set; }


        [Display(Name = "Marca*")]
        public string? NomeFornecedor { get; set; }


        [Required]
        public int CodCategoria { get; set; }


        [Display(Name = "Categoria*")]
        public string? NomeCategoria { get; set; }


        [Display(Name = "Descrição*"), Required(ErrorMessage = "A Descrição do produto é obrigatória")]
        public string Descricao { get; set; }

    }
}
