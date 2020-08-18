using mdijk.blazoring.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared
{
	public class DriverColourCreator
	{ 	
		public static string CreateRGBHexFromName(Driver driver)
		{
			var result = CreateHexFromName($"{driver.FirstName} {driver.LastName}");

			return $"#{result.Item1:X2}{result.Item2:X2}{result.Item3:X2}";
		}

		public static Tuple<byte, byte, byte> CreateHexFromName(string driverName)
		{
			var length = (double)driverName.Length;
			var pieces = Convert.ToInt32(Math.Round(length / 3));

			var parts = new[]
			{
				driverName.Substring(0, pieces),
				driverName.Substring(pieces, pieces),
				driverName.Substring(pieces + pieces)
			};

			return new Tuple<byte, byte, byte>(CreateByte(parts[0]),
				CreateByte(parts[1]),
				CreateByte(parts[2]));
		}

		private static byte CreateByte(string str)
		{
			byte bytes = 0;
			foreach (var m in str)
			{
				bytes += Convert.ToByte(m);
			}
			
			while(bytes > 255)
			{
				bytes -= 255;
			}

			return bytes;

		}
	}
}
