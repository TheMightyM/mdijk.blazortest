using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mdijk.f1.domain.Data;
using Microsoft.EntityFrameworkCore;

namespace mdijk.f1.domainservices.QueryServices.Impl
{
	public class CircuitQueryService : ICircuitQueryService
	{
		private readonly FormulaContext _context;

		public CircuitQueryService(FormulaContext context)
		{
			_context = context;
		}
		public async Task<List<Circuit>> GetCircuits()
		{
			var results = await _context.Circuits.ToListAsync();
			return results;
		}

		public async Task<ICollection<RaceEvent>> GetRaceEventsAtCircuit(int circuitId)
		{
			var result = await _context.RaceEvents
				.Include(re => re.Season)
				.Where(re => re.CircuitId == circuitId)
				.Select(re => new RaceEvent
				{
					CircuitId = circuitId,
					RaceEventId = re.RaceEventId,
					Season = new Season
					{
						SeasonId = re.SeasonId,
						SeasonYear = re.Season.SeasonYear
					},
					SeasonId = re.SeasonId,
					RaceEventName = re.RaceEventName,
					SequenceNumber = re.SequenceNumber
				})
				.ToListAsync();

			return result;
		}


	}
}
