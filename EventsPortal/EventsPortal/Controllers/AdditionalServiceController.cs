using EventsPortal.Data;
using EventsPortal.Models;
using EventsPortal.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventsPortal.Controllers
{
    public class AdditionalServiceController : Controller
    {
        private Repository.AdditionalServiceRepository _repository;
        public AdditionalServiceController (ApplicationDbContext dbContext)
        {
            _repository = new Repository.AdditionalServiceRepository (dbContext);
        }
        // GET: AdditionalServicesController
        public ActionResult Index()
        {
            var services = _repository.GetAllAdditionalServices ();
            return View("Index", services);
        }

        // GET: AdditionalServicesController/Details/5
        public ActionResult Details(Guid id)
        {
            var model = _repository.GetAdditionalServiceByID (id);
            return View("AdditionalServiceDetails", model);
        }

        // GET: AdditionalServicesController/Create
        public ActionResult Create()
        {
            return View("CreateAdditionalService");
        }

        // POST: AdditionalServicesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Models.AdditionalServiceModel model = new Models.AdditionalServiceModel();
                var task = TryUpdateModelAsync(model);
                task.Wait();
                if(task.Result)
                {
                    _repository.InsertAdditionalService(model);
                }
                return View("CreateAdditionalService");
            }
            catch
            {
                return View("CreateAdditionalService");
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: AdditionalServicesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdditionalServicesController/Edit/5
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

        // GET: AdditionalService/Delete/5
        public ActionResult Delete(Guid id)
        {
            try
            {
                // Get the additional service details
                var model = _repository.GetAdditionalServiceByID(id);
                return View("DeleteAdditionalService", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return RedirectToAction("Index", id);
            }
        }

        // POST: AdditionalServicesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _repository.DeleteAdditionalService(id);
                return RedirectToAction("Index", id);  
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return View("DeleteAdditionalService", id);
            }
        }

    }
}
