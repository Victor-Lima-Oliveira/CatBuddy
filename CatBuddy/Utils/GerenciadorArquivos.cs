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

            string nomeArquivo = DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetFileName(file.FileName);

            // Caminho para salvar a imagem de produtos
            string Caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/produtos", nomeArquivo);

            // Faz a copia do arquivo para o servidor
            using (FileStream stream = new FileStream(Caminho, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Retorna a string para cadastrar no banco
            return Path.Combine("img/produtos/", nomeArquivo).Replace("\\", " /");
        }

        public static void DeletarImagemProduto(string imgPathProduto)
        {
            // Pega o caminho que está salvo no servidor e o caminho salvo no banco
            string CaminhoCompleto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imgPathProduto).Replace("/", "\\");

            // Se existir essa imagem no banco deleta ela 
            if (File.Exists(CaminhoCompleto))
            {
                File.Delete(CaminhoCompleto);
            }
        }

    }

}
