using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared.Models
{
	public class EventResultMetaData
	{
		public int EventResultMetaDataId { get; set; }
		public string Key { get; set; }
		public string Value { get; set; }

	}

	public class EventResultMetaDataTyped
	{
		public string Points { get; set; }
		public int GridPosition { get; set;}
		public int Laps { get; set; }
		public string Status { get; set; }
		public string Time { get; set; }
		public string FastestLap { get; set; }
		public int FastestLapRank { get; set; }
	}

}
