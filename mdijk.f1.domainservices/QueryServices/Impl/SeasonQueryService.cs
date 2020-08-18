using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mdijk.f1.domain.Data;
using Microsoft.EntityFrameworkCore;

namespace mdijk.f1.domainservices.QueryServices.Impl
{

	public class SeasonQueryService : ISeasonQueryService
	{
		private readonly FormulaContext _context;

		public SeasonQueryService(FormulaContext context)
		{
			_context = context;
		}

		public async Task<List<Season>> GetSeasons()
		{
			var results = await _context.Seasons.ToListAsync();
			return results;
		}


		public async Task<Season> GetSeasonById(int seasonId)
		{
			var season = await _context.Seasons.Where(x => x.SeasonId == seasonId)
				.Include(s => s.Entries)
					.ThenInclude(e => e.Drivers)
						.ThenInclude(de => de.Driver)
				//.Include(s => s.Entries)
				//					.ThenInclude(e => e.SecondDriver)
				.Include(s => s.Entries)
					.ThenInclude(e => e.Team)
				.Include(s => s.Entries)
					.ThenInclude(e => e.Engine)
				.FirstOrDefaultAsync();

			return season;
		}

		public async Task<List<RaceEvent>> GetRaceEventsForSeason(int seasonId)
		{
			return await _context.RaceEvents.Where(x => x.SeasonId == seasonId)
				.Include(re => re.Season)
				.Include(re => re.Circuit)
				.ToListAsync();
		}

		public async Task<List<RaceEvent>> GetRaceEventsForSeasonWithResults(int seasonId)
		{
			return await _context.RaceEvents.Where(x => x.SeasonId == seasonId)
				.Include(rE => rE.Results)
					.ThenInclude(r => r.DriverEntry)
						.ThenInclude(dE => dE.Driver)
				.Include(rE => rE.Results)
					.ThenInclude(r => r.DriverEntry)
						.ThenInclude(dE => dE.Entry)
							.ThenInclude(e => e.Team)
				.Include(rE => rE.Results)
					.ThenInclude(r => r.DriverEntry)
						.ThenInclude(dE => dE.Entry)
							.ThenInclude(e => e.Engine)
				.Where(x => x.Results != null)
				.OrderBy(x => x.SequenceNumber)
				.ToListAsync();
		}

		public async Task<List<RaceEvent>> GetRaceEventsForSeasonWithResults(int seasonId, bool useLegacy, bool notracking)
		{
			if(useLegacy)
			{
				return await GetRaceEventsForSeasonWithResults(seasonId);
			}

			if (notracking)
			{
				return await _context.RaceEvents.Where(x => x.SeasonId == seasonId)
					.AsNoTracking()
					.Include(rE => rE.Results)
						.ThenInclude(r => r.DriverEntry)
							.ThenInclude(dE => dE.Driver)
							.AsNoTracking()
					.Include(rE => rE.Results)
						.ThenInclude(r => r.DriverEntry)
							.ThenInclude(dE => dE.Entry)
								.ThenInclude(e => e.Team)
								.AsNoTracking()
					.Include(rE => rE.Results)
						.ThenInclude(r => r.DriverEntry)
							.ThenInclude(dE => dE.Entry)
								.ThenInclude(e => e.Engine)
								.AsNoTracking()
					.Where(x => x.Results != null)
					.OrderBy(x => x.SequenceNumber)
					.AsNoTracking()
					.ToListAsync();
			}

			return await _context.RaceEvents.Where(x => x.SeasonId == seasonId)
				.Include(rE => rE.Results)
					.ThenInclude(r => r.DriverEntry)
						.ThenInclude(dE => dE.Driver)
				.Include(rE => rE.Results)
					.ThenInclude(r => r.DriverEntry)
						.ThenInclude(dE => dE.Entry)
							.ThenInclude(e => e.Team)
				.Include(rE => rE.Results)
					.ThenInclude(r => r.DriverEntry)
						.ThenInclude(dE => dE.Entry)
							.ThenInclude(e => e.Engine)
				.Where(x => x.Results != null)
				.OrderBy(x => x.SequenceNumber)
				.ToListAsync();
		}



		public async Task<IDictionary<int, int>> GetNumberOfRacesPerSeason()
		{
			return await _context.Seasons
							.Include(s => s.Events)
							.ToDictionaryAsync(s => s.SeasonYear, s => s.Events.Count());
		}



		public async Task<Season> GetMostReasonSeason()
		{
			return await _context.Seasons.OrderByDescending(x => x.SeasonYear).FirstAsync();
		}

		public async Task<DriverEntry> DriverEntryExistsForSeason(int teamId, int driverId, int seasonId)
		{
			var entry = await _context.Entries
				.Include(e => e.Drivers)
				.Where(e => e.SeasonId == seasonId)
				.Where(e => e.TeamId == teamId)
				.Where(e => e.Drivers.Any())
				.Where(e => e.Drivers.Any(d => d.DriverId == driverId))
				.FirstOrDefaultAsync();


			if (entry != null)
			{
				return entry.Drivers.First(y => y.DriverId == driverId);
			}
			return null;

		}
		public async Task<Entry> GetEntryByNameAndSeason(string name, int seasonId)
		{
			return await _context.Entries
				.FirstOrDefaultAsync(x => x.FullEntryName == name && x.SeasonId == seasonId);
		}
	}
}
