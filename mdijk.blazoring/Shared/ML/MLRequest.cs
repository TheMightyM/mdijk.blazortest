using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared.ML
{
	public class MLRequest
	{
		public int SeasonYear { get; set; }
		public int CircuitId { get; set; }
		public int DriverId { get; set;}
		public int LapNumber { get; set; }
		public int CurrentPosition { get; set; }
	}
}
