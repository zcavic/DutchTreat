﻿using Microsoft.AspNetCore.Mvc;
using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using DutchTreat.Data.Entities;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace DutchTreat.Controllers
{
	public class AccountController : Controller
	{
		private readonly ILogger<AccountController> logging;
		private readonly SignInManager<StoreUser> signInManager;
		private readonly UserManager<StoreUser> userManager;
		private readonly IConfiguration config;

		public AccountController(ILogger<AccountController> logging, SignInManager<StoreUser> signInManager, UserManager<StoreUser> userManager, IConfiguration config)
		{
			this.logging = logging;
			this.signInManager = signInManager;
			this.userManager = userManager;
			this.config = config;
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

		[HttpPost]
		public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByNameAsync(model.Username);
				if (user != null)
				{
					var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
					if (result.Succeeded)
					{
						var claims = new[]
						{
							new Claim(JwtRegisteredClaimNames.Sub, user.Email),
							new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
							new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
						};

						var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"]));
						var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
						var token = new JwtSecurityToken(config["Token:Issuer"], config["Token:Audience"], claims, signingCredentials: creds, expires: DateTime.UtcNow.AddMinutes(20));

						return Created("", new
						{
							token = new JwtSecurityTokenHandler().WriteToken(token),
							expiration = token.ValidTo
						} );
					}
				}
			}
			return BadRequest();
		}
	}
}
