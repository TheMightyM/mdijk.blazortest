using System.Collections.Generic;
using System.Threading.Tasks;
using mdijk.f1.domain.Data;

namespace mdijk.f1.domainservices.QueryServices
{
	/// <summary>
	/// doet verder niet veel.
	/// </summary>
	public interface IEntryQueryService
	{
		Task<List<Entry>> GetEntries();

	}
}
