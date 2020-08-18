using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace mdijk.f1.domain.Data
{
	public class Entry
	{
		public int EntryId { get; set; }
		public string FullEntryName { get; set; }

		public int TeamId { get; set; }
	
		public int? EngineId { get; set; }

		public int SeasonId { get; set; }

		[ForeignKey("TeamId")]
		public Team Team {get; set;}

		[ForeignKey("EngineId")]
		public Engine Engine { get; set; }

		[ForeignKey("SeasonId")]
		public Season Season { get; set; }
		public List<DriverEntry> Drivers { get; set; }

	}

	public enum EntryDriverPosition
	{
		FirstDriver,
		SecondDriver
	}
}
