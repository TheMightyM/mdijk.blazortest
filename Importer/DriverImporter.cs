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
	public class DriverImporter
	{
		//http://ergast.com/api/f1/drivers?offset=30&limit=30

		public async Task ImportDrivers()
		{
			await Task.FromResult(true);

			int offset = 0;
			int limit = 30;

			bool keepGoing = true;

			while(keepGoing)
			{
				var driverGetsUrl = $"http://ergast.com/api/f1/drivers?offset={offset}&limit={limit}";

				var request = WebRequest.CreateHttp(driverGetsUrl);
				var response = request.GetResponse();
				XmlSerializer sr = new XmlSerializer(typeof(MRDataType));
				var data = (MRDataType)sr.Deserialize(response.GetResponseStream());

				Console.WriteLine($"Offset {offset}");
				if (data.DriverTable.Driver.Length == 0) break;


				foreach(var driver in data.DriverTable.Driver)
				{
					if (!string.IsNullOrEmpty(driver.PermanentNumber)) continue;
					Console.WriteLine($"Adding {driver.GivenName} {driver.FamilyName}");
					var res =await Program.HTTPCLIENT.PostAsJsonAsync<Driver>("Driver/adddriver", new Driver { 
					Country = driver.Nationality,
					DriverNumber = string.IsNullOrEmpty(driver.PermanentNumber) ? -1 : Convert.ToInt32(driver.PermanentNumber),
					FirstName = driver.GivenName,
					LastName = driver.FamilyName
					});

					var respo = await res.Content.ReadFromJsonAsync<AddItemResponse>();
					if(respo.IsError)
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
