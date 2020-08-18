using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared
{
	public static class Points
	{
		public static readonly Dictionary<int, int> PointsForPosition = new Dictionary<int, int>
		{
			{ 1, 25 },
			{ 2, 18 },
			{ 3, 15 },
			{ 4, 12 },
			{ 5, 10 },
			{ 6, 8 },
			{ 7, 6 },
			{ 8, 4 },
			{ 9, 2 },
			{ 10, 1 },
		};

		public static readonly int FastedLap = 1;

		// ja, want dit is zoveel leesbaarder dan gewone functies! Wauw, wat een verbetering!!!!!!
		public static bool IsFastestLap(int finishingPosition) => finishingPosition <= 10;
	}
}
