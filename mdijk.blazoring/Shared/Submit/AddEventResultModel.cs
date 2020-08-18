using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared.Submit
{
	public class AddEventResultModel
	{
		public int EventId { get; set; }
		public IList<AddEventResultModelRow> Rows { get; set; }
	}

	public class AddEventResultModelRow
	{
		public int FinishingPosition { get; set; }

		public string DriverEntryId { get; set; }

		public string Points { get; set; }
		public string GridPosition { get; set; }
		public string Laps { get; set; }
		public string Status { get; set; }
		public string Time { get; set; }
		public string FastestLap { get; set; }
		public string FastestLapRank { get; set; }

		public ICollection<MetaData> MetaDatas { get; set; }



		// NOT SUBMITTING BELOW HERE:
		public string EntryName { get; set; }

	}

	public class MetaData
	{
		public string Key { get; set; }
		public string Value { get; set; }
	}
}
