using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mdijk.f1.domain.Data;
using Microsoft.EntityFrameworkCore;
namespace mdijk.f1.domainservices.QueryServices.Impl
{
	public class EngineQueryService : IEngineQueryService
	{
		private readonly FormulaContext _context;

		public EngineQueryService(FormulaContext context)
		{
			_context = context;
		}

		public async Task<List<Engine>> GetEngines()
		{
			var results = await _context.Engines.ToListAsync();
			return results;
		}
	}
}
