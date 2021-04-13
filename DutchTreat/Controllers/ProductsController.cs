using DutchTreat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace DutchTreat.Controllers
{
	[Route("api/[Controller]")]
	public class ProductsController : Controller
	{
		private readonly IDutchRepository repository;
		private readonly ILogger<ProductsController> logger;

		public ProductsController(IDutchRepository repository, ILogger<ProductsController> logger)
		{
			this.repository = repository;
			this.logger = logger;
		}

		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				return Ok(repository.GetAllProducts());
			}
			catch (Exception ex)
			{
				logger.LogError($"Failed to get products: {ex}");
				return BadRequest("Bad request");
			}
		}
	}
}
