using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Garage2;
using Garage2.Models;
using System.Data.Entity.Infrastructure;
using System.Reflection;

namespace Garage2.Controllers
{
	public class GarageController : Controller
	{
		private VehiclesDB db = new VehiclesDB();

		public class MyComparer : IComparer<Object>
		{
			public int Compare(object stringA, object stringB)
			{
				return stringA.ToString().CompareTo(stringB.ToString());
			}
		}
		
		private bool FilterFields(string text, params object[] fields)
		{
			return true;
		}

		// GET: Garage
		public ActionResult Index(string filter, string sort)
		{
			bool Ascend = true;
			ViewBag.SortParam = sort;
			var result = db.Vehicles
				.Where(v => (filter == null || v.RegNr.Contains(filter)));
			//if((sort.IndexOf("_desc") > 0) {
			//	sort = 
			//}
			//switch (sort) {
			//	case "time":
			//		result = result.OrderBy(v => v.EntryTime);
			//	break;
			//	case: "type":
			//		result = result.OrderBy
			//}
			Func<Vehicle, object> func = v => v.Type;

			MyComparer comparer = new MyComparer();
			var result3 = result.OrderBy(func, comparer);
			//result = result.Or
			var result2 = result3.ToList();

			if (Request.IsAjaxRequest())
			{
				return PartialView("_Vehicles", result2);
			}
			return View(result2);
		}

		// GET: Garage/Details/5
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
