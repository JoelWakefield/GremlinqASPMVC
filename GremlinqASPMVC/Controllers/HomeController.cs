using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Samples.Shared;
using GremlinqASPMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GremlinqASPMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEnumerable<string> Vertices = new string[] { "All", "Person", "Pet", "Software" };
        private readonly IEnumerable<string> Edges = new string[] { "All", "Knows", "Owns", "Created" };

        private readonly ILogger<HomeController> _logger;
        private readonly IGremlinQuerySource source;

        public HomeController(ILogger<HomeController> logger, IGremlinQuerySource source)
        {
            _logger = logger;
            this.source = source;
        }

        public IActionResult Index()
        {
            ViewData["Parents"] = new SelectList(Vertices);
            ViewData["Relationships"] = new SelectList(Edges);
            ViewData["Children"] = new SelectList(Vertices);

            return View();
        }

        public async Task<JsonResult> GetPeople()
        {
            return Json(await source
                .V<Person>()
                .ToArrayAsync());
        }

        public async Task<JsonResult> GetPets()
        {
            return Json(await source
                .V<Pet>()
                .ToArrayAsync());
        }

        public async Task<JsonResult> GetSoftwares()
        {
            return Json(await source
                .V<Software>()
                .ToArrayAsync());
        }

        public async Task<JsonResult> GetKnows()
        {
            return Json(await source
                .E<Knows>()
                .ToArrayAsync());
        }

        public async Task<JsonResult> GetOwns()
        {
            return Json(await source
                .E<Owns>()
                .ToArrayAsync());
        }

        public async Task<JsonResult> GetCreated()
        {
            return Json(await source
                .E<Created>()
                .ToArrayAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
