using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared.Models
{
	public class DriverLaptimes
	{
		public int DriverId { get; set; }
		public Driver Driver { get; set; }
		public List<Laptime> Laptimes { get; set; }
	}
}
