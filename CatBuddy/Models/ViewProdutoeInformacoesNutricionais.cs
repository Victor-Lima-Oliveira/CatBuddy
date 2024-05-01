namespace CatBuddy.Models
{
    public class ViewProdutoeInformacoesNutricionais
    {
        public Produto produto {  get; set; }
        public InfoNutricionais infoNutricionais { get; set; }
        public bool PossuiInformacoesNutricionais { get; set; }
    }
}
