using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mdijk.f1.domain.Data
{
	public class Season
	{
		public int SeasonId { get; set; }
		public int SeasonYear { get; set; }
		public List<Entry> Entries { get; set; }
		public List<RaceEvent> Events { get; set; }
	}
}
