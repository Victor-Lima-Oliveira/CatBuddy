using CatBuddy.Models;

namespace CatBuddy.Utils
{
    public static class MainLayout
    {
        private static bool showDialog = false;
        public static string TituloDialog = String.Empty;
        public static string ConteudoDialog = String.Empty;
        private static object? _parametro;

        public static bool showSnackbar = false;
        public static string MensagemSnackbar = String.Empty;
        public static int codCliente = 0;

        public static int qtdCarrinho = 0;

        public static Endereco EnderecoSelecionado = null;


        public static void OpenDialog(string Titulo, string conteudo, object parametro = null)
        {
            TituloDialog = Titulo;
            ConteudoDialog = conteudo;
            if(parametro != null)
            {
                _parametro = parametro;
            }
            showDialog = true;
        }

        public static void CloseDialog()
        {
            showDialog = false;
            TituloDialog = String.Empty ;
            ConteudoDialog = String.Empty ;
            _parametro = null ;
        }

        public static object ObterParametro()
        {
            return _parametro;
        }

        public static bool IsDialogOpen()
        {
            return showDialog;
        }

        public static bool IsSnackBarOpen()
        {
            return showSnackbar;
        }

        public static async void OpenSnackbar(string mensagem)
        {
            showSnackbar = true;
            MensagemSnackbar = mensagem;
        }

        public static void CloseSnackbar()
        {
            showSnackbar = false;
        }
    }
}
