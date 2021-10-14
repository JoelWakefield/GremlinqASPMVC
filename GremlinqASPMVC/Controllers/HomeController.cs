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

        [HttpGet]
        [Route("Home/Person")]
        public async Task<JsonResult> Person()
        {
            var items = await source
                .V<Person>()
                .As((__, person) => __
                    .OutE<Knows>()
                    .InV<Person>()
                    .As((__, people) => __
                        .Select(person, people)));

            if (items != null)
                return Json(Repackage(items));
            else
                return null;
        }

        [HttpGet]
        [Route("Home/Person/{id}")]
        public async Task<JsonResult> Person(string id)
        {
            var items = await source
                .V<Person>(id)
                .As((__, person) => __
                    .OutE<Knows>()
                    .InV<Person>()
                    .As((__, people) => __
                        .Select(person, people)));

            if (items != null)
                return Json(Repackage(items));
            else
                return null;
        }

        [HttpGet]
        [Route("Home/Pet")]
        public async Task<JsonResult> Pet()
        {
            var items = await source
                .V<Pet>()
                .As((__, pet) => __
                    .InE<Owns>()
                    .OutV<Person>()
                    .As((__, person) => __
                        .Select(person, pet)));

            if (items != null)
                return Json(Repackage(items));
            else
                return null;
        }
        
        [HttpGet]
        [Route("Home/Pet/{id}")]
        public async Task<JsonResult> Pet(string id)
        {
            var items = await source
                .V<Pet>(id)
                .As((__, pet) => __
                    .InE<Owns>()
                    .OutV<Person>()
                    .As((__, person) => __
                        .Select(person, pet)));

            if (items != null)
                return Json(Repackage(items));
            else
                return null;
        }

        [HttpGet]
        [Route("Home/Software")]
        public async Task<JsonResult> Software()
        {
            var items = await source
                .V<Software>()
                .As((__, software) => __
                    .InE<Created>()
                    .OutV<Person>()
                    .As((__, person) => __
                        .Select(person, software)));

            if (items != null)
                return Json(Repackage(items));
            else
                return null;
        }

        [HttpGet]
        [Route("Home/Software/{id}")]
        public async Task<JsonResult> Software(string id)
        {
            var items = await source
                .V<Software>(id)
                .As((__, software) => __
                    .InE<Created>()
                    .OutV<Person>()
                    .As((__, person) => __
                        .Select(person, software)));

            if (items != null)
                return Json(Repackage(items));
            else
                return null;
        }

        [HttpGet]
        [Route("Home/Knows")]
        public async Task<JsonResult> Knows()
        {
            var items = await source
                .V<Person>()
                .As((__, person) => __
                    .OutE<Knows>()
                    .InV<Person>()
                    .As((__, people) => __
                        .Select(person, people)));

            if (items != null)
                return Json(Repackage(items));
            else
                return null;
        }

        [HttpGet]
        [Route("Home/Knows/{id}/{relationshipType}")]
        public async Task<JsonResult> Knows(string id, string relationshipType)
        {
            dynamic items;

            switch (relationshipType)
            {
                case "1:n":
                    items = await source
                        .V<Person>(id)
                        .As((__, person) => __
                            .OutE<Knows>()
                            .InV<Person>()
                            .As((__, people) => __
                                .Select(person, people)));
                    break;
                case "n:1":
                    items = await source
                        .V<Person>(id)
                        .As((__, people) => __
                            .InE<Knows>()
                            .OutV<Person>()
                            .As((__, person) => __
                                .Select(person, people)));
                    break;
                default:
                    return null;
            }

            if (items != null)
                return Json(Repackage(items));
            else
                return null;
        }


        [HttpGet]
        [Route("Home/Owns")]
        public async Task<JsonResult> Owns()
        {
            var items = await source
                .V<Person>()
                .As((__, person) => __
                    .OutE<Owns>()
                    .InV<Pet>()
                    .As((__, pet) => __
                        .Select(person, pet)));

            if (items != null)
                return Json(Repackage(items));
            else
                return null;
        }

        [HttpGet]
        [Route("Home/Owns/{id}/{relationshipType}")]
        public async Task<JsonResult> Owns(string id, string relationshipType)
        {
            dynamic items;

            switch (relationshipType)
            {
                case "1:n":
                    items = await source
                        .V<Person>(id)
                        .As((__, person) => __
                            .OutE<Owns>()
                            .InV<Pet>()
                            .As((__, pet) => __
                                .Select(person, pet)));
                    break;
                case "n:1":
                    items = await source
                        .V<Pet>(id)
                        .As((__, pet) => __
                            .InE<Owns>()
                            .OutV<Person>()
                            .As((__, person) => __
                                .Select(person, pet)));
                    break;
                default:
                    return null;
            }

            if (items != null)
                return Json(Repackage(items));
            else
                return null;
        }

        [HttpGet]
        [Route("Home/Created")]
        public async Task<JsonResult> Created()
        {
            var items = await source
                .V<Person>()
                .As((__, person) => __
                    .OutE<Created>()
                    .InV<Software>()
                    .As((__, software) => __
                        .Select(person, software)));

            if (items != null)
                return Json(Repackage(items));
            else
                return null;
        }

        [HttpGet]
        [Route("Home/Created/{id}/{relationshipType}")]
        public async Task<JsonResult> Created(string id, string relationshipType)
        {
            dynamic items;

            Type t = typeof(Person);

            switch (relationshipType)
            {
                case "1:n":
                    items = await source
                        .V<Person>(id)
                        .As((__, person) => __
                            .OutE<Created>()
                            .InV<Software>()
                            .As((__, software) => __
                                .Select(person, software)));
                    break;
                case "n:1":
                    items = await source
                        .V<Software>(id)
                        .As((__, software) => __
                            .InE<Created>()
                            .OutV<Person>()
                            .As((__, person) => __
                                .Select(person, software)));
                    break;
                default:
                    return null;
            }

            if (items != null)
                return Json(Repackage(items));
            else
                return null;
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


        private List<dynamic> Repackage((Person, Person)[] items)
        {
            List<dynamic> list = new List<dynamic>();

            foreach (var item in items)
            {
                var (h, t) = item;
                list.Add(new dynamic[] { h, t });
            }

            return list;
        }

        private List<dynamic> Repackage((Person, Pet)[] items)
        {
            List<dynamic> list = new List<dynamic>();

            foreach (var item in items)
            {
                var (h, t) = item;
                list.Add(new dynamic[] { h, t });
            }

            return list;
        }

        private List<dynamic> Repackage((Person, Software)[] items)
        {
            List<dynamic> list = new List<dynamic>();

            foreach (var item in items)
            {
                var (h, t) = item;
                list.Add(new dynamic[] { h, t });
            }

            return list;
        }
    }
}
