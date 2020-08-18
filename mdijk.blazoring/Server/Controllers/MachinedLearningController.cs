using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mdijk.blazoring.Shared.ML;
using mdijk.blazoring.Shared.Models;
using mdijk.f1.domainservices.QueryServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using MLMARKML.Model;
using Namotion.Reflection;

namespace mdijk.blazoring.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class MachinedLearningController : ControllerBase
	{
		private readonly ILogger<MachinedLearningController> logger;
		private readonly ISeasonQueryService _seasonQueryService;
		private readonly IRaceEventQueryService _raceEventQueryService;
		private readonly ILaptimeQueryService _laptimeQueryService;
		private readonly ICircuitQueryService _circuitQueryService;
		public MachinedLearningController(ILogger<MachinedLearningController> logger, ISeasonQueryService seasonQueryService,
		IRaceEventQueryService raceEventQueryService,
		ICircuitQueryService circuitQueryService,
		ILaptimeQueryService laptimeQueryService)	
		{
			this.logger = logger;
			_seasonQueryService = seasonQueryService;
			_raceEventQueryService = raceEventQueryService;
			_circuitQueryService = circuitQueryService;
			_laptimeQueryService = laptimeQueryService;
		}

		[HttpPost("PredictLaptime")]
		public MLResponse PredictLaptime([FromBody] MLRequest request)
		{
			try
			{
				var modelInput = new ModelInput
				{
					CircuitId = Convert.ToSingle(request.CircuitId),
					//CurrentPosition = Convert.ToSingle(request.CurrentPosition),
					DriverId = Convert.ToSingle(request.DriverId),
					LapNumber = Convert.ToSingle(request.LapNumber),
					SeasonYear = Convert.ToSingle(request.SeasonYear)
				};

				var response = ConsumeModel.Predict(modelInput);

				return new MLResponse
				{
					PredictedMilliseconds = Convert.ToInt32(response.Score)
				};
			}
			catch(Exception ex)
			{
				throw ex;
			}
		
		}

		[HttpPost("PredictFullRace")]
		public async Task<MLDriverCircuitResponse> PredictFullRace([FromBody] MLDriverCircuitRequest request)
		{
			var raceEvent = await _raceEventQueryService.GetRaceEventAndSeasonById(request.RaceEventId);

			var maxLaps = 0;
			if (request.TotalLaps != 0)
			{
				maxLaps = request.TotalLaps;
			}
			else
			{
				maxLaps = await _raceEventQueryService.GetMaxsLapsForRaceEvent(request.RaceEventId);
			}
			
			var response = new MLDriverCircuitResponse
			{
				RaceLaps = maxLaps
			};

			var predictions = new List<MLDriverPrediction>();

			foreach (var driverId in request.DriverIds)
			{
				var driverPrediction = new MLDriverPrediction
				{
					DriverId = driverId,
					Laptimes = new List<MLDriverPredictionLap>()
				};

				var actualLaptimes = await _laptimeQueryService.GetAllLaptimesForEventAndDriver(request.RaceEventId, driverId);
				driverPrediction.ActualLaptimes = actualLaptimes.Select(x => new MLDriverPredictionLap { 
				LapNumber = x.LapNumber,
				PredictedLapTime = x.Milliseconds
				}).ToList();

				for (int lapNumber = 1; lapNumber <= maxLaps; lapNumber++)
				{
					var modelInput = new ModelInput
					{
						CircuitId = Convert.ToSingle(raceEvent.CircuitId),
						DriverId = Convert.ToSingle(driverId),
						LapNumber = Convert.ToSingle(lapNumber),
						SeasonYear = Convert.ToSingle(raceEvent.Season.SeasonYear)
					};

					driverPrediction.Laptimes.Add(new MLDriverPredictionLap
					{
						LapNumber = lapNumber,
						PredictedLapTime = Convert.ToInt32(ConsumeModel.Predict(modelInput).Score)
					});

				}

				predictions.Add(driverPrediction);
			}

			response.Predictions = predictions.
				OrderBy(x => x.Laptimes.ToList().Sum(lt => lt.PredictedLapTime))
				.ToList();

			return response;
		}
	}
}
