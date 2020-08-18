using mdijk.f1.domain.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mdijk.f1.domainservices.QueryServices
{
	public interface ILaptimeQueryService
	{
		Task<ICollection<Laptime>> GetAllLaptimesForEvent(int raceEventId);

		Task<ICollection<Laptime>> GetAllLaptimesForEventAndDriver(int raceEventId, int driverId);

		Task<ICollection<Laptime>> AllLapsForDriver(int driverId);

		Task<ICollection<Laptime>> GetSpecificLaptime(int seasonYear, int circuitId, int driverId, int lapnumber, int position);
	}
}
