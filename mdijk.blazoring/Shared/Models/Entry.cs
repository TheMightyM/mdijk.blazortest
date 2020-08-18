using System.Collections.Generic;

namespace mdijk.blazoring.Shared.Models
{
	public class Entry
	{
		public int EntryId { get; set; }
		public string FullEntryName { get; set; }
		public Team Team {get;set;}
		public List<DriverEntry> Drivers { get; set; }
		public Engine Engine { get; set; }
	}
}
