using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Garage2;
using Garage2.Extensions;
using Garage2.DataLayer;
using System.Data.Entity.Infrastructure;
using Garage2.Models.VehicleViewModels;

namespace Garage2.Controllers
{
	public class GarageController : Controller
	{
		private VehiclesDB db = new VehiclesDB();

		public ActionResult Index(string filter, string sort = "")
		{
			ViewBag.Filter = filter;
			ViewBag.SortParam = sort;
			IQueryable<Vehicle> result = db.Vehicles;
			string[] filters = (filter ?? "").Trim().Split();
			for (int i = 0; i < filters.Length; i++)
			{
				string tmpfilter = filters[i];
				if (tmpfilter != "")
				{
					result = result.Where(v => (
						v.RegNr.Contains(tmpfilter) ||
						v.Type.ToString().Contains(tmpfilter) ||
						v.Brand.Contains(tmpfilter) ||
						v.Model.Contains(tmpfilter) ||
						v.Color.Contains(tmpfilter)));
				}
			}
			string[] sortdir = sort.Split('_');
			result = result.OrderBy(sortdir[0], sortdir.Length == 1);

			if (Request.IsAjaxRequest())
			{
				return PartialView("_Vehicles", result);
			}
			return View(result);
		}

		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Vehicle vehicle = db.Vehicles.Find(id);
			if (vehicle == null)
			{
				return HttpNotFound();
			}
			return PartialView("_Details", vehicle);
		}

		public ActionResult Park()
		{
			EditVehicleViewModel vehicleWithDefaults = new EditVehicleViewModel();
			return PartialView("_Park", vehicleWithDefaults);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Park(EditVehicleViewModel evvm)
		{
			try
			{
				if (ModelState.IsValid)
				{
					Vehicle newVehicle = new Vehicle();
					if (TryUpdateModel(newVehicle, "", null, new string[] { "Id", "EntryTime" }))
					{
						// Set parking time
						newVehicle.EntryTime = DateTime.Now;
						db.Vehicles.Add(newVehicle);
						db.SaveChanges();
						TempData["alert"] = "success|Fordonet är parkerat!";
					}
					else
					{
						TempData["alert"] = "danger|Kunde inte lägga till fordonet!";
					}
				}
			}
			catch (DataException /* dex */)
			{
				//Log the error (uncomment dex variable name and add a line here to write a log.
				ModelState.AddModelError("", "Kan inte spara ändringar. Försök igen och om problemet kvarstår kontakta din systemadministratör.");
			}
			return PartialView("_Park", evvm);
		}

		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Vehicle vehicle = db.Vehicles.Find(id);
			if (vehicle == null)
			{
				return HttpNotFound();
			}
			EditVehicleViewModel evvm = vehicle;
			HttpContext.Session["vehicleid"] = id;
			return PartialView("_Edit", evvm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit()
		{
			int? id = (int?)HttpContext.Session["vehicleid"];
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var vehicleToUpdate = db.Vehicles.Find(id);
			if (id != null && TryUpdateModel(vehicleToUpdate, "", null, new string[] { "Id", "EntryTime" }))
			//if (TryUpdateModel(vehicleToUpdate, "", new string[] { "RegNr", "Type", "Brand", "Model", "Color", "Wheels" }))
			{
				try
				{
					db.SaveChanges();
					TempData["alert"] = "success|Fordonet är uppdaterat!";
				}
				catch (RetryLimitExceededException /* dex */)
				{
					//Log the error (uncomment dex variable name and add a line here to write a log.
					ModelState.AddModelError("", "Kan inte spara ändringar. Försök igen och om problemet kvarstår kontakta din systemadministratör.");
				}
			}
			else
			{
				TempData["alert"] = "danger|Kunde inte uppdatera fordonet!";
			}
			return PartialView("_Edit", (EditVehicleViewModel)vehicleToUpdate);
		}

		public ActionResult Checkout(int? id, bool? saveChangesError = false)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			if (saveChangesError.GetValueOrDefault())
			{
				//ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
				ViewBag.ErrorMessage = "Hämtning gick inte att genomföra. Försök igen och om problemet kvarstår kontakta din systemadministratör.";
			}
			Vehicle vehicle = db.Vehicles.Find(id);
			if (vehicle == null)
			{
				return HttpNotFound();
			}
			return PartialView("_Checkout", vehicle);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Receipt(int? id)
		{
			ReceiptViewModel receipt;
			try
			{
				Vehicle vehicle = db.Vehicles.Find(id);
				if (vehicle == null)
				{
					return HttpNotFound();
				}
				receipt = new ReceiptViewModel()
				{
					Type = vehicle.Type,
					RegNr = vehicle.RegNr,
					Brand = vehicle.Brand,
					Model = vehicle.Model,
					Color = vehicle.Color,
					Wheels = vehicle.Wheels,
					EntryTime = vehicle.EntryTime,
					CheckoutTime = DateTime.Now,
					ParkDuration = vehicle.ParkDuration,
					ParkCost = vehicle.ParkCost
				};
				db.Vehicles.Remove(vehicle);
				db.SaveChanges();
			}
			catch (RetryLimitExceededException/* dex */)
			{
				//Log the error (uncomment dex variable name and add a line here to write a log.
				return RedirectToAction("Delete", new { id = id, saveChangesError = true });
			}
			return View(receipt);
		}

		public ActionResult Statistics()
		{
			StatisticsViewModal model;
			try
			{
				var VehicleList = db.Vehicles.ToList();
				model = new StatisticsViewModal()
				{
					TotalCount = VehicleList.Count(),
					TotalWheels = VehicleList.Sum(v => v.Wheels),
					TotalCost = String.Format("{0:C}", VehicleList.Sum(v => v.ParkSpan.TotalSeconds) / 3600d * 60d),
					TypeCountList = VehicleList
						.GroupBy(v => v.Type)
						.Select(item => new TypeCountViewModel
						{
							Type = item.Key,
							Count = item.Count(),
							Wheels = item.Sum(v => v.Wheels),
							Cost = String.Format("{0:C}", item.Sum(v => v.ParkSpan.TotalSeconds) / 3600d * 60d)
						})
				};
			}
			catch (Exception)
			{
				return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
			}
			return View(model);
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
