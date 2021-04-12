﻿using DutchTreat.Data.Entities;
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

		public IEnumerable<Product> GetAllProducts()
		{
			return ctx.Products
				.OrderBy(x => x.Title)
				.ToList();
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
