using Microsoft.AspNetCore.Mvc;

namespace AfterSalesMessageApi.Controllers
{
    public class AuthController : ControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
