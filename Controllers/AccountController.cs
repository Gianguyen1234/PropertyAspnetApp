using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PropertyApp.Models;
using System.Threading.Tasks;

namespace PropertyApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager; // Add this line

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager) // Inject RoleManager
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager; // Initialize the role manager
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // Find the user by email
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        // Check if the user is in the Admin role
                        bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                        if (isAdmin)
                        {
                            return RedirectToAction("Dashboard", "Admin"); // Redirect to Admin Dashboard
                        }
                        return RedirectToAction("Index", "Home"); // Redirect to Home
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        

        [HttpPost]
        public async Task<IActionResult> CreateAdmin(string email, string password)
        {
            // Ensure the Admin role exists
            var roleExists = await _roleManager.RoleExistsAsync("Admin");
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Assign the "Admin" role to the new user
                await _userManager.AddToRoleAsync(user, "Admin");
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(); // Return to the view if there were errors
        }
    }
}
