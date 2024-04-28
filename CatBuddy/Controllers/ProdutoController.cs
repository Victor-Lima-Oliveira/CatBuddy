using CatBuddy.httpContext;
using CatBuddy.Models;
using CatBuddy.Repository.Contract;
using CatBuddy.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CatBuddy.Controllers
{
    public class ProdutoController : Controller
    {
        private IProdutoRepository _produtoRepository;
        private CarrinhoDeCompraCookie _carrinhoDeCompraCookie;
        public ProdutoController(IProdutoRepository produtoRepository, CarrinhoDeCompraCookie carrinhoDeCompraCookie)
        {
            _produtoRepository = produtoRepository;
            _carrinhoDeCompraCookie = carrinhoDeCompraCookie;
        }

        // Action para produtos não encontrados
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult InformacoesProduto(int id)
        {
            Produto produtoSelecionado;
            try
            {
                if (id != 0)
                {
                    produtoSelecionado = _produtoRepository.retornaProduto(id);

                    if (produtoSelecionado.CodIdProduto == 0)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(produtoSelecionado);
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception err)
            {
                TempData[Const.ErroTempData] = err.Message;
                return RedirectToAction(Const.ErroAction, Const.ErroController);
            }

        }

        public IActionResult AdicionarItemAoCarrinho(Produto produtoSelecionado)
        {
            try
            {

                // Busca os dados do produto
                Produto produto = _produtoRepository.retornaProduto(produtoSelecionado.CodIdProduto);

                // Verifica se o produto existe
                if (produto == null)
                {
                    return View("Index");
                }
                else
                {
                    // Salva nos cookies apenas os dados que serão apresentados no carrinho
                    Produto produtoNoCarrinho = new Produto()
                    {
                        CodIdProduto = produto.CodIdProduto,
                        ImgPath = produto.ImgPath,
                        QtdDeProduto = produtoSelecionado.QtdDeProduto,
                        NomeProduto = produto.NomeProduto,
                        Preco = produto.Preco,
                        Subtotal = (float)Math.Round(produtoSelecionado.QtdDeProduto * produto.Preco, 2),
                        NomeFornecedor = produto.NomeFornecedor
                    };

                    // Salva os dados nos cookies
                    _carrinhoDeCompraCookie.AdicionarAoCarrinho(produtoNoCarrinho);

                    // Depois redireciona para o carrinho 
                    return RedirectToAction("Carrinho", "Carrinho");
                }
            }
            catch (Exception err)
            {
                TempData[Const.ErroTempData] = err.Message;
                return RedirectToAction(Const.ErroAction, Const.ErroController);
            }
        }

    }
}
