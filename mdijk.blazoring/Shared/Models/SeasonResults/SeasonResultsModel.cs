using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared.Models.SeasonResults
{
	public class SeasonResultsModel
	{
		public int SeasonId { get; set; }
		public int SeasonYear { get; set; }
		public ICollection<SeasonRaceEvent> RaceEvents { get; set; }
	}

	public class SeasonResultsModelRow
	{
		public Driver Driver { get; set; }
		public string TeamEntryName { get; set; }
		public Team Team { get; set; }

		/// <summary>
		/// Zelf zorgen voor dejuiste volgorde!
		/// </summary>		
		public ICollection<EventResult> Results { get; set; }
	}
}
