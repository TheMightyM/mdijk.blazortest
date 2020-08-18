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

namespace Importer
{
	public class SeasonImporter
	{
		public async Task ImportSeasons()
		{
			Console.WriteLine("al deze zooi zit er al in!");
			return;
			var url = "http://ergast.com/api/f1/seasons?limit=90";

			var request = WebRequest.CreateHttp(url);
			var response = request.GetResponse();
			XmlSerializer sr = new XmlSerializer(typeof(MRDataType));
			var data = (MRDataType)sr.Deserialize(response.GetResponseStream());
			
			foreach(var season in data.SeasonTable.Season)
			{
				if (season.Value == "2020") continue;
				Console.WriteLine($"Importing season? ${season.Value}");
				//	var postRequest = WebRequest.Create("http://localhost:57256/Season/addseason");

				var jsonResponse= await Program.HTTPCLIENT.PostAsJsonAsync("Season/addseason", new Season
				{
					SeasonYear = Convert.ToInt32(season.Value)
				});

				var aj = await jsonResponse.Content.ReadFromJsonAsync<AddItemResponse>();
				if(aj.IsError)
				{
					Console.WriteLine($"ging niet goed! {aj.Message}");
				}
				else
				{
					Console.WriteLine($"{aj.NewItemId}"	);
				}
			}
		}

		
	}
}
