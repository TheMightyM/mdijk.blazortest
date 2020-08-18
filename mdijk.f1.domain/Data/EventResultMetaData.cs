using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace mdijk.f1.domain.Data
{
	public class EventResultMetaData
	{
		public int EventResultMetaDataId { get; set; }
		public int EventResultId { get; set; }
		public string Key { get; set; }
		public string Value { get; set;}

		[ForeignKey("EventResultId")]
		public EventResult EventResult { get; set; }
	}
}
