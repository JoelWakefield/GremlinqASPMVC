using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Samples.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GremlinqASPMVC.Controllers
{
    public class SoftwareController : Controller
    {
        private readonly IGremlinQuerySource source;

        public SoftwareController(IGremlinQuerySource source)
        {
            this.source = source;
        }

        // GET: SoftwareController
        public ActionResult Index()
        {
            return View(source.V<Software>().ToArrayAsync().Result);
        }

        // GET: SoftwareController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SoftwareController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SoftwareController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SoftwareController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SoftwareController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SoftwareController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SoftwareController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
