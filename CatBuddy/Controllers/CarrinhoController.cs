using CatBuddy.httpContext;
using CatBuddy.Models;
using CatBuddy.Repository.Contract;
using CatBuddy.Utils;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Transactions;

namespace CatBuddy.Controllers
{
    public class CarrinhoController : Controller
    {
        private IProdutoRepository _produtoRepository;
        private IPedidoRepository _pedidoRepository;
        private IHttpContextAccessor _httpContextAccessor;
        private CarrinhoDeCompraCookie _carrinhoDeCompraCookie;

        public CarrinhoController(IProdutoRepository produtoRepository,
            CarrinhoDeCompraCookie carrinhoDeCompraCookie,
            IPedidoRepository pedidoRepository, IHttpContextAccessor httpContextAccessor)
        {
            _produtoRepository = produtoRepository;
            _carrinhoDeCompraCookie = carrinhoDeCompraCookie;
            _pedidoRepository = pedidoRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Carrinho()
        {
            List<Produto> listproduto = _carrinhoDeCompraCookie.ConsultarProdutosNoCarrinho();
            return View(listproduto);
        }

        public IActionResult AlterarQuantidadeNosCookies(int codIdProduto, int qtdDeProduto)
        {
            try
            {
                Produto produto = _produtoRepository.retornaProduto(codIdProduto);

                Produto produtoNoCarrinho = new Produto()
                {
                    CodIdProduto = produto.CodIdProduto,
                    ImgPath = produto.ImgPath,
                    QtdDeProduto = qtdDeProduto,
                    NomeProduto = produto.NomeProduto,
                    Preco = produto.Preco,
                    Subtotal = (float)Math.Round(qtdDeProduto * Convert.ToDouble(produto.Preco), 2),
                };

                _carrinhoDeCompraCookie.AdicionarAoCarrinho(produtoNoCarrinho);
            }
            catch (Exception err)
            {
                TempData[Const.ErroTempData] = err.Message;
                return RedirectToAction(Const.ErroAction, Const.ErroController);
            }

            return RedirectToAction("Carrinho");
        }

        public IActionResult ExcluirCarrinho()
        {
            _carrinhoDeCompraCookie.RemoverTodos();
            return RedirectToAction("Carrinho");
        }

        public IActionResult RemoverProduto(int id)
        {
            Produto produto;
            try
            {
                produto = _produtoRepository.retornaProduto(id);

                _carrinhoDeCompraCookie.RemoverProduto(produto);
            }
            catch (Exception err)
            {
                TempData[Const.ErroTempData] = err.Message;
                return RedirectToAction(Const.ErroAction, Const.ErroController);
            }
            return RedirectToAction("Carrinho");
        }

        public IActionResult Pagamento()
        {
            ViewCheckout view = new ViewCheckout()
            {
                ListProduto = _carrinhoDeCompraCookie.ConsultarProdutosNoCarrinho(),
                Endereco = MainLayout.EnderecoSelecionado,
            };

            return View(view);
        }

        public IActionResult FinalizarPedido(int codPagamento)
        {
            List<Produto> listProdutosDoCarrinho;
            Produto produtoAux;
            Cliente cliente;
            float valorTotal = 0;
            int codPedido;
            int qtdEstoqueAux;


            try
            {
                // Recupera os dados do cliente da sessão
                cliente = JsonConvert.DeserializeObject<Cliente>(_httpContextAccessor.HttpContext.Session.GetString("Login.Cliente"));

                // Inicia uma transação que garante a consistencia de dados
                using (var scope = new TransactionScope())
                {
                    // Busca tudo que está no carrinho
                    listProdutosDoCarrinho = _carrinhoDeCompraCookie.ConsultarProdutosNoCarrinho();

                    // Varre todo o carrinho para descobrir o valor total da compra
                    foreach (Produto produtoItem in listProdutosDoCarrinho)
                    {
                        valorTotal += produtoItem.Subtotal;
                    }

                    // Cadastra o pedido
                    codPedido = _pedidoRepository.CadastrarPedido(cliente.cod_id_cliente.Value, valorTotal, codPagamento);

                    // Cadastra os itens do pedido 
                    foreach (Produto produtoItem in listProdutosDoCarrinho)
                    {
                        _pedidoRepository.CadastrarItemPedido(codPedido,
                            produtoItem.CodIdProduto,
                            cliente.cod_id_cliente.Value,
                            produtoItem.QtdDeProduto,
                            produtoItem.Subtotal);
                    }

                    // Atualiza a quantidade no estoque 
                    foreach (Produto produtoItemCarrinho in listProdutosDoCarrinho)
                    {
                        // Retorna a quantidade do produto em estoque 
                        produtoAux = _produtoRepository.retornaProduto(produtoItemCarrinho.CodIdProduto);

                        // Verifica qual será o estoque pós venda
                        qtdEstoqueAux = Convert.ToInt32(produtoAux.QtdEstoque - produtoItemCarrinho.QtdDeProduto);

                        // Retira a quantidade comprada do estoque 
                        _produtoRepository.VendeProduto(produtoAux.CodIdProduto, qtdEstoqueAux);
                    }

                    // Finaliza a transação
                    scope.Complete();
                }

                // Limpa os cookies 
                _carrinhoDeCompraCookie.RemoverTodos();

                return View();
            }
            catch (Exception err)
            {
                TempData[Const.ErroTempData] = err.Message;
                return RedirectToAction(Const.ErroAction, Const.ErroController);
            }

        }
    }
}
