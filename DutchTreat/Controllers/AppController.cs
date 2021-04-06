﻿using Microsoft.AspNetCore.Mvc;
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
			return View();
		}

		[HttpPost("contact")]
		public IActionResult Contact(object model)
		{
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
