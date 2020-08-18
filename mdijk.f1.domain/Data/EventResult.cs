using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace mdijk.f1.domain.Data
{
	public class EventResult
	{
		public int EventResultId { get; set; }
		public int RaceEventId { get; set; }
		public int DriverEntryId { get; set; }
		public int FinishingPosition { get; set; }
		public bool FastestLap { get; set; }

		[ForeignKey("RaceEventId")]
		public RaceEvent RaceEvent { get; set; }
		[ForeignKey("DriverEntryId")]
		public DriverEntry DriverEntry { get; set; }


		public IList<EventResultMetaData> MetaData { get; set; }
	}
}
