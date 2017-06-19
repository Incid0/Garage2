using System.ComponentModel.DataAnnotations;

namespace Garage2
{
	public class TypeCountViewModel
	{
		[Display(Name = "Fordonstyp")]
		public VehicletypesEnum Type { get; set; }
		[Display(Name = "Antal")]
		public int Count { get; set; }
		[Display(Name = "Summa hjul")]
		public int Wheels { get; set; }
		[Display(Name = "Summa kostnad")]
		public string Cost { get; set; }
	}
}
