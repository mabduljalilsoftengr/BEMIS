using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspStudio.Models;

namespace AspStudio.Controllers;

public class PagesController : Controller
{
		public IActionResult ErrorPage()
		{
				return View();
		}
}
