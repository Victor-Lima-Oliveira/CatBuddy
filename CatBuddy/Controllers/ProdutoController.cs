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
    }
}
