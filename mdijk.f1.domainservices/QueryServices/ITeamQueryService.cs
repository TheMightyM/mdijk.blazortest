using System.Collections.Generic;
using System.Threading.Tasks;
using mdijk.f1.domain.Data;

namespace mdijk.f1.domainservices.QueryServices
{
	public interface ITeamQueryService
	{
		Task<List<Team>> GetTeams();
		Task<IList<EventResultMetaData>> GetAllEventResultMetaDataForTeam(int teamId);
		Task<Team> GetTeamById(int teamId);
		Task<int> GetEventCountForTeam(int teamId);
		Task<IList<Season>> GetAllSeasonsForTeam(int teamId);
		Task<int> GetMaxRacesInSeasonForTeam(int teamId);
		Task<int> GetNumberOfDriversForTeamInSeason(int teamId, int seasonId);

	}
}
