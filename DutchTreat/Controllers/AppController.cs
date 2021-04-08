using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
	public class AppController : Controller
	{
		private readonly IMailService _mailSerivce;

		public AppController(IMailService mailSerivce)
		{
			this._mailSerivce = mailSerivce;
		}
		
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet("contact")]
		public IActionResult Contact()
		{
			return View();
		}

		[HttpPost("contact")]
		public IActionResult Contact(ContactViewModel model)
		{
			if (ModelState.IsValid)
			{
				_mailSerivce.SendMassage("shawn@wildermuth.com", model.Subject, $"From: {model.Name} - {model.Email}, Message {model.Message}");
				ViewBag.UserMessage = "Mail Sent";
				ModelState.Clear();
			}

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
