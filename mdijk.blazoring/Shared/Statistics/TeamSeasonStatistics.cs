using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared.Statistics
{
	public class TeamSeasonStatistics
	{
		public int TeamId { get; set; }

		public ICollection<TeamSeason> Seasons { get; set; }
	}

	public class TeamSeason
	{ 
		public int RacesInSeason { get; set; }
		public int SeasonId { get; set; }
		public int SeasonYear { get; set;}

		public ICollection<int> DriversIds { get; set; }

		public ICollection<TeamSeasonEventResult> EventResults { get; set; }
	}

	public class TeamSeasonEventResult
	{
		public int EventSequenceNr { get; set; }
		public int EventId { get; set; }
		public string EventName { get; set; }

		/// <summary>
		/// key: driverid
		/// value result voor dit event duh
		/// </summary>
		public ICollection<TeamSeasonEventDriverResult> DriverResults { get; set; }
	}

	public class TeamSeasonEventDriverResult
	{
		public int DriverId { get; set; }
		public int FinishingPosition { get; set; }
		public int StartingPosition { get; set; }
		public bool Started { get; set; }
		public bool Finished { get; set; }
	}
}
