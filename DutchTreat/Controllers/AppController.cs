﻿using DutchTreat.Data;
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
		private readonly DutchContext _context;

		public AppController(IMailService mailSerivce, DutchContext context)
		{
			this._mailSerivce = mailSerivce;
			this._context = context;
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

		public IActionResult Shop()
		{
			var results = _context.Products
				.OrderBy(p => p.Category)
				.ToList();

			return View(results);
		}
	}
}
