using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared.Submit
{
	public class RegisterModel
	{
		public int SeasonId { get; set; }
		public int TeamId { get; set; }
		public int DriverId { get; set; }
		public string EntryName { get; set; }
	}
}
