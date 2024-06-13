using Microsoft.AspNetCore.Http;

namespace CatBuddy.Utils
{
    public static class Const
    {
        public const string ErroController  = "Erro";
        public const string ErroAction  = "MostrarErro";
        public const string ErroTempData  = "ERRO";
        public const int ErroProdutoNaoEncontrado  = 1;

        public const string AvisoPaginaPrincipalSucesso = "ViewBagPaginaPrincipalSucesso";
        public const string AvisoPaginaVizualizarProdutos = "ViewBagPaginaVizualizarSucesso";

        // Categorias 
        public const int RacaoSeca = 1;
        public const int RacaoUmida = 2;
        public const int Petisco = 3;
        public const int ComedourosEFontes = 4;
        public const int ColeirasEPeitorais = 5;
        public const int Esconderijos = 6;
        public const int Transporte = 7;
        public const int Higiene = 8;
        public const int RoupasGatos = 9;
        public const int MimosHumanos = 10;

        // PRODUTOS
        public const int SEM_FORNECEDOR_SELECIONADO = 0;
        public const int SEM_CATEGORIA_SELECIONADA = 0;

        // Nivel de acesso 
        public const int GERENTE = 3;
        public const int RH = 4;

        // Validações 
        public const int SEM_NIVEL_ACESSO = 0;
        public const int SEM_GENERO = 0;

    }
}
