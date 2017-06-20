using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage2.Models.VehicleViewModels
{
	public class EditVehicleViewModel
	{
		public int Id { get; set; }
		[Required]
		[Display(Name = "Fordonstyp")]
		public VehicletypesEnum Type { get; set; }
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

		public EditVehicleViewModel()
		{
		}

		public static implicit operator EditVehicleViewModel(Vehicle model)
		{
			return new EditVehicleViewModel
			{
				Id = model.Id,
				Type = model.Type,
				RegNr = model.RegNr,
				Brand = model.Brand,
				Model = model.Model,
				Color = model.Color,
				Wheels = model.Wheels,
				EntryTime = model.EntryTime
			};
		}
	}
}