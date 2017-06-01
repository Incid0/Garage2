using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Garage2
{
	public enum Vehicletypes { Airplane, Boat, Bus, Car, Motorcycle }

	public class Vehicle
	{
		public Vehicletypes Type { get; set; }
		[Key]
		[Required]
		[StringLength(6, MinimumLength = 6)]
		public string RegNr { get; set; }
		public string Brand { get; set; }
		public string Model { get; set; }
		public string Color { get; set; }
		public int Wheels { get; set; }
		public DateTime EntryTime { get; set; }

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
