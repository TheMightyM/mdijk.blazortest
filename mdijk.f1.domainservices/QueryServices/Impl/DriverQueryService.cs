using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mdijk.f1.domain.Data;
using Microsoft.EntityFrameworkCore;

namespace mdijk.f1.domainservices.QueryServices.Impl
{
	public class DriverQueryService : IDriverQueryService
	{
		private readonly FormulaContext _context;

		public DriverQueryService(FormulaContext context)
		{
			_context = context;
		}

		public async Task<IList<EventResultMetaData>> GetAllEventResultMetaDataForDriver(int driverId)
		{
			var andereKantop = await _context.EventResultMetaDatas
								.Include(ermd => ermd.EventResult)
									.ThenInclude(er => er.DriverEntry)
								.Where(ermd => ermd.EventResult.DriverEntry.DriverId == driverId)
								.ToListAsync();

			return andereKantop;
		}

		public async Task<IList<EventResultMetaData>> GetAllEventResultMetaDataForDriver2(int driverId)
		{

			var andereKantop = await _context.EventResultMetaDatas
								.Include(ermd => ermd.EventResult)
									.ThenInclude(er => er.DriverEntry)
								.Include(ermd => ermd.EventResult)
									.ThenInclude(er => er.RaceEvent)
										.ThenInclude(re => re.Season)
								.Where(ermd => ermd.EventResult.DriverEntry.DriverId == driverId)
								.ToListAsync();

			return andereKantop;
		}

		public async Task<List<Driver>> GetDrivers()
		{
			var results = await _context.Drivers.ToListAsync();
			return results;
		}

		public async Task<List<Driver>> GetDrivers(int pageNr, int pageSize)
		{
			var results = await _context.Drivers
				.OrderBy(x => x.LastName)
				.Skip(pageNr * pageSize)
				.Take(pageSize)
				.ToListAsync();
			return results;
		}

		public async Task<int> GetTotalNumberOfDrivers()
		{
			return await _context.Drivers.CountAsync();
		}

		public async Task<Driver> GetDriverById(int driverId)
		{
			return await _context.Drivers.FirstAsync(x => x.DriverId == driverId);
		}	

		public async Task<ICollection<EventResultMetaData>> GetMetaDataForDriverByEventId(int driverId, int eventId)
		{
			return await _context.EventResultMetaDatas
				.Include(md => md.EventResult)
					.ThenInclude(er => er.DriverEntry)
				.Include(md => md.EventResult)
					.ThenInclude(er => er.RaceEvent)
				.Where(md => md.EventResult.RaceEventId == eventId
						&& md.EventResult.DriverEntry.DriverId == driverId)
				.ToListAsync();
		}
	}
}