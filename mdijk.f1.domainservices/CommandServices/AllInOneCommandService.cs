using mdijk.f1.domain.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace mdijk.f1.domainservices.CommandServices
{
	public interface IAllInOneCommandService
	{
		// add functies
		Task<int> AddOrUpdateDriver(Driver driver);
		Task<int> AddOrUpdateTeam(Team team);
		Task<int> AddOrUpdateEngine(Engine engine);
		Task<int> AddOrUpdateSeason(Season season);
		Task<int> AddOrUpdateEntry(Entry entry);
		
		Task<int> AddOrUpdateCircuit(Circuit circuit);

		Task<int> AddOrUpdateRaceEvent(RaceEvent raceEvent);

		Task<int> AddOrUpdateEventResult(EventResult result);

		Task<int> AddEventResultMetaData(EventResultMetaData metaData);

		Task SetEntryTeam(int entryId, int teamId);
		Task<int> AddDriverToEntry(int entryId, int driverId);
		//Task RemoveDriverFromEntry(int entryId, int driverId, EntryDriverPosition driverPosition);
		Task AddEngineToEntry(int entryId, int engineId);

		Task DeleteEventResult(int raceEventId);

		Task AddLaptime(Laptime laptime);

	}

	public class AllInOneCommandService : IAllInOneCommandService
	{
		private readonly FormulaContext _context;

		public AllInOneCommandService(FormulaContext context)
		{
			_context = context;
		}

		

		public async Task<int> AddOrUpdateDriver(Driver driver)
		{			
			if (driver.DriverId != 0)
			{
				var existing = await _context.Drivers.FirstAsync(x => x.DriverId == driver.DriverId);
				existing.Country = driver.Country;
				existing.DriverNumber = driver.DriverNumber;
				existing.FirstName = driver.FirstName;
				existing.LastName = driver.LastName;
			}
			else if (await _context.Drivers.AnyAsync(x => x.FirstName == driver.FirstName
			&& x.LastName == driver.LastName
			&& x.DriverNumber == driver.DriverNumber))
			{
				return -123;
			}
			else
			{
				await _context.Drivers.AddAsync(driver);
			}

			await _context.SaveChangesAsync();

			return driver.DriverId;

		}

		public async Task<int> AddOrUpdateEngine(Engine engine)
		{
			if (engine.EngineId != 0)
			{
				var existing = await _context.Engines.FirstAsync(x => x.EngineId == engine.EngineId);
				existing.Country = engine.Country;
				existing.EngineName= engine.EngineName;
			}
			else
			{
				await _context.Engines.AddAsync(engine);
			}

			await _context.SaveChangesAsync();

			return engine.EngineId;
		}

		public async Task<int> AddOrUpdateEntry(Entry entry)
		{

			if (entry.EntryId != 0)
			{
				var existing = await _context.Entries.FirstAsync(x => x.EntryId == entry.EntryId);
				existing.FullEntryName = entry.FullEntryName;
			}
			else
			{
				await _context.Entries.AddAsync(entry);
			}

			await _context.SaveChangesAsync();

			return entry.EntryId;
		}

		public async Task<int> AddOrUpdateSeason(Season season)
		{
			if (season.SeasonId != 0)
			{
				var existing = await _context.Seasons.FirstAsync(x => x.SeasonId == season.SeasonId);
				existing.SeasonYear = season.SeasonYear;
			}
			else
			{
				await _context.Seasons.AddAsync(season);
			}

			await _context.SaveChangesAsync();

			return season.SeasonId;
		}


		public async Task<int> AddOrUpdateCircuit(Circuit circuit)
		{
			// vanwege import, kijk of het circuit naam niet al bestaat
		
			if (circuit.CircuitId != 0)
			{
				var existing = await _context.Circuits.FirstAsync(x => x.CircuitId == circuit.CircuitId);
				existing.CircuitName = circuit.CircuitName;
				existing.Country = circuit.Country;
			}
			else if (await _context.Circuits.AnyAsync(x => x.CircuitName == circuit.CircuitName))
			{
				return -123;
			}

			else
			{
				await _context.Circuits.AddAsync(circuit);
			}

			await _context.SaveChangesAsync();

			return circuit.CircuitId;
		}

		public async Task<int> AddOrUpdateTeam(Team team)
		{
			if (team.TeamId != 0)
			{
				var existing = await _context.Teams.FirstAsync(x => x.TeamId == team.TeamId);
				existing.TeamName = team.TeamName;
				existing.Country = team.Country;
			}
			else if( (await _context.Teams.AnyAsync(x => x.TeamName == team.TeamName)))
			{
				return -1;
			}
			else
			{
				await _context.Teams.AddAsync(team);
			}

			await _context.SaveChangesAsync();

			return team.TeamId;
		}


		public async Task<int> AddOrUpdateRaceEvent(RaceEvent raceEvent)
		{
			// eerst checken of er in dat jaar al een race met deze naam is
			
			if (raceEvent.RaceEventId != 0)
			{
				var existing = await _context.RaceEvents.FirstAsync(x => x.RaceEventId == raceEvent.RaceEventId);
				existing.RaceEventName = raceEvent.RaceEventName;
				existing.CircuitId = raceEvent.CircuitId;
				existing.SeasonId = raceEvent.SeasonId;
				existing.SequenceNumber = raceEvent.SequenceNumber;
			}
			else if (await _context.RaceEvents.AnyAsync(x => x.RaceEventName == raceEvent.RaceEventName
			&& x.SeasonId == raceEvent.SeasonId
			&& x.SequenceNumber == raceEvent.SequenceNumber))
			{
				return -123;
			}
			else
			{
				await _context.RaceEvents.AddAsync(raceEvent);
			}

			await _context.SaveChangesAsync();

			return raceEvent.RaceEventId;
		}

		public async Task<int> AddOrUpdateEventResult(EventResult eventResult)
		{
			if (eventResult.EventResultId != 0)
			{
				var existing = await _context.EventResults.FirstAsync(x => x.EventResultId == eventResult.EventResultId);
				existing.DriverEntryId = eventResult.DriverEntryId;
				existing.FastestLap = eventResult.FastestLap;
				existing.FinishingPosition = eventResult.FinishingPosition;
				existing.RaceEventId = eventResult.RaceEventId;
			}
			else
			{
				await _context.EventResults.AddAsync(eventResult);
			}

			await _context.SaveChangesAsync();

			return eventResult.EventResultId;
		}

		// alleen ADD op dit moment
		public async Task<int> AddEventResultMetaData(EventResultMetaData metaData)
		{
			await _context.EventResultMetaDatas.AddAsync(metaData);
			await _context.SaveChangesAsync();			
			return metaData.EventResultMetaDataId;
		}

		public async Task<int> AddDriverToEntry(int entryId, int driverId)
		{

			var dE = new DriverEntry
			{
				DriverId = driverId,
				EntryId = entryId
			};
			_context.DriverEntries.Add(dE);
			await _context.SaveChangesAsync();

			return dE.DriverEntryId;
		}

		public async Task AddEngineToEntry(int entryId, int engineId)
		{
			var entry = await _context.Entries.FirstAsync(x => x.EntryId == entryId);
			var engine = await _context.Engines.FirstAsync(x => x.EngineId == engineId);

			entry.Engine = engine;
			await _context.SaveChangesAsync();
		}

		/*public async Task RemoveDriverFromEntry(int entryId, int driverId, EntryDriverPosition driverPosition)
		{
			var entry = await _context.Entries.FirstAsync(x => x.EntryId == entryId);

			if (driverPosition == EntryDriverPosition.FirstDriver)
			{
				entry.FirstDriver = null;
			}
			else
			{
				entry.SecondDriver = null;
			}
			
			await _context.SaveChangesAsync();
		}*/

		public async Task SetEntryTeam(int entryId, int teamId)
		{
			var entry = await _context.Entries.FirstAsync(x => x.EntryId == entryId);
			var team = await _context.Teams.FirstAsync(x => x.TeamId == teamId);

			entry.Team = team;
			await _context.SaveChangesAsync();
		}

		public async Task DeleteEventResult(int raceEventId)
		{
			// eerst alle metadata pakken
			var ermds = await _context.EventResultMetaDatas
				.Include(e => e.EventResult)
				.Where(x => x.EventResult.RaceEventId == raceEventId)
				.ToListAsync();

			foreach(var ermd in ermds)
			{
				_context.Remove<EventResultMetaData>(ermd);
			}

			var results = await _context.EventResults
				.Where(x => x.RaceEventId == raceEventId)
				.ToListAsync();

			foreach(var result in results)
			{
				_context.Remove(result);
			}

			await _context.SaveChangesAsync();
		}

		
		public async Task AddLaptime(Laptime laptime)
		{
			_context.Laptimes.Add(laptime);
			await _context.SaveChangesAsync();
		}
	}
}
