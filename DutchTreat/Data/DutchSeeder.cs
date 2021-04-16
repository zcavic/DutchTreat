using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
	public class DutchSeeder
	{
		private readonly DutchContext ctx;
		private readonly IWebHostEnvironment env;
		private readonly UserManager<StoreUser> userManager;

		public DutchSeeder(DutchContext ctx, IWebHostEnvironment env, UserManager<StoreUser> userManager)
		{
			this.ctx = ctx;
			this.env = env;
			this.userManager = userManager;
		}

		public async Task SeedAsync()
		{
			ctx.Database.EnsureCreated();

			StoreUser user = await userManager.FindByEmailAsync("zeljkochavic@gmail.com");

			if (user == null)
			{
				user = new StoreUser()
				{
					FistName = "Zeljko",
					LastName = "Cavic",
					Email = "zeljkochavic@gmail.com",
					UserName = "zeljkochavic@gmail.com"
				};
				var results = await userManager.CreateAsync(user, "P@ss0rd!");
				if (results != IdentityResult.Success)
					throw new InvalidOperationException("Could not create new user in seeder");

			}

			if (ctx.Products.Any())
				return;

			var filePath = Path.Combine(env.ContentRootPath, "Data/art.json");
			var json = File.ReadAllText(filePath);
			var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);
			ctx.Products.AddRange(products);

			var order = ctx.Orders.Where(o => o.Id == 1).FirstOrDefault();

			if(order != null)
			{
				order.User = user;
				order.Items = new List<OrderItem>()
				{
					new OrderItem()
					{
						Product = products.First(),
						Quantity = 5,
						UnitPrice = products.First().Price
					}
				};
			};

			ctx.SaveChanges();
		}
	}
}
