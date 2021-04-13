﻿using DutchTreat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace DutchTreat.Controllers
{
	[Route("api/[Controller]")]
	public class OrdersController : Controller
	{
		private readonly IDutchRepository repository;
		private readonly ILogger<OrdersController> logger;

		public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger)
		{
			this.repository = repository;
			this.logger = logger;
		}

		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				return Ok(repository.GetAllOrders());

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
				var order = repository.GetOrderById(id);
				if (order != null)
					return Ok(order);
				else
					return NotFound();
			}
			catch (Exception ex)
			{
				logger.LogError($"Failed to get orders: {ex}");
				return BadRequest("ailed to get orders");
			}
		}
	}
}
