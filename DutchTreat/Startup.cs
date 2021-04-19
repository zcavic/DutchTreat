using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat
{
	public class Startup
	{
		private readonly IConfiguration config;

		public Startup(IConfiguration config)
		{
			this.config = config;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddIdentity<StoreUser, IdentityRole>(cfg =>
			{
				cfg.User.RequireUniqueEmail = true;
			})
				.AddEntityFrameworkStores<DutchContext>();

			services.AddAuthentication()
				.AddCookie()
				.AddJwtBearer(cfg =>
				{
					cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
					{
						ValidIssuer = config["Token:Issuer"],
						ValidAudience = config["Token:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"]))
					};
				});

			services.AddDbContext<DutchContext>(cnf => 
			{
				cnf.UseSqlServer();
			});

			services.AddTransient<DutchSeeder>();
			services.AddTransient<IMailService, NullMailService>();
			services.AddScoped<IDutchRepository, DutchRepository>();

			services.AddAutoMapper(Assembly.GetExecutingAssembly());

			services.AddControllersWithViews()
				.AddRazorRuntimeCompilation()
				.AddNewtonsoftJson(cfg => cfg.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

			services.AddRazorPages();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/error");
			}

			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();
			
			app.UseEndpoints(cfg =>
			{
				cfg.MapRazorPages();
				cfg.MapControllerRoute("Default",
				"/{controller}/{action}/{id?}",
				new { controller = "App",action = "Index" });
			});
		}
	}
}
