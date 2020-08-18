using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared.Models.CircuitEvents
{
	public class HistoricalRaceEvent
	{
		public int RaceEventId { get; set; }
		public int SequenceNumber { get; set; }
		public string RaceEventName { get; set; }
		public int SeasonId { get; set; }
		public int SeasonYear { get; set; }
	}
}
