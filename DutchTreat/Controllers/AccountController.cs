using Microsoft.AspNetCore.Mvc;
using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using DutchTreat.Data.Entities;
using System.Threading.Tasks;
using System.Linq;

namespace DutchTreat.Controllers
{
	public class AccountController : Controller
	{
		private readonly ILogger<AccountController> logging;
		private readonly SignInManager<StoreUser> signInManager;

		public AccountController(ILogger<AccountController> logging, SignInManager<StoreUser> signInManager)
		{
			this.logging = logging;
			this.signInManager = signInManager;
		}

		public IActionResult Login()
		{
			if (this.User.Identity.IsAuthenticated)
				return RedirectToAction("Index", "App");

			return View();

		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

				if (result.Succeeded)
				{
					if (Request.Query.Keys.Contains("ReturnUrl"))
						return Redirect(Request.Query["ReturnUrl"].First());
					else
						return RedirectToAction("Shop", "App");
				}
			}
			ModelState.AddModelError("", "Failed to login");
			return View();
		}
	
		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "App");
		}
	}
}
