using mdijk.f1.domain.Data;
using mdijk.f1.domain.Migrations;
using mdijk.f1.domainservices.CommandServices;
using mdijk.f1.domainservices.QueryServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Importer.MLE
{
	public class SpecialLaptimeExporter
	{
		private FormulaContext _context;

		public SpecialLaptimeExporter()
		{
			DbContextOptionsBuilder<FormulaContext> builder = new DbContextOptionsBuilder<FormulaContext>();
			builder.UseSqlServer("Server=OVSPC435\\SQLEXPRESS; Database=formula;User Id=formula; Password=formula;");
			_context = new FormulaContext(builder.Options);
		}

		public async Task ExportSpecialLaptimes()
		{
			string header = "SeasonYear, SequenceNumber, CircuitId, DriverId, LapNumber, CurrentPosition, Milliseconds";

			using (var writer = new StreamWriter(@"D:\git\mlmark\data\filterslaps.csv"))
			{
				await writer.WriteLineAsync(header);
				/*
				 * per race waar we laptimes van hebben
				 * pak de snelste ronde van dat event.
				 * print hoeveel er boven de 30% van die tijd zitten
				 * 
				 */


				var groupedByRaceEvent = (await _context.Laptimes
											.Include(l => l.DriverEntry)
											.Include(l => l.RaceEvent)
												.ThenInclude(re => re.Season)
											.ToListAsync())
											.GroupBy(l => l.RaceEventId)
											.Select(l => new
											{
												RaceEventId = l.Key,
												Laptimes = l.Select(t => new Laptime
												{
													CurrentPosition = t.CurrentPosition,
													DriverEntry = new DriverEntry
													{
														DriverId = t.DriverEntry.DriverId,
														DriverEntryId = t.DriverEntry.DriverEntryId
													},
													LapNumber = t.LapNumber,
													LaptimeId = t.LaptimeId,
													Milliseconds = t.Milliseconds,
													RaceEvent = new RaceEvent
													{
														CircuitId = t.RaceEvent.CircuitId,
														SequenceNumber = t.RaceEvent.SequenceNumber,
														Season = new Season
														{
															SeasonYear = t.RaceEvent.Season.SeasonYear
														}
													}
												}).ToList()
											});

				foreach (var re in groupedByRaceEvent)
				{



					var laptimes = re.Laptimes;

					var fastest = laptimes.Min(x => x.Milliseconds);
					var cutoffTime = ((fastest / 100) * 20) + fastest;

					var validLaps = laptimes.Where(x => x.Milliseconds <= cutoffTime).ToList();
					//string header = "SeasonYear, SequenceNumber, CircuitId, DriverId, LapNumber, CurrentPosition, Milliseconds";

					foreach (var validLap in validLaps)
					{
						await writer.WriteLineAsync($"{validLap.RaceEvent.Season.SeasonYear},{validLap.RaceEvent.SequenceNumber}, {validLap.RaceEvent.CircuitId}, {validLap.DriverEntry.DriverId}, {validLap.LapNumber}, {validLap.CurrentPosition}, {validLap.Milliseconds}");
					}
				}
			}
		}
	}
}
