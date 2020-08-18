using System.Collections.Generic;
using System.Threading.Tasks;
using mdijk.f1.domain.Data;

namespace mdijk.f1.domainservices.QueryServices
{
	public interface IEngineQueryService
	{
		Task<List<Engine>> GetEngines();
	}
}
