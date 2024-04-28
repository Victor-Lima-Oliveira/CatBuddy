using CatBuddy.httpContext;
using CatBuddy.Models;
using CatBuddy.Repository.Contract;
using CatBuddy.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CatBuddy.Controllers
{
    public class CarrinhoController : Controller
    {
        private IProdutoRepository _produtoRepository;
        private IPedidoRepository _pedidoRepository;
        private CarrinhoDeCompraCookie _carrinhoDeCompraCookie;
        public CarrinhoController(IProdutoRepository produtoRepository,
            CarrinhoDeCompraCookie carrinhoDeCompraCookie,
            IPedidoRepository pedidoRepository)
        {
            _produtoRepository = produtoRepository;
            _carrinhoDeCompraCookie = carrinhoDeCompraCookie;
            _pedidoRepository = pedidoRepository;
        }
        public IActionResult Carrinho()
        {
            return View(_carrinhoDeCompraCookie.ConsultarProdutosNoCarrinho());
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
                    Subtotal = (float)Math.Round(qtdDeProduto * produto.Preco, 2),
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
            return View();
        }
        public IActionResult FinalizarPedido(int codPagamento)
        {
            // TODO: Criar uma transaction para adicionar produto e itens do produto 
            // TODO: Recuperar o id do usuario

            List<Produto> listProdutosDoCarrinho;
            Produto produtoAux;
            float valorTotal = 0;
            int codPedido;
            int qtdEstoqueAux;
            
            try
            {
                // Busca tudo que está no carrinho
                listProdutosDoCarrinho = _carrinhoDeCompraCookie.ConsultarProdutosNoCarrinho();

                // Varre todo o carrinho para descobrir o valor total da compra
                foreach (Produto produtoItem in listProdutosDoCarrinho)
                {
                    valorTotal += produtoItem.Subtotal;
                }

                // Cadastra o pedido
                codPedido = _pedidoRepository.CadastrarPedido(1, valorTotal, codPagamento);

                // Cadastra os itens do pedido 
                foreach (Produto produtoItem in listProdutosDoCarrinho)
                {
                    _pedidoRepository.CadastrarItemPedido(codPedido,
                        produtoItem.CodIdProduto,
                        produtoItem.QtdDeProduto,
                        produtoItem.Subtotal);
                }

                // Atualiza a quantidade no estoque 
                foreach(Produto produtoItemCarrinho in listProdutosDoCarrinho)
                {
                    // Retorna a quantidade do produto em estoque 
                    produtoAux = _produtoRepository.retornaProduto(produtoItemCarrinho.CodIdProduto);

                    // Verifica qual será o estoque pós venda
                    qtdEstoqueAux = produtoAux.QtdEstoque - produtoItemCarrinho.QtdDeProduto;

                    // Retira a quantidade comprada do estoque 
                    _produtoRepository.VendeProduto(produtoAux.CodIdProduto, qtdEstoqueAux);
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
