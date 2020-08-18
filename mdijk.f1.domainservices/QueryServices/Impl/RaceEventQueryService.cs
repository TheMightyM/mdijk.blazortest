using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mdijk.f1.domain.Data;
using Microsoft.EntityFrameworkCore;

namespace mdijk.f1.domainservices.QueryServices.Impl
{
	public class RaceEventQueryService : IRaceEventQueryService
	{
		private readonly FormulaContext _context;

		public RaceEventQueryService(FormulaContext context)
		{
			_context = context;
		}

		public async Task<List<EventResult>> GetResultsForEvent(int eventId)
		{
			return await _context.EventResults.Where(x => x.RaceEventId == eventId)
				.Include(er => er.DriverEntry)
					.ThenInclude(de => de.Driver)
				.Include(er => er.DriverEntry)
					.ThenInclude(de => de.Entry)
						.ThenInclude(e => e.Team)
				.Include(er => er.MetaData)
				.ToListAsync();
		}

		public async Task<List<Entry>> GetEntriesForEvent(int eventId)
		{

			var events = await _context.RaceEvents
				.Where(rE => rE.RaceEventId == eventId)
					.Include(rE => rE.Season)
						.ThenInclude(s => s.Entries)
							.ThenInclude(e => e.Drivers)
								.ThenInclude(d => d.Driver)
					.Select(s => s.Season.Entries)
					.AsNoTracking()
					.FirstAsync();

			return events;

		}

		public async Task<RaceEvent> GetMostRecentCompletedRace()
		{
			var mostRecentEventId = await _context.EventResults
					.Include(er => er.RaceEvent)
						.ThenInclude(re => re.Season)
					.Select(er => new
					{
						er.RaceEvent.RaceEventId,
						er.RaceEvent.Season.SeasonYear,
						er.RaceEvent.SequenceNumber
					})
					.OrderByDescending(x => x.SeasonYear)
					.ThenByDescending(x => x.SequenceNumber)
					.Select(x => x.RaceEventId)
					.FirstAsync();


			return await _context.RaceEvents
				.Include(re => re.Circuit)
				.Include(re => re.Season)
				.Where(re => re.RaceEventId == mostRecentEventId)
				.Select(re => new RaceEvent
				{
					RaceEventId = re.RaceEventId,
					RaceEventName = re.RaceEventName,
					SeasonId = re.SeasonId,
					SequenceNumber = re.SequenceNumber,
					Circuit = new Circuit
					{
						CircuitId = re.Circuit.CircuitId,
						CircuitName = re.Circuit.CircuitName,
						Country = re.Circuit.Country
					},
					Season = new Season
					{
						SeasonYear = re.Season.SeasonYear,
						SeasonId = re.Season.SeasonId
					},
					Results = re.Results.Select(r => new EventResult
					{
						FinishingPosition = r.FinishingPosition,
						DriverEntry = new DriverEntry
						{
							DriverEntryId = r.DriverEntry.DriverEntryId,
							Driver = new Driver
							{
								DriverId = r.DriverEntry.Driver.DriverId,
								DriverNumber = r.DriverEntry.Driver.DriverNumber,
								Country = r.DriverEntry.Driver.Country,
								FirstName = r.DriverEntry.Driver.FirstName,
								LastName = r.DriverEntry.Driver.LastName
							},
							Entry = new Entry
							{
								FullEntryName = r.DriverEntry.Entry.FullEntryName,
								Team = new Team
								{
									TeamName = r.DriverEntry.Entry.Team.TeamName,
									TeamId = r.DriverEntry.Entry.Team.TeamId,
									Country = r.DriverEntry.Entry.Team.Country
								},
								EntryId = r.DriverEntry.EntryId
							}
						}
					}).ToList()

				})
				.FirstAsync();
		}

		public async Task<ICollection<Driver>> GetDriversForEvent(int teamId, int eventId)
		{
			return await _context.DriverEntries
						.Include(de => de.Entry)
						.Include(de => de.EventResults)
						.Include(de => de.Driver)
						.Where(de => de.Entry.TeamId == teamId
						 && de.EventResults.Any(x => x.RaceEventId == eventId)
						 )
						.Select(de => de.Driver)
						.ToListAsync();
		}

		public async Task<RaceEvent> GetRaceEventAndSeasonById(int eventId)
		{
			return await _context.RaceEvents
				.Include(r => r.Season)
				.Where(r => r.RaceEventId == eventId)
				.FirstAsync();
		}

		public async Task<int> GetMaxsLapsForRaceEvent(int eventId)
		{
			return await _context.Laptimes
				.Where(x => x.RaceEventId == eventId)
				.MaxAsync(x => x.LapNumber);
		}

	}
}
