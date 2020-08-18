using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mdijk.f1.domain.Data;
using Microsoft.EntityFrameworkCore;

namespace mdijk.f1.domainservices.QueryServices.Impl
{
	public class EntryQueryService : IEntryQueryService
	{
		private readonly FormulaContext _context;

		public EntryQueryService(FormulaContext context)
		{
			_context = context;
		}

		public async Task<List<Entry>> GetEntries()
		{
			var results = await _context.Entries.ToListAsync();
			return results;
		}

		

	}
}
