using System.Collections.Generic;
using System.Threading.Tasks;
using mdijk.f1.domain.Data;

namespace mdijk.f1.domainservices.QueryServices
{
	public interface IRaceEventQueryService
	{
		Task<List<EventResult>> GetResultsForEvent(int eventId);
		Task<List<Entry>> GetEntriesForEvent(int eventId);
		Task<RaceEvent> GetMostRecentCompletedRace();
		Task<ICollection<Driver>> GetDriversForEvent(int teamId, int eventId);

		Task<RaceEvent> GetRaceEventAndSeasonById(int eventId);

		Task<int> GetMaxsLapsForRaceEvent(int eventId);
	}
}
