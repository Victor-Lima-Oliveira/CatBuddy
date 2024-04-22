using CatBuddy.httpContext;
using CatBuddy.Models;
using CatBuddy.Repository.Contract;
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
            if (id != 0)
            {
                return View(_produtoRepository.retornaProduto(id));
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult AdicionarItemAoCarrinho(int id)
        {
            // Busca os dados do produto
            Produto produto = _produtoRepository.retornaProduto(id);
            
            // Verifica se o produto existe
            if(produto == null)
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
                    QtdDeProduto = produto.QtdDeProduto,
                    DsNome = produto.DsNome,
                    Preco = produto.Preco
                };

                // Salva os dados nos cookies
                _carrinhoDeCompraCookie.AdicionarAoCarrinho(produtoNoCarrinho);

                // Depois redireciona para o carrinho 
                return RedirectToAction("Carrinho");
            }
        }

        public IActionResult Carrinho()
        {
            return View(_carrinhoDeCompraCookie.ConsultarProdutosNoCarrinho());
        }


    }
}
