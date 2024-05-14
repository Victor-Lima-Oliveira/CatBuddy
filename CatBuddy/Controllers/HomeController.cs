using CatBuddy.httpContext;
using CatBuddy.Models;
using CatBuddy.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CatBuddy.Controllers
{
    public class HomeController : Controller
    {
        private IProdutoRepository _produtoRepository;
        protected List<Categoria> listCategoria = new List<Categoria>();

        public HomeController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public IActionResult Index()
        { 
            return View(_produtoRepository.RetornaCategorias());
        }

    }
}

