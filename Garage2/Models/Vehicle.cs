using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Garage2
{
//	public enum Vehicletypes { Airplane, Boat, Bus, Car, Motorcycle, Truck }
	public enum Vehicletypes { Bil, Buss, Båt, Flygplan, Lastbil , Motorcykel }

	public class Vehicle
	{
		[Display(Name ="Fordonstyp")]
		public Vehicletypes Type { get; set; }
		[Key]
		[Required]
		[StringLength(6, MinimumLength = 6)]
		[Display(Name = "Registreringsnr")]
		public string RegNr { get; set; }
		[Display(Name = "Märke")]
		public string Brand { get; set; }
		[Display(Name = "Modell")]
		public string Model { get; set; }
		[Display(Name = "Färg")]
		public string Color { get; set; }
		[Range(0, 100)]
		[Display(Name = "Hjulantal")]
		public int Wheels { get; set; }
		[Display(Name = "Parkerat vid")]
		public DateTime EntryTime { get; set; }

		public Vehicle()
		{
			//Wheels = 0;
		}

		public virtual bool Match(string pattern)
		{
			return RegNr.IndexOf(pattern, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
					Brand.IndexOf(pattern, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
					Model.IndexOf(pattern, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
					Color.IndexOf(pattern, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
					Wheels.ToString().IndexOf(pattern) >= 0;
		}
	}
}
