using System.Collections.Generic;

namespace mdijk.blazoring.Shared.Models
{
	public class Season
	{
		public int SeasonId { get; set; }
		public int SeasonYear { get; set; }
		public List<Entry> Entries { get; set; }
		public List<RaceEvent> RaceEvents { get; set; }
	}
}
