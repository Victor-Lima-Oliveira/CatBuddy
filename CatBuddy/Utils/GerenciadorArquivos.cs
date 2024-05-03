namespace CatBuddy.Utils
{
    public class GerenciadorArquivos
    {
        /// <summary>
        /// Salva a imagem no servidor e retorna o caminho da imagem
        /// </summary>
        public static string CadastrarImagemProduto(IFormFile file)
        {
            // Nome do caminho da imagem + horario para evitar conflito de arquivos com mesmo nome 
            string nomeArquivo = Path.GetFileName(file.FileName) + DateTime.Now.ToString();

            // Caminho para salvar a imagem de produtos
            string Caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/produtos", nomeArquivo);

            // Faz a copia do arquivo para o servidor
            using (FileStream stream = new FileStream(Caminho, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Retorna a string para cadastrar no banco
            return Path.Combine("/Imagem", nomeArquivo).Replace("\\", "/");
        }

    }

}
