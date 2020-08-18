using mdijk.blazoring.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared
{
	public class DriverFormatter
	{
		public static string Format(Driver driver, Guid guid)
		{
			Console.WriteLine($"Formatten voor {guid}");
			return Format(driver);
		}
		public static string Format(Driver driver)
		{
			if (driver == null) return string.Empty;
			if(driver.DriverNumber < 0)
			{
				return $"{driver.FirstName} {driver.LastName}";
			}
			return $"{driver.FirstName} {driver.LastName} ({driver.DriverNumber})";
		}

		public static string Format(string firstName, string lastName, int driverNumber)
		{
			if (driverNumber < 0)
			{
				return $"{firstName} {lastName}";
			}
			return $"{firstName} {lastName} ({driverNumber})";
		}
	}
}
