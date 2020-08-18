using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared.Statistics
{
	public class DriverStatistics
	{
		public int DriverId { get; set; }
		public string DriverName { get; set; }

		public int NumberOfRaces { get; set; }
		public float CareerPoints { get; set; }

		public int NumberOfWins { get; set; }
		public int NumberOfPodiums { get; set; }
		public int NumberOfPoles { get; set; }

		public int NumberOfDNF { get; set; }
		public int NumberOfDNQ { get; set; }
	}
}
