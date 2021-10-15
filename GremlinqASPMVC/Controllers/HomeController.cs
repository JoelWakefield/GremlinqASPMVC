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
        private readonly GremlinqHelper gremlinq;
        private readonly IGremlinQuerySource source;

        public HomeController(
            ILogger<HomeController> logger, 
            GremlinqHelper gremlinq,
            IGremlinQuerySource source)
        {
            _logger = logger;
            this.gremlinq = gremlinq;
            this.source = source;
        }

        public IActionResult Index()
        {
            ViewData["Parents"] = new SelectList(Vertices);
            ViewData["Relationships"] = new SelectList(Edges);
            ViewData["Children"] = new SelectList(Vertices);

            return View();
        }

        [HttpGet]
        [Route("Home/Knows")]
        public async Task<JsonResult> Knows()
        {
            return Json(await gremlinq.People("n:n"));
        }

        [HttpGet]
        [Route("Home/Knows/{id}/{relationshipType}")]
        public async Task<JsonResult> Knows(string id, string relationshipType)
        {
            return Json(await gremlinq.People(relationshipType, id));
        }


        [HttpGet]
        [Route("Home/Owns")]
        public async Task<JsonResult> Owns()
        {
            return Json(await gremlinq.Pet("n:n"));
        }

        [HttpGet]
        [Route("Home/Owns/{id}/{relationshipType}")]
        public async Task<JsonResult> Owns(string id, string relationshipType)
        {
            return Json(await gremlinq.Pet(relationshipType,id));
        }

        [HttpGet]
        [Route("Home/Created")]
        public async Task<JsonResult> Created()
        {
            return Json(await gremlinq.Software("n:n"));
        }

        [HttpGet]
        [Route("Home/Created/{id}/{relationshipType}")]
        public async Task<JsonResult> Created(string id, string relationshipType)
        {
            return Json(await gremlinq.Software(relationshipType, id));
        }


        public async Task TextStream()
        {
            Response.ContentType = "text/plain";

            var writer = new StreamWriter(Response.Body, Encoding.UTF8)
            {
                AutoFlush = true
            };

            await new Logic(source, writer)
                .Run();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
