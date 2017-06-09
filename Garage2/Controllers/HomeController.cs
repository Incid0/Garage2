using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garage2.Models;

namespace Garage2.Controllers
{
	public class HomeController : Controller
	{
		private VehiclesDB db = new VehiclesDB();

		public ActionResult Index()
		{
			var model = new HomeViewModel();
			var vehicles = db.Vehicles;
			model.Current = vehicles.Count();
			model.Free = model.Max - model.Current;
			if (model.Percent < 50)
				model.ColorLevel = "success";
			else if (model.Percent < 75)
				model.ColorLevel = "warning";
			else
				model.ColorLevel = "danger";
			return View(model);
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}