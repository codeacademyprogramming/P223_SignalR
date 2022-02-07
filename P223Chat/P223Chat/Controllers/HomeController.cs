using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using P223Chat.Hubs;
using P223Chat.Models;
using P223Chat.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace P223Chat.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHubContext<ChatHub> _hubContext;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHubContext<ChatHub> hubContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hubContext = hubContext;
        }

        public IActionResult CreateUser()
        {
            AppUser user1 = new AppUser { FullName = "Vusal Bagirov", UserName = "Vusal" };
            AppUser user2 = new AppUser { FullName = "Hesen Nuruzade", UserName = "Hasan" };
            AppUser user3 = new AppUser { FullName = "Lale Quliyeva", UserName = "Lala" };
            AppUser user4 = new AppUser { FullName = "Ehed Tagiyev", UserName = "Ehed" };

            var result1 = _userManager.CreateAsync(user1,"User@123").Result;
            var result2 = _userManager.CreateAsync(user2, "User@123").Result;
            var result3 = _userManager.CreateAsync(user3, "User@123").Result;
            var result4 = _userManager.CreateAsync(user4, "User@123").Result;


            return Content("Created");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await  _userManager.FindByNameAsync(loginModel.UserName);

            if(user == null)
            {
                ModelState.AddModelError("", "UserName or Password is incorrect!");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password,true,false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UserName or Password is incorrect!");
                return View();
            }


            return RedirectToAction("Chat");
        }

        public IActionResult Index()
        {
            return View();
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

        public IActionResult Chat()
        {
            List<AppUser> model = _userManager.Users.ToList();
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> ShowToaster(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);

            if (appUser == null) return NotFound();

            await _hubContext.Clients.Client(appUser.ConnectionId).SendAsync("ShowToaster");

            return RedirectToAction("Chat");
        }
    }
}
