using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared.Models
{
	public class DriverEntry
	{
		public int DriverEntryId { get; set; }
		public Driver Driver { get; set; }
		public string EntryFullEntryName { get; set; }
		public Team EntryTeam { get; set; }

		public Engine EntryEngine { get; set; }
	}
}
