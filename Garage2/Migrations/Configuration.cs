namespace Garage2.Migrations
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<Garage2.Models.VehiclesDB>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
			ContextKey = "Garage2.Models.VehiclesDB";
		}

		protected override void Seed(Garage2.Models.VehiclesDB context)
		{
			context.Vehicles.AddOrUpdate(
				v => v.RegNr,
				new Vehicle { RegNr = "ABC123", Type = Vehicletypes.Bil, Brand = "Ford", Model = "T-Ford", Color = "Pink", Wheels = 4, EntryTime = DateTime.Now },
				new Vehicle { RegNr = "XXX666", Type = Vehicletypes.Flygplan, Brand = "Boing", Model = "747", Color = "White", Wheels = 12, EntryTime = DateTime.Now },
				new Vehicle { RegNr = "ZOO123", Type = Vehicletypes.Motorcykel, Brand = "Yamaha", Model = "CR80", Color = "Red", Wheels = 2, EntryTime = DateTime.Now },
				new Vehicle { RegNr = "TXT000", Type = Vehicletypes.Båt, Brand = "Otter", Model = "T500", Color = "Orange", Wheels = 0, EntryTime = DateTime.Now },
				new Vehicle { RegNr = "ZOO567", Type = Vehicletypes.Bil, Brand = "Volvo", Model = "V70", Color = "Silver", Wheels = 4, EntryTime = DateTime.Now },
				new Vehicle { RegNr = "GBG421", Type = Vehicletypes.Lastbil, Brand = "Mach", Model = "X20", Color = "Orange", Wheels = 4, EntryTime = DateTime.Now }
			);
		}
	}
}
