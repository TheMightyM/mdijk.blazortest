using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared.Submit
{
	public class AddEntryModel
	{
		public int EntryId { get; set; }
		public string FullEntryName { get; set; }
		public string TeamId { get; set; }
		public string FirstDriverId { get; set; }
		public string SecondDriverId { get; set; }
		public string EngineId { get; set; }
		public int SeasonId { get; set; }
	}
}
