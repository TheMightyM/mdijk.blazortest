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

namespace Importer
{
	public class EventImporter
	{
		//http://ergast.com/api/f1/2012

		public async Task ImportEvents()
		{
			// eerst alle de circuits ophalen van onze eigen API ding vet
			// daarna alle seizoenen uit ons api ding 

			// daarna voor elk siezoen hallen we alle events op uit die mooie api van die lui

			// daarna voegen wij events aan seizoenen toe enzo
			Console.WriteLine("getting all circuit...");
			var allcircuits = await Program.HTTPCLIENT.GetFromJsonAsync<IList<Circuit>>("circuit/getall");
			Dictionary<string, int> circuitDict = new Dictionary<string, int>();
			foreach(var circuit in allcircuits)
			{
				circuitDict.Add(circuit.CircuitName, circuit.CircuitId);
			}

			Console.WriteLine("getting all seasons...");
			//var allSeasons = await Program.HTTPCLIENT.GetFromJsonAsync<IList<Season>>("season/getall");
			var allSeasons =
				new List<Season>
				{
				new Season { SeasonId = 1, SeasonYear = 2020}
				};


			Console.WriteLine("getting events from external API:");

			foreach(var season in allSeasons)
			{
				//if (season.SeasonYear == 2020) continue;
				var url = $"http://ergast.com/api/f1/{season.SeasonYear}";

				var request = WebRequest.CreateHttp(url);
				var response = request.GetResponse();
				XmlSerializer sr = new XmlSerializer(typeof(MRDataType));
				var data = (MRDataType)sr.Deserialize(response.GetResponseStream());

				foreach(var race in data.RaceTable.Race)
				{
					var crt = race.Item as CircuitType;
					Console.WriteLine($"Adding the {season.SeasonYear} {race.RaceName}");
					AddRaceEventModel arem = new AddRaceEventModel
					{
						CircuitId = circuitDict[crt.CircuitName].ToString(),
						RaceEventName = race.RaceName,
						SeasonId = season.SeasonId,
						SequenceNr = race.round
					};

					await Program.HTTPCLIENT.PostAsJsonAsync<AddRaceEventModel>("season/addraceevent", arem);
				}	
			}
		}
	}
}
