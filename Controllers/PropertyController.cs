using Microsoft.AspNetCore.Mvc;
using PropertyApp.Data;
using PropertyApp.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PropertyApp.Controllers
{
    public class PropertyController : Controller
    {
        private readonly PropertyContext _context;

        public PropertyController(PropertyContext context)
        {
            _context = context;
        }

        // GET: Property
        public async Task<IActionResult> Index()
        {
            var properties = await _context.Properties.ToListAsync();
            return View(properties);
        }
    }
}
