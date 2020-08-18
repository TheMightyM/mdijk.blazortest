using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared.Models.SeasonResults
{
	public class SeasonRaceEvent
	{
		public int RaceEventId { get; set; }
		public int SequenceNumber { get; set; }

		public string RaceEventName { get; set; }

		public Circuit Circuit { get; set; }

		public ICollection<SeasonRaceEventResult> Results { get; set; }
	}
}
