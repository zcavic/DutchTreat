using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
	public class DutchRepository : IDutchRepository
	{
		private readonly DutchContext ctx;

		public DutchRepository(DutchContext ctx)
		{
			this.ctx = ctx;
		}

		public void AddEntity(object model)
		{
			ctx.Add(model);
		}

		public IEnumerable<Order> GetAllOrders(bool includeItems)
		{
			if (includeItems)
				return ctx.Orders.Include(o => o.Items)
					.ThenInclude(i => i.Product)
					.ToList();
			else
				return ctx.Orders
					.ToList();
		}

		public IEnumerable<Product> GetAllProducts()
		{
			return ctx.Products
				.OrderBy(x => x.Title)
				.ToList();
		}

		public Order GetOrderById(int id)
		{
			return ctx.Orders
				.Include(o => o.Items)
				.ThenInclude(i => i.Product)
				.Where(o => o.Id == id)
				.FirstOrDefault();
		}

		public IEnumerable<Product> GetProductByCategory(string category)
		{
			return ctx.Products
				.Where(p => p.Category == category)
				.ToList();
		}

		public bool SaveAll()
		{
			return ctx.SaveChanges() > 0;
		}
	}
}
