using System.Collections.Generic;
using System.Threading.Tasks;
using mdijk.f1.domain.Data;

namespace mdijk.f1.domainservices.QueryServices
{
	public interface IDriverQueryService
	{
		Task<List<Driver>> GetDrivers();
		Task<List<Driver>> GetDrivers(int pageNr, int pageSize);
		Task<Driver> GetDriverById(int driverId);
		Task<int> GetTotalNumberOfDrivers();
		Task<IList<EventResultMetaData>> GetAllEventResultMetaDataForDriver(int driverId);

		/// <summary>
		/// deze heeft extra includes
		/// </summary>
		/// <param name="driverId"></param>
		/// <returns></returns>
		Task<IList<EventResultMetaData>> GetAllEventResultMetaDataForDriver2(int driverId);
		Task<ICollection<EventResultMetaData>> GetMetaDataForDriverByEventId(int driverId, int eventId);
	}
}