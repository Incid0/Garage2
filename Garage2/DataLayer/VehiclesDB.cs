using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Garage2.DataLayer
{
	class VehiclesDB : DbContext
	{
		public VehiclesDB() : base("name=Garage")
		{
		}

		public DbSet<Vehicle> Vehicles { get; set; }
	}
}