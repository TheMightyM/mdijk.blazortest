using mdijk.f1.domain.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdijk.f1.domainservices.QueryServices.Impl
{
	public class LaptimeQueryService : ILaptimeQueryService
	{
		private readonly FormulaContext _context;

		public LaptimeQueryService(FormulaContext context)
		{
			_context = context;
		}

		public async Task<ICollection<Laptime>> GetAllLaptimesForEvent(int raceEventId)
		{
			return await _context.Laptimes
				.Include(l => l.DriverEntry)
					.ThenInclude(de => de.Driver)
				.Where(l => l.RaceEventId == raceEventId)
				.ToListAsync();
		}

		public async Task<ICollection<Laptime>> GetAllLaptimesForEventAndDriver(int raceEventId, int driverId)
		{
			return await _context.Laptimes
			.Include(l => l.DriverEntry)
				.ThenInclude(de => de.Driver)
			.Where(l => l.RaceEventId == raceEventId
			&& l.DriverEntry.DriverId == driverId)
			.ToListAsync();
		}

		public async Task<ICollection<Laptime>> AllLapsForDriver(int driverId)
		{
			return await _context.Laptimes
					.Include(l => l.DriverEntry)
						.ThenInclude(de => de.Driver)
					.Include(l => l.RaceEvent)
						.ThenInclude(re => re.Season)
					.Where(l => l.DriverEntry.DriverId == driverId)
					.OrderBy(l => l.RaceEvent.Season.SeasonYear)
						.ThenBy(l => l.RaceEvent.SequenceNumber)
							.ThenBy(l => l.LapNumber)
					.ToListAsync();
		}

		public async Task<ICollection<Laptime>> GetSpecificLaptime(int seasonYear, int circuitId, int driverId, int lapnumber, int position)
		{
			return await _context.Laptimes
					.Include(l => l.DriverEntry)
						.ThenInclude(de => de.Driver)
					.Include(l => l.RaceEvent)
						.ThenInclude(re => re.Season)
						.Where(l => l.RaceEvent.CircuitId == circuitId
						&& l.RaceEvent.Season.SeasonYear == seasonYear
						&& l.DriverEntry.DriverId == driverId
						&& l.LapNumber == lapnumber
						//&& l.CurrentPosition == position
						)
						.ToListAsync();
		}
	}
}
