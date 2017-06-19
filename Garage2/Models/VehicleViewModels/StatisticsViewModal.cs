using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Garage2
{
	public class StatisticsViewModal
	{
		[Display(Name = "Totalt")]
		public int TotalCount { get; set; }
		public int TotalWheels { get; set; }
		public string TotalCost { get; set; }
		public IEnumerable<TypeCountViewModel> TypeCountList { get; set; }
	}
}
