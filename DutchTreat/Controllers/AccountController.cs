using Microsoft.AspNetCore.Mvc;
using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
	public class AccountController : Controller
	{
		private readonly ILogger<AccountController> logging;

		public AccountController(ILogger<AccountController> logging)
		{
			this.logging = logging;
		}

		public IActionResult Login()
		{
			if (this.User.Identity.IsAuthenticated)
				return RedirectToAction("Index", "App");

			return View();

		}

	}
}
