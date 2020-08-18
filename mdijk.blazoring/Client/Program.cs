using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using mdijk.blazoring.Client.Services;
using mdijk.blazoring.Shared.Clients;

namespace mdijk.blazoring.Client
{
	public class Program
	{

		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("app");

			Constants.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
			builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
			builder.Services.AddSingleton(s => new DriverNameServer(new Uri(builder.HostEnvironment.BaseAddress)));

			builder.Services.AddTransient(sp => new CircuitClient(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }));
			builder.Services.AddTransient(sp => new DriverClient(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }));
			builder.Services.AddTransient(sp => new EngineClient(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }));
			builder.Services.AddTransient(sp => new EventResultClient(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }));
			builder.Services.AddTransient(sp => new SeasonClient(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }));
			builder.Services.AddTransient(sp => new TeamClient(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }));
			builder.Services.AddTransient(sp => new LaptimeClient(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }));
			builder.Services.AddTransient(sp => new MachinedLearningClient(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }));

			await builder.Build().RunAsync();
		}
	}

	public static class Constants
	{
		public static Uri BaseAddress { get; set; }
	}
}
