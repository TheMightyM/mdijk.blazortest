using System;
using System.Net.Http;
using System.Threading.Tasks;
using Importer.xml;
using System.Net.Http.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using mdijk.blazoring.Shared.Models.SeasonResults;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Importer
{

	
	class Program
	{
		public static readonly HttpClient HTTPCLIENT = new HttpClient();

		static async Task Main(string[] args)
		{
			try
			{
				var ts = new TimeSpan(0, 0, 0, 0, 92833);
				Console.WriteLine(ts.TotalHours > 1);
				Console.WriteLine(ts.ToString(@"hh\:mm\:ss\:fff"));

				return;
				var p = new Program();
				await p.RunImport();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public async Task RunImport()
		{			
			Program.HTTPCLIENT.BaseAddress = new Uri("http://localhost:57256");
		
			try
			{
				SeasonImporter si = new SeasonImporter();
				//await si.ImportSeasons();

				CircuitImporter ci = new CircuitImporter();
				//await ci.ImportCircuit();

				EventImporter ei = new EventImporter();
				//await ei.ImportEvents();

				DriverImporter di = new DriverImporter();
				//await di.ImportDrivers();

				TeamImporter ti = new TeamImporter();
				//await ti.ImportTeams();

				ResultImporter ri = new ResultImporter();
				//await ri.ImportResults();

				CSV.LaptimeImporter li = new CSV.LaptimeImporter();
				//await li.Import();

				CSV.PitstopImporter pitstopImporter= new CSV.PitstopImporter();
				//await pitstopImporter.Import();

				MLE.SpecialLaptimeExporter sple = new MLE.SpecialLaptimeExporter();
				await sple.ExportSpecialLaptimes();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}
		}

		public Program()
		{
			
		}
	}
}
