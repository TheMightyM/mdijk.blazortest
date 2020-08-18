using mdijk.blazoring.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace mdijk.blazoring.Shared.Response
{
	public class AddItemResponse : ResponseModel
	{
		public int NewItemId { get; set; }

		public static async Task<AddItemResponse> CreateErrorAsync(string message)
		{
			return await Task.FromResult(new AddItemResponse
			{ 
				IsError = true,
				Message = message
			});
		}

		public static AddItemResponse CreateError(string message)
		{
			return new AddItemResponse
			{
				IsError = true,
				Message = message
			};
		}
	}
}
