using CatBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CatBuddy.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}

