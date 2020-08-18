﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace mdijk.f1.domain.Data
{
	public class Pitstop
	{
		public int PitstopId { get; set; }
		public int RaceEventId { get; set; }
		public int DriverEntryId { get; set; }
		public int LapNumber { get; set; }
		public int Milliseconds { get; set; }

		[ForeignKey("RaceEventId")]
		public RaceEvent RaceEvent { get; set; }
		[ForeignKey("DriverEntryId")]
		public DriverEntry DriverEntry { get; set; }
	}
}
