using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyApp.Data;
using PropertyApp.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PropertyContext _context;

        public HomeController(ILogger<HomeController> logger, PropertyContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string? keyword, decimal? minPrice, decimal? maxPrice, int? minBedrooms, int? maxBedrooms, int? minBathrooms, int? maxBathrooms, int page = 1, int pageSize = 5)
        {
            var propertiesQuery = _context.Properties.AsQueryable();

            // Filter by keyword
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                propertiesQuery = propertiesQuery.Where(p =>
                    p.Title.Contains(keyword) ||
                    (p.DetailDescription != null && p.DetailDescription.Contains(keyword))
                );
            }

            // Filter by price
            if (minPrice.HasValue)
            {
                propertiesQuery = propertiesQuery.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                propertiesQuery = propertiesQuery.Where(p => p.Price <= maxPrice.Value);
            }

            // Filter by number of bedrooms
            if (minBedrooms.HasValue)
            {
                propertiesQuery = propertiesQuery.Where(p => p.Bedrooms >= minBedrooms.Value);
            }
            if (maxBedrooms.HasValue)
            {
                propertiesQuery = propertiesQuery.Where(p => p.Bedrooms <= maxBedrooms.Value);
            }

            // Filter by number of bathrooms
            if (minBathrooms.HasValue)
            {
                propertiesQuery = propertiesQuery.Where(p => p.Bathrooms >= minBathrooms.Value);
            }
            if (maxBathrooms.HasValue)
            {
                propertiesQuery = propertiesQuery.Where(p => p.Bathrooms <= maxBathrooms.Value);
            }

            // Get the total number of properties
            var totalProperties = await propertiesQuery.CountAsync();

            // Calculate the total number of pages
            var totalPages = (int)Math.Ceiling((double)totalProperties / pageSize);

            // Fetch the properties for the current page
            var latestProperties = await propertiesQuery
                .OrderByDescending(p => p.Id) // Assumes higher ID means more recent
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new PropertyListViewModel
            {
                Properties = latestProperties,
                CurrentPage = page,
                TotalPages = totalPages,
                Keyword = keyword,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                MinBedrooms = minBedrooms,
                MaxBedrooms = maxBedrooms,
                MinBathrooms = minBathrooms,
                MaxBathrooms = maxBathrooms
            };

            return View(model);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
