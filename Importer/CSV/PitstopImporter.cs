using mdijk.f1.domain.Data;
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
using System.Text;
using System.Threading.Tasks;

namespace Importer.CSV
{
	public class PitstopImporter
	{
		private FormulaContext _context;
		// key: csv id, value: database id
		private Dictionary<int, int> _drivers = new Dictionary<int, int>();
		private Dictionary<int, RaceEvent> _raceEvents = new Dictionary<int, RaceEvent>();

		public PitstopImporter()
		{
			DbContextOptionsBuilder<FormulaContext> builder = new DbContextOptionsBuilder<FormulaContext>();
			builder.UseSqlServer("Server=OVSPC435\\SQLEXPRESS; Database=formula;User Id=formula; Password=formula;");
			_context = new FormulaContext(builder.Options);
		}

		public async Task Import()
		{
			await ProcessDrivers();
			await ProcessEvents();

			await ProcessPitstops();

		}

		private async Task ProcessDrivers()
		{
			int colId = 0;
			int colFirstname = 4;
			int colLastname = 5;
			string filename = @"D:\git\blazoring\drivers.csv";

			using (StreamReader reader = new StreamReader(filename))
			{
				//eerste regel is zooi
				await reader.ReadLineAsync();
				while (!reader.EndOfStream)
				{
					var line = await reader.ReadLineAsync();
					var splitted = Split(line);
					var id = Convert.ToInt32(splitted[colId]);
					var firstname = splitted[colFirstname].Replace("\"", "");
					var lastname = splitted[colLastname].Replace("\"", "");

					var driver = await _context.Drivers
						.AsNoTracking()
						.Where(x => x.FirstName == firstname
					&& x.LastName == lastname).FirstOrDefaultAsync();
					if (driver == null)
					{
						throw new Exception($"Driver {firstname} {lastname} does not exist!");
					}

					_drivers.Add(driver.DriverId, id);
				}
			}


			return;
		}

		private async Task ProcessEvents()
		{
			int colId = 0;
			int colYear = 1;
			int colSequence = 2;
			string filename = @"D:\git\blazoring\races.csv";

			using (StreamReader reader = new StreamReader(filename))
			{
				//eerste regel is zooi
				await reader.ReadLineAsync();
				while (!reader.EndOfStream)
				{
					var line = await reader.ReadLineAsync();
					var splitted = Split(line);
					var id = Convert.ToInt32(splitted[colId]);
					var year = Convert.ToInt32(splitted[colYear]);
					var sequenceNumber = Convert.ToInt32(splitted[colSequence]);

					var raceEvent = await _context.RaceEvents
						.AsNoTracking()
						.Include(r => r.Season)
						.AsNoTracking()
						.Where(x => x.Season.SeasonYear == year
					&& x.SequenceNumber == sequenceNumber).FirstOrDefaultAsync();
					if (raceEvent == null)
					{
						Console.WriteLine($"RaceEvent {year} {sequenceNumber} does not exist!");
						continue;
					}

					_raceEvents.Add(id, raceEvent);
				}
			}
		}

		private async Task ProcessPitstops()
		{
			int colRaceId = 0;
			int coldriverId = 1;
			int colLapnr = 2;
			int colMil = 6;

			string filename = @"D:\git\blazoring\pit_stops.csv";

			using (StreamReader reader = new StreamReader(filename))
			{
				//eerste regel is zooi
				await reader.ReadLineAsync();
				// key: driverId uit geval, driverEntryId
				// dus: als we in het bestand een driverId vinden.
				// gaan we naar _drivers[driverId] om DRIVERID database te krijgen
				// met dat nummer zoeken we in de database naar het DriverEntryId dat bij deze rijder en dit event hoort
				// dat nummer stoppen in de de entries database.
				// daarna, als een op een regel dus een DriverId tegenkomen, weten we het juiste driverentryid nummer
				// man, man, man.
				var entries = new Dictionary<int, int>();
				int currentRaceId = -1;
				int counter = 1;
				//eerste regel is zooi
				await reader.ReadLineAsync();
				while (!reader.EndOfStream)
				{
					var line = await reader.ReadLineAsync();
					var splitted = Split(line);

					int raceId = Convert.ToInt32(splitted[colRaceId]);
					int driverId = Convert.ToInt32(splitted[coldriverId]);
					int lap = Convert.ToInt32(splitted[colLapnr]);
					int milli = Convert.ToInt32(splitted[colMil]);

					
					if (raceId != currentRaceId)
					{

						Console.WriteLine($"New race!: {raceId}");
						var convertedRaceId = _raceEvents[raceId].RaceEventId;
						currentRaceId = raceId;
						var entriesTemp = await _context.EventResults
							.AsNoTracking()
								.Include(er => er.DriverEntry)
								.AsNoTracking()
								.Where(er => er.RaceEventId == convertedRaceId)
								.Select(x => x.DriverEntry)
								.ToListAsync();

						entries = new Dictionary<int, int>();

						foreach (var et in entriesTemp)
						{
							var mappedDriverId = _drivers[et.DriverId];
							entries.Add(mappedDriverId, et.DriverEntryId);
						}
					}

					var raceEvent = _raceEvents[raceId].RaceEventId;
					if (!entries.ContainsKey(driverId))
					{
						Console.WriteLine("possible race not run!");
						counter++;
						continue;
					}
					var driverToUse = entries[driverId];

					var pitstop = new Pitstop
					{
						RaceEventId = raceEvent,
						DriverEntryId = driverToUse,
						LapNumber = lap,
						Milliseconds = milli
					};

					Console.WriteLine($"adding lap nr {counter}: {milli}ms");
					_context.Pitstops.Add(pitstop);

					//elke 1000 committen
					//if (counter % 1000 == 0)
					//{
					//await _context.SaveChangesAsync();
					//}

					counter++;
				}
			}

			await _context.SaveChangesAsync();
		}


		private string[] Split(string line)
		{
			return line.Split(new[] { "," }, StringSplitOptions.None);
		}
	}
}
