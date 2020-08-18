using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared.Submit
{
	public class AddRaceEventModel
	{
		public int RaceEventId { get; set; }
		public string SequenceNr { get; set; }
		public string RaceEventName { get; set; }
		public string CircuitId { get; set; }
		public int SeasonId { get; set; }
	}
}
