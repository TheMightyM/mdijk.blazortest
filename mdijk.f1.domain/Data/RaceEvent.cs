using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace mdijk.f1.domain.Data
{
	public class RaceEvent
	{
		public int RaceEventId { get; set; }
		public int SequenceNumber { get; set; }

		public string RaceEventName { get; set; }
		public int CircuitId { get; set; }

		[ForeignKey("CircuitId")]

		public Circuit Circuit { get; set; }

		public int SeasonId { get; set; }

		[ForeignKey("SeasonId")]
		public Season Season { get; set; }

		public virtual ICollection<EventResult> Results { get; set; }
		public virtual ICollection<RaceEventMetaData> MetaDatas { get; set; }

		public virtual ICollection<Laptime> Laptimes { get; set; }
		public virtual ICollection<Pitstop> Pitstops { get; set; }
	}
}
