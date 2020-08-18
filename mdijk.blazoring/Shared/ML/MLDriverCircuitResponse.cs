using System;
using System.Collections.Generic;
using System.Text;

namespace mdijk.blazoring.Shared.ML
{
	public class MLDriverCircuitResponse
	{
		public int RaceLaps { get; set; }
		public ICollection<MLDriverPrediction> Predictions { get; set; }
	}

	public class MLDriverPrediction
	{ 
		public int DriverId { get; set; }
		public ICollection<MLDriverPredictionLap> Laptimes { get; set; }
		public ICollection<MLDriverPredictionLap> ActualLaptimes { get; set; }

	}

	public class MLDriverPredictionLap
	{
		public int LapNumber { get; set; }
		public int PredictedLapTime { get; set; }
	}
}
