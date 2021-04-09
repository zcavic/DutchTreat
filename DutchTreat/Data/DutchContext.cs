﻿using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace DutchTreat.Data
{
	public class DutchContext : DbContext
	{
		private readonly IConfiguration _config;

		public DutchContext(IConfiguration config)
		{
			this._config = config;
		}

		public DbSet<Product> Products
		{
			get; set;
		}

		public DbSet<Order> Orders
		{
			get; set;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlServer(_config["ConnectionString:DutchContextDb"]);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Order>()
				.HasData(new Order() 
				{ 
					Id = 1,
					OrderDate = DateTime.Now,
					OrderNumber = "12345"
				});
		}
	}
}
