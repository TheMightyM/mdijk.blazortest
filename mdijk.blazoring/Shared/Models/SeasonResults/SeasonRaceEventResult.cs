using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared.Models.SeasonResults
{
	public class SeasonRaceEventResult
	{		
		public int EventResultId { get; set; }
		public int FinishingPosition { get; set; }
		public bool FastestLap { get; set; }

		public DriverEntry DriverEntry { get; set; }
		public IList<EventResultMetaData> MetaData { get; set; }

	}
}
