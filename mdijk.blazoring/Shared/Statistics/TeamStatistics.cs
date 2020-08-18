using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared.Statistics
{
	public class TeamStatistics
	{
		public int TeamId { get; set; }
		public string TeamName { get; set; }
		public int NumberOfRaces { get; set; }
		public float TotalPoints { get; set; }
		public int NumberOfWins { get; set; }
		public int NumberOfPodiums { get; set; }
		public int NumberOfPoles { get; set; }
	}
}
