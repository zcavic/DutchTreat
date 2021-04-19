using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
	[Route("api/[Controller]")]
	[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
	public class OrdersController : Controller
	{
		private readonly IDutchRepository repository;
		private readonly ILogger<OrdersController> logger;
		private readonly IMapper mapper;
		private readonly UserManager<StoreUser> userManager;

		public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger, IMapper mapper, UserManager<StoreUser> userManager)
		{
			this.repository = repository;
			this.logger = logger;
			this.mapper = mapper;
			this.userManager = userManager;
		}

		[HttpGet]
		public IActionResult Get(bool includeItems = true)
		{
			try
			{
				var results = repository.GetAllOrdersByUser(User.Identity.Name, includeItems);
				return Ok(mapper.Map<IEnumerable<OrderViewModel>>(results));
			}
			catch (Exception ex)
			{

				logger.LogError($"Field to get orders: {ex}");
				return BadRequest("Field to get orders");
			}
		}

		[HttpGet("{id:int}")]
		public IActionResult Get(int id)
		{
			try
			{
				var order = repository.GetOrderById(User.Identity.Name, id);
				if (order != null)
					return Ok(mapper.Map<Order, OrderViewModel>(order));
				else
					return NotFound();
			}
			catch (Exception ex)
			{
				logger.LogError($"Failed to get orders: {ex}");
				return BadRequest("Failed to get orders");
			}
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody]OrderViewModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var newOrder = mapper.Map<OrderViewModel, Order>(model);

					if (newOrder.OrderDate == DateTime.MinValue)
						newOrder.OrderDate = DateTime.Now;

					newOrder.User = await userManager.FindByNameAsync(User.Identity.Name);

					repository.AddEntity(newOrder);
					if (repository.SaveAll())
					{
						return Created($"/app/orders/{newOrder.Id}", mapper.Map<Order, OrderViewModel>(newOrder));
					}
				}
				else
				{
					return BadRequest(ModelState);
				}
			}
			catch (Exception ex)
			{
				logger.LogError($"Failed add orders: {ex}");
			}
			return BadRequest("Failed add orders");
		}
	}
}
