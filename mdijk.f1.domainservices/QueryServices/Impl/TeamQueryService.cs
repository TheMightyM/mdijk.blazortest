using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mdijk.f1.domain.Data;
using Microsoft.EntityFrameworkCore;

namespace mdijk.f1.domainservices.QueryServices.Impl
{
	public class TeamQueryService : ITeamQueryService
	{
		private readonly FormulaContext _context;

		public TeamQueryService(FormulaContext context)
		{
			_context = context;
		}

		public async Task<List<Team>> GetTeams()
		{
			var results = await _context.Teams.ToListAsync();
			return results;
		}

		public async Task<IList<EventResultMetaData>> GetAllEventResultMetaDataForTeam(int teamId)
		{

			var andereKantop = await _context.EventResultMetaDatas
								.Include(ermd => ermd.EventResult)
									.ThenInclude(er => er.DriverEntry)
										.ThenInclude(de => de.Entry)
											.ThenInclude(e => e.Team)
								.Include(ermd => ermd.EventResult)
									.ThenInclude(er => er.RaceEvent)
										.ThenInclude(re => re.Season)
								.Where(ermd => ermd.EventResult.DriverEntry.Entry.TeamId == teamId)
								.ToListAsync();

			return andereKantop;
		}

		public async Task<Team> GetTeamById(int teamId)
		{
			return await _context.Teams.FirstAsync(t => t.TeamId == teamId);
		}

		public async Task<int> GetEventCountForTeam(int teamId)
		{

			var m = await _context.EventResults
					.Include(er => er.DriverEntry)
						.ThenInclude(de => de.Entry)
					.Where(er => er.DriverEntry.Entry.TeamId == teamId)
					.Select(x => x.RaceEventId)
					.Distinct()
					.CountAsync();

			return m;
		}

		public async Task<IList<Season>> GetAllSeasonsForTeam(int teamId)
		{
			return await _context.Seasons
					.Include(s => s.Entries)
					.Where(s => s.Entries.Any(e => e.TeamId == teamId))
					.Select(s => new Season { SeasonId = s.SeasonId, SeasonYear = s.SeasonYear })
					.AsNoTracking()
					.ToListAsync();
		}

		public async Task<int> GetMaxRacesInSeasonForTeam(int teamId)
		{
			var eventCountPerSeason = await _context.Seasons
				.Include(s => s.Entries)
				.Include(s => s.Events)
				.Where(s => s.Entries.Any(e => e.TeamId == teamId))
				.Select(s => s.Events.Count())
				.ToListAsync();

			if (eventCountPerSeason.Any()) return eventCountPerSeason.Max();

			return 0;
		}

		public async Task<int> GetNumberOfDriversForTeamInSeason(int teamId, int seasonId)
		{
			var season = await _context.Seasons
					.Include(s => s.Entries)
						.ThenInclude(e => e.Drivers)
					.Where(s => s.SeasonId == seasonId)
					.FirstOrDefaultAsync();

			var entry = season.Entries.Where(x => x.TeamId == teamId).FirstOrDefault();

			if (entry == null) return 0;
			return entry.Drivers.Count();

		}
	}
}
