using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace mdijk.f1.domain.Data
{
	public class RaceEventMetaData
	{
		public int RaceEventMetaDataId { get; set; }
		public int RaceEventId { get; set; }

		public string Key { get; set; }
		public string Value { get; set; }


		[ForeignKey("RaceEventId")]
		public RaceEvent RaceEvent { get; set; }

	}
}
