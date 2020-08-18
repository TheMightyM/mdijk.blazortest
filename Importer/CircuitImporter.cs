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
	public class CircuitImporter
	{
		//http://ergast.com/api/f1/circuits?limit=74
		public async Task ImportCircuit()
		{
			await Task.FromResult(true);
			Console.WriteLine("al deze zooi zit er al in!");
			return;
			var url = "http://ergast.com/api/f1/circuits?limit=74";

			var request = WebRequest.CreateHttp(url);
			var response = request.GetResponse();
			XmlSerializer sr = new XmlSerializer(typeof(MRDataType));
			var data = (MRDataType)sr.Deserialize(response.GetResponseStream());

			foreach (var circuit in data.CircuitTable.Circuit)
			{
				Console.WriteLine($"Importing circuit? {circuit.CircuitName}");
				//	var postRequest = WebRequest.Create("http://localhost:57256/Season/addseason");

				var jsonResponse = await Program.HTTPCLIENT.PostAsJsonAsync("circuit/addcircuit", new Circuit
				{
					CircuitName = circuit.CircuitName,
					Country = circuit.Location.Country
				});

				var aj = await jsonResponse.Content.ReadFromJsonAsync<AddItemResponse>();
				if (aj.IsError)
				{
					Console.WriteLine($"ging niet goed! {aj.Message}");
				}
				else
				{
					Console.WriteLine($"{aj.NewItemId}");
				}
			}

			return;
		}
	}
}
