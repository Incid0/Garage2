using System;
using System.ComponentModel.DataAnnotations;

namespace Garage2
{
	public class ReceiptViewModel
	{
		[Display(Name = "Fordonstyp")]
		public VehicletypesEnum Type { get; set; }
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
}
