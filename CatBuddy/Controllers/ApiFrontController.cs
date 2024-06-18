using CatBuddy.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CatBuddy.Controllers
{
    [Route("api/front")]
    [ApiController]
    public class ApiFrontController : Controller
    {
        [HttpGet("CloseDialog")]
        public IActionResult CloseDialog()
        {
            MainLayout.CloseDialog();
            return Ok();
        }
    }
}
