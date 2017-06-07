using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage2
{
	public class HomeViewModel
	{
		public int Current { get; set; }
		public int Max { get; set; }
		public int Percent {
			get { return (int)Current * 100 / Max; }
		}
		public string ColorLevel { get; set; }

		public HomeViewModel()
		{
			Max = 15;
		}
	}
}