using CatBuddy.httpContext;
using CatBuddy.LibrariesSessao.Login;
using CatBuddy.Models;
using CatBuddy.Repository.Contract;
using CatBuddy.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CatBuddy.Controllers
{
    public class HomeController : Controller
    {
        private IProdutoRepository _produtoRepository;

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

