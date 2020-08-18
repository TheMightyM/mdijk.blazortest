using System.Collections.Generic;
using System.Threading.Tasks;
using mdijk.f1.domain.Data;

namespace mdijk.f1.domainservices.QueryServices
{
	public interface ISeasonQueryService
	{
		Task<List<Season>> GetSeasons();
		Task<Season> GetSeasonById(int seasonId);
		Task<List<RaceEvent>> GetRaceEventsForSeason(int seasonId);
		Task<Season> GetMostReasonSeason();
		Task<List<RaceEvent>> GetRaceEventsForSeasonWithResults(int seasonId);
		Task<IDictionary<int, int>> GetNumberOfRacesPerSeason();
		Task<DriverEntry> DriverEntryExistsForSeason(int teamId, int driverId, int seasonId);
		Task<Entry> GetEntryByNameAndSeason(string name, int seasonId);


	}
}
