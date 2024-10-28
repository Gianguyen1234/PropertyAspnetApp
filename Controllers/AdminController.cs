using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PropertyApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
          public IActionResult Dashboard()
        {
            return View();
        }
    }
}
