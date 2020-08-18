using System.Collections;
using System.Collections.Generic;

namespace mdijk.f1.domain.Data
{
	public class Driver
	{
		public int DriverId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Country { get; set; }
		public int DriverNumber { get; set; }

		public ICollection<DriverEntry> DriverEntries { get; set; }
	}
}
