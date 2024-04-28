using CatBuddy.Models;
using Newtonsoft.Json;

namespace CatBuddy.httpContext
{
    public class CarrinhoDeCompraCookie
    {
        private string sKey = "Carrinho.Compra";
        private Cookie _cookie;

        public CarrinhoDeCompraCookie(Cookie cookie)
        {
            _cookie = cookie;
        }

        /// <summary>
        /// Persiste os produtos no carrinho
        /// </summary>
        private void SalvarProdutoNosCookies(List<Produto> listProdutos)
        {
            // Converte a lista de objetos que o usuário selecionou em string
            string listProdutosString = JsonConvert.SerializeObject(listProdutos);

            // Cadastra esse cookie no navegador do usuário
            _cookie.Atualizar(sKey, listProdutosString);
        }

        /// <summary>
        /// Retorna a lista de produtos no carrinho
        /// </summary>
        public List<Produto> ConsultarProdutosNoCarrinho()
        {
            if (_cookie.Existe(sKey))
            {
                // Recupera os produtos cadastrados no cookie (que estão em string)
                string sProdutosAdicionados = _cookie.Consultar(sKey);

                // converte os mesmo para o objeto produto
                return JsonConvert.DeserializeObject<List<Produto>>(sProdutosAdicionados);
            }
            else
            {
                // Se não possuir nada cadastrado, retorna uma lista vazia
                return new List<Produto>();
            }
        }

        /// <summary>
        /// Insere ou atualiza a quantidade de um produto no carrinho
        /// </summary>
        public void AdicionarAoCarrinho(Produto produtoQueSeraPersistido)
        {
            List<Produto> listProduto;

            // Se existe o cookie
            if (_cookie.Existe(sKey))
            {
                // Busca a lista de produtos já cadastrados 
                listProduto = ConsultarProdutosNoCarrinho();

                // Se o produto já estiver no carrinho, resgata ele dos cookies
                Produto produtoJaCadastrado = listProduto.SingleOrDefault(a => a.CodIdProduto == produtoQueSeraPersistido.CodIdProduto);

                // Se o produto não estiver cadastrado, insere na lista
                if (produtoJaCadastrado == null)
                {
                    listProduto.Add(produtoQueSeraPersistido);
                }
                // Se o produto ja foi cadastrado, atualiza a quantidade com o novo valor 
                else
                {
                    // Remove o produto antigo do cookies
                    RemoverProduto(produtoJaCadastrado);

                    // Remove o produto antigo da lista
                    listProduto.Remove(produtoJaCadastrado);

                    // Adiciona o produto com um novo subtotal e quantidade
                    Produto produtoAtualizado = new Produto()
                    {
                        CodIdProduto = produtoJaCadastrado.CodIdProduto,
                        ImgPath = produtoJaCadastrado.ImgPath,
                        QtdDeProduto = produtoQueSeraPersistido.QtdDeProduto,
                        NomeProduto = produtoJaCadastrado.NomeProduto,
                        Preco = produtoJaCadastrado.Preco,
                        Subtotal = produtoQueSeraPersistido.Subtotal,
                        NomeFornecedor = produtoJaCadastrado.NomeFornecedor 
                    };

                    // Adiciona o novo produto na lista 
                    listProduto.Add(produtoAtualizado);
                }
            }
            else
            {
                listProduto = new List<Produto>();
                listProduto.Add(produtoQueSeraPersistido);
            }

            // Registra os produtos nos cookies
            SalvarProdutoNosCookies(listProduto);
        }

        /// <summary>
        /// Remove um produto do carrinho
        /// </summary>
        public void RemoverProduto(Produto produto)
        {
            // Retorna a lista com todos os produtos 
            List<Produto> listProduto = ConsultarProdutosNoCarrinho();

            // Verifica se o produto está nos cookies 
            Produto produtoJaCadastrado = listProduto.SingleOrDefault(a => a.CodIdProduto == produto.CodIdProduto);

            // Se o produto já estiver cadastrados nos cookies
            if(produtoJaCadastrado != null)
            {
                // Remove o produto da lista
                listProduto.Remove(produtoJaCadastrado);

                // Salva a lista sem ele o produto nos cookies 
                SalvarProdutoNosCookies(listProduto);
            }
        }

        /// <summary>
        /// Remove todos os produtos e o cookie 
        /// </summary>
        public void RemoverTodos()
        {
            _cookie.Remover(sKey);
        }

    }
}
