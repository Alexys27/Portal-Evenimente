using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventsPortal.Controllers
{
    public class EventServiceController : Controller
    {
        // GET: EventServiceController
        public ActionResult Index()
        {
            return View();
        }

        // GET: EventServiceController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EventServiceController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventServiceController/Create
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

        // GET: EventServiceController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EventServiceController/Edit/5
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

        // GET: EventServiceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EventServiceController/Delete/5
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
