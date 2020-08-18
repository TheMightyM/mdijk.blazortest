using AutoMapper;
using mdijk.f1.domainservices.CommandServices;
using mdijk.f1.domain.Data;
using mdijk.blazoring.Server.Mapping;
using mdijk.f1.domainservices.QueryServices;
using mdijk.f1.domainservices.QueryServices.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace mdijk.blazoring.Server
{
	public class Startup
	{
		private static string ConnectionConfig = "mdijk.formula.database";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddControllersWithViews();
			services.AddRazorPages();


			var constr = Configuration.GetConnectionString(ConnectionConfig);
			// connectionstring mooi opmaken
			services.AddDbContext<FormulaContext>(options => options.UseSqlServer(constr));
			services.AddTransient<IAllInOneCommandService, AllInOneCommandService>();

			services.AddTransient<ICircuitQueryService, CircuitQueryService>();
			services.AddTransient<IDriverQueryService, DriverQueryService>();
			services.AddTransient<IEngineQueryService, EngineQueryService>();
			services.AddTransient<IRaceEventQueryService, RaceEventQueryService>();
			services.AddTransient<ISeasonQueryService, SeasonQueryService>();
			services.AddTransient<ITeamQueryService, TeamQueryService>();
			services.AddTransient<ILaptimeQueryService, LaptimeQueryService>();

			// Auto Mapper Configurations

			var mappingConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new DataToModelDriverProfile());
			});

			IMapper mapper = mappingConfig.CreateMapper();
			services.AddSingleton(mapper);

			// Register the Swagger services
			services.AddSwaggerDocument();


		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseWebAssemblyDebugging();
			}
			else
			{
				app.UseExceptionHandler("/Error");
			}

			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();

			// Register the Swagger generator and the Swagger UI middlewares
			app.UseOpenApi();
			app.UseSwaggerUi3();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllers();
				endpoints.MapFallbackToFile("index.html");
			});


			var rootDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
			if(rootDir.StartsWith(@"file:\\"))
			{
				rootDir = rootDir.Replace(@"file:\\", "");
			}
			if (rootDir.StartsWith(@"file:\"))
			{
				rootDir = rootDir.Replace(@"file:\", "");
			}
			// register ML.NET thingies
			MLMARKML.Model.ConsumeModel.MLNetModelPath = $"{rootDir}\\MLModel.zip";

		}
	}
}
