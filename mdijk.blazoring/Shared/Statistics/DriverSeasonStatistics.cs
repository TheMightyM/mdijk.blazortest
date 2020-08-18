using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared.Statistics
{
	public class DriverSeasonStatistics
	{
		public int DriverId { get; set; }
		public ICollection<DriverSeason> Seasons { get; set; }
	}

	public class DriverSeason
	{
		public int SeasonYear { get; set; }
		public int SeasonId { get; set; }
		/// <summary>
		/// het totaal aantal races in het seizeon, ongeacht of deze persoon daar aan mee gedaan heeft
		/// </summary>
		public int EventsInSeason { get; set; }

		public ICollection<DriverResult> Results { get; set; }
	}

	public class DriverResult
	{
		public int EventSequenceNr { get; set; }
		public int EventId { get; set; }
		public string EventName { get; set; }
		public int FinishingPosition { get; set; }
		public int StartingPosition { get; set; }
		public bool Started { get; set;}
		public bool Finished { get; set; }
	}
}
