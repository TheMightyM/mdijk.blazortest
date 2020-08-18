using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace mdijk.f1.domain.Data
{
	public class DriverEntry
	{
		public int DriverEntryId { get; set; }
		public int DriverId { get; set; }
		public int EntryId { get; set; }
		[ForeignKey("DriverId")]
		public Driver Driver { get; set; }

		[ForeignKey("EntryId")]
		public Entry Entry { get; set; }

		public ICollection<EventResult> EventResults { get; set; }
	}
}
