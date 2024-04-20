using CatBuddy.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace CatBuddy.Controllers
{
    public class ProdutoController : Controller
    {
        private IProdutoRepository _produtoRepository;
        public ProdutoController(IProdutoRepository produtoRepository)
        { 
            _produtoRepository = produtoRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult InformacoesProduto(int id)
        {
            id = 1;
            var produto = _produtoRepository.retornaProduto(id);
            return View(produto);
        }
    }
}
