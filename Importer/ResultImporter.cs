using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.IO;
using Importer.xml;
using mdijk.blazoring.Shared.Models;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Net.Http.Json;
using mdijk.blazoring.Shared.Response;
using System.Linq;
using mdijk.blazoring.Shared.Submit;
using System.Data;

namespace Importer
{
	public class ResultImporter
	{
		//http://ergast.com/api/f1/{year}/{sequence}/results

		public async Task ImportResults()
		{
			// dit is uitbereiden
			Console.WriteLine("Getting seasons");
			//var seasons = await Program.HTTPCLIENT.GetFromJsonAsync<Season[]>("Season/getall");
			var seasons =
				new List<Season>
				{
				new Season { SeasonId = 1, SeasonYear = 2020}
				};

			Console.WriteLine("Getting all drivers");
			var drivers = await Program.HTTPCLIENT.GetFromJsonAsync<IList<Driver>>("driver/getall");
			Console.WriteLine("Getting all teams");
			var teams = await Program.HTTPCLIENT.GetFromJsonAsync<IList<Team>>("team/getall");
			Console.WriteLine("Getting all circuits");
			var circuits = await Program.HTTPCLIENT.GetFromJsonAsync<IList<Circuit>>("circuit/getall");

			foreach (var season in seasons)
			{
				
				var driverEntryStorage = new List<TempStorage>();

				Console.WriteLine($"Getting the events for the {season.SeasonYear} season");
				IList<RaceEvent> raceEvents = await Program.HTTPCLIENT.GetFromJsonAsync<IList<RaceEvent>>($"Season/GetSeasonCalendar/{season.SeasonId}/{season.SeasonYear}");

				foreach (var rE in raceEvents)
				{
					if (rE.SequenceNumber == 5) break;
					if (season.SeasonYear == 1950 && rE.SequenceNumber == 1) continue;
					var eRurl = $"http://ergast.com/api/f1/{season.SeasonYear}/{rE.SequenceNumber}/results";

					Console.WriteLine($"Request race {rE.SequenceNumber} from {season.SeasonYear}");
					var request = WebRequest.CreateHttp(eRurl);
					var response = request.GetResponse();
					XmlSerializer sr = new XmlSerializer(typeof(MRDataType));
					var data = (MRDataType)sr.Deserialize(response.GetResponseStream());

					var race = data.RaceTable.Race[0];

					var circuitType = race.Item as CircuitType;

					// dit is het model dat gesubmit gaat worden!
					var resultModel = new AddEventResultModel
					{
						EventId = rE.RaceEventId,
						Rows = new List<AddEventResultModelRow>()
					};

					var resultsList = race.ResultsList;
					for (int i = 0; i < resultsList.Length; i++)
					{
						var result = resultsList[i];

						var rTeam = result.Item as ConstructorType;
						// split the team name
						if(rTeam.Name.Contains("-"))
						{
							rTeam.Name = rTeam.Name.Split(new string[] { "-" }, StringSplitOptions.None)[0];
						}
						var rDriver = result.Items.First() as DriverType;

						// position is string in geval. kan ook R zijn
						var position = i + 1;
						// teamId en driverId erbij zoeken
						Driver driver = null;
						try
						{
							driver = drivers.First(x => x.FirstName == rDriver.GivenName && x.LastName == rDriver.FamilyName);
						}
						catch(Exception ex)
						{
							Console.WriteLine($"problem finding driver: {rDriver.GivenName} {rDriver.FamilyName}");
							throw ex;
						}
						var team = teams.First(x => x.TeamName == rTeam.Name);


						// kijk of we deze combo al in onze storage hebben
						var driverEntry = driverEntryStorage.FirstOrDefault(x => x.DriverId == driver.DriverId
						&& x.TeamId == team.TeamId
						// super zinloos, maar we hebben eigenlijk alleen constructors.
						// is ok voor nu
						&& x.EntryName == team.TeamName);


						if (driverEntry == null)
						{
							Console.WriteLine($"registring new Entry for the {season.SeasonYear} season: {rDriver.GivenName} {rDriver.FamilyName} driving for {team.TeamName}");
							var nwDriverEntryId = await Register(season.SeasonId, team.TeamId, driver.DriverId, team.TeamName);
							driverEntry = new TempStorage
							{
								DriverEntryId = nwDriverEntryId,
								DriverId = driver.DriverId,
								EntryName = team.TeamName,
								TeamId = team.TeamId
							};
							driverEntryStorage.Add(driverEntry);
						}

						// woopwoop!
						var metaData = new List<MetaData>();
						metaData.Add(new MetaData { Key = "Position", Value = result.position });
						metaData.Add(new MetaData { Key = "Grid", Value = result.Grid });
						metaData.Add(new MetaData { Key = "Laps", Value = result.Laps });
						if (result.pointsSpecified)
						{
							metaData.Add(new MetaData { Key = "Points", Value = $"{result.points}" });
						}
						else
						{
							metaData.Add(new MetaData { Key = "Points", Value = "0" });
						}
						if (result.Status != null) metaData.Add(new MetaData { Key = "Status", Value = result.Status.Value });
						if (result.Time != null) metaData.Add(new MetaData { Key = "Time", Value = result.Time.Value });
						if (result.FastestLap != null && result.FastestLap.Time != null) metaData.Add(new MetaData { Key = "FastestLap", Value = result.FastestLap.Time.Value });
						if (result.FastestLap != null) metaData.Add(new MetaData { Key = "FastestLap", Value = result.FastestLap.rank });


						resultModel.Rows.Add(new AddEventResultModelRow
						{
							DriverEntryId = $"{driverEntry.DriverEntryId}",
							//FastestLap  = result.FastestLap != null && result.FastestLap.rank == "1",
							FinishingPosition = position,
							MetaDatas = metaData
						});
					}

					Console.WriteLine($"Opslaan van race {rE.SequenceNumber} van seizoen {season.SeasonYear}");
					var postresult =  await Program.HTTPCLIENT.PostAsJsonAsync<AddEventResultModel>("eventresult/AddCompleteResult", resultModel);
					var uitkomst = await postresult.Content.ReadFromJsonAsync<AddItemResponse>();
					if(uitkomst.IsError)
					{
						Console.WriteLine(uitkomst.Message);
					}
					// omdat wij aan het testen zijn wil ik alleen één race proberen.
				}
			}




			return;
		}

		private async Task<int> Register(int seasonId, int teamId, int driverId, string entryName)
		{
			var jsonResponse = await Program.HTTPCLIENT.PostAsJsonAsync("Season/register", new RegisterModel
			{
				DriverId = driverId,
				EntryName = entryName,
				SeasonId = seasonId,
				TeamId = teamId
			});

			var aj = await jsonResponse.Content.ReadFromJsonAsync<AddItemResponse>();
			return aj.NewItemId;
		}


		private class TempStorage
		{
			public string EntryName { get; set; }
			public int TeamId { get; set; }
			public int DriverId { get; set; }
			public int DriverEntryId { get; set; }
		}
	}
}


