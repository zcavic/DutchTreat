using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
	public class AppController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet("contact")]
		public IActionResult Contact()
		{
			ViewBag.Title = "Contact";

			throw new InvalidOperationException("Bad things happen to good developer.");

			return View();
		}

		[HttpGet("About")]
		public IActionResult About()
		{
			ViewBag.Title = "About Us";
			return View();
		}
	}
}
