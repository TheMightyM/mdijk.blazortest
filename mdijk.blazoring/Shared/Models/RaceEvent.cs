using System.Collections.Generic;

namespace mdijk.blazoring.Shared.Models
{
	public class RaceEvent
	{
		public int RaceEventId { get; set; }
		public int SequenceNumber { get; set; }

		public string RaceEventName { get; set; }

		public Circuit Circuit { get; set; }

		public Season Season { get; set; }

		public ICollection<EventResult> Results { get; set; }
	}
}
