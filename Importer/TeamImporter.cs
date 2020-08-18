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
	public class TeamImporter
	{
		//http://ergast.com/api/f1/constructors?offset={offset}&limit={limit}"

		public async Task ImportTeams()
		{
			await Task.FromResult(true);
			Console.WriteLine("deze hebben we al gedaan");
			return;

			int offset = 0;
			int limit = 30;

			bool keepGoing = true;

			while (keepGoing)
			{
				var driverGetsUrl = $"http://ergast.com/api/f1/constructors?offset={offset}&limit={limit}";

				var request = WebRequest.CreateHttp(driverGetsUrl);
				var response = request.GetResponse();
				XmlSerializer sr = new XmlSerializer(typeof(MRDataType));
				var data = (MRDataType)sr.Deserialize(response.GetResponseStream());

				Console.WriteLine($"Offset {offset}");
				if (data.ConstructorTable.Constructor.Length == 0) break;


				foreach (var team in data.ConstructorTable.Constructor)
				{
					var teamName = team.Name;
					if(teamName.Contains("-"))
					{
						teamName = teamName.Split(new string[] { "-"}, StringSplitOptions.None)[0];
					}
					Console.WriteLine($"Adding {team.Name}");
					var res = await Program.HTTPCLIENT.PostAsJsonAsync<Team>("Team/addTeam", new Team
					{
						Country = team.Nationality,
						TeamName = teamName
					});

					var respo = await res.Content.ReadFromJsonAsync<AddItemResponse>();
					if (respo.IsError)
					{
						Console.WriteLine(respo.Message);
					}
					else
					{
						Console.WriteLine(respo.NewItemId);
					}
				}


				offset += limit;
				if (offset > Convert.ToInt32(data.total)) break;
			}


			return;
		}

	}
}