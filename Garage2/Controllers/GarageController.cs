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
using Garage2.Models;
using System.Data.Entity.Infrastructure;

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
			var filters = (filter ?? "").Trim().Split();
			for (int i = 0; i < filters.Length; i++)
			{
				filter = filters[i];
				if (filter != "")
				{
					result = result.Where(v => (
						v.RegNr.Contains(filter) ||
						v.Type.ToString().Contains(filter) ||
						v.Brand.Contains(filter) ||
						v.Model.Contains(filter) ||
						v.Color.Contains(filter)));
				}
			}
			var sortdir = sort.Split('_');
			result = result.OrderBy(sortdir[0], sortdir.Length == 1);

			if (Request.IsAjaxRequest())
			{
				return PartialView("_Vehicles", result);
			}
			return View(result);
		}

		public ActionResult Details(string id)
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
			return View(vehicle);
		}

		public ActionResult Park()
		{
			Vehicle vehicleWithDefaults = new Vehicle();
			return View(vehicleWithDefaults);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Park([Bind(Include = "RegNr,Type,Brand,Model,Color,Wheels")] Vehicle vehicle)
		{
			try
			{
				if (ModelState.IsValid)
				{
					// Set parking time
					vehicle.EntryTime = DateTime.Now;
					db.Vehicles.Add(vehicle);
					db.SaveChanges();
					return RedirectToAction("Index");
				}
			}
			catch (DataException /* dex */)
			{
				//Log the error (uncomment dex variable name and add a line here to write a log.
				//ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
				ModelState.AddModelError("", "Kan inte spara ändringar. Försök igen och om problemet kvarstår kontakta din systemadministratör.");
			}
			return View(vehicle);
		}

		public ActionResult Edit(string id)
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
			return View(vehicle);
		}

		[HttpPost, ActionName("Edit")]
		[ValidateAntiForgeryToken]
		public ActionResult EditPost(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var vehicleToUpdate = db.Vehicles.Find(id);
			if (TryUpdateModel(vehicleToUpdate, "", new string[] { "RegNr", "Type", "Brand", "Model", "Wheels" }))
			{
				try
				{
					db.SaveChanges();

					return RedirectToAction("Index");
				}
				catch (RetryLimitExceededException /* dex */)
				{
					//Log the error (uncomment dex variable name and add a line here to write a log.
					//ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
					ModelState.AddModelError("", "Kan inte spara ändringar. Försök igen och om problemet kvarstår kontakta din systemadministratör.");
				}
			}
			return View(vehicleToUpdate);
		}

		public ActionResult Checkout(string id, bool? saveChangesError = false)
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
			return View(vehicle);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Checkout(string id)
		{
			try
			{
				Vehicle vehicle = db.Vehicles.Find(id);
				db.Vehicles.Remove(vehicle);
				db.SaveChanges();
			}
			catch (RetryLimitExceededException/* dex */)
			{
				//Log the error (uncomment dex variable name and add a line here to write a log.
				return RedirectToAction("Delete", new { id = id, saveChangesError = true });
			}
			return RedirectToAction("Index");
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
