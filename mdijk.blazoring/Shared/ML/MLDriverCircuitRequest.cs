using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared.ML
{
	public class MLDriverCircuitRequest
	{
		public int RaceEventId { get; set; }
		public int TotalLaps { get; set; }

		public ICollection<int> DriverIds { get; set; }
	}
}
