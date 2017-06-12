using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Garage2
{
	//	public enum Vehicletypes { Airplane, Boat, Bus, Car, Motorcycle, Truck }
	public enum Vehicletypes { Bil, Buss, Båt, Flygplan, Lastbil, Motorcykel }

	public class Vehicle
	{
		public int Id { get; set; }
		[Required]
		[Display(Name = "Fordonstyp")]
		public Vehicletypes Type { get; set; }
		[Required]
		[StringLength(8, ErrorMessage = "{0} måste vara mellan 5 och 8 tecken långt.", MinimumLength = 5)]
		[RegularExpression("^[A-Za-z0-9]*$", ErrorMessage = "Endast tecknen A till Z och 0 till är tillåtna.")]
		[Display(Name = "Registreringsnr")]
		public string RegNr { get; set; }
		[StringLength(40)]
		[Display(Name = "Märke")]
		public string Brand { get; set; }
		[StringLength(40)]
		[Display(Name = "Modell")]
		public string Model { get; set; }
		[StringLength(40)]
		[Display(Name = "Färg")]
		public string Color { get; set; }
		[Range(0, 100)]
		[Display(Name = "Hjulantal")]
		public int Wheels { get; set; }
		[Display(Name = "Parkerat vid")]
		public DateTime EntryTime { get; set; }
		[NotMapped]
		public TimeSpan ParkSpan {
			get { return DateTime.Now - EntryTime; }
		}
		[NotMapped]
		public string ParkDuration {
			get { return FormattedTimeSpan(ParkSpan); }
		}
		[NotMapped]
		public string ParkCost {
			get { return String.Format("{0:C}", ParkSpan.TotalSeconds / 3600d * 60d); }
		}

		public Vehicle()
		{
		}

		static private string FormattedTimeSpan(TimeSpan span)
		{
			string result = "";

			if (span.Days > 0)
			{
				result += span.Days.ToString() + "dag, ";
			}
			if (span.Hours > 0)
			{
				result += span.Hours.ToString() + "tim, ";
			}
			result += span.Minutes.ToString() + "min";
			return result;
		}
	}

	public class ReceiptViewModel
	{
		[Display(Name = "Fordonstyp")]
		public Vehicletypes Type { get; set; }
		[Display(Name = "Registreringsnr")]
		public string RegNr { get; set; }
		[Display(Name = "Märke")]
		public string Brand { get; set; }
		[Display(Name = "Modell")]
		public string Model { get; set; }
		[Display(Name = "Färg")]
		public string Color { get; set; }
		[Display(Name = "Hjulantal")]
		public int Wheels { get; set; }
		[Display(Name = "Parkerat vid")]
		public DateTime EntryTime { get; set; }
		[Display(Name = "Hämtat vid")]
		public DateTime CheckoutTime { get; set; }
		[Display(Name = "Parkeringstid")]
		public string ParkDuration { get; set; }
		[Display(Name = "Park.kostnad")]
		public string ParkCost { get; set; }
	}

	public class TypeCount
	{
		[Display(Name = "Fordonstyp")]
		public Vehicletypes Type { get; set; }
		[Display(Name = "Antal")]
		public int Count { get; set; }
		[Display(Name = "Summa hjul")]
		public int Wheels { get; set; }
		[Display(Name = "Summa kostnad")]
		public string Cost { get; set; }
	}

	public class StatisticsViewModal
	{
		[Display(Name = "Totalt")]
		public int TotalCount { get; set; }
		public int TotalWheels { get; set; }
		public string TotalCost { get; set; }
		public IEnumerable<TypeCount> TypeCountList { get; set; }
	}
}
