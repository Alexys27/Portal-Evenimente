using EventsPortal.Data;
using EventsPortal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration;

namespace EventsPortal.Controllers
{
    public class PackageController : Controller
    {
        private Repository.PackageRepository _repository;
        public PackageController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.PackageRepository(dbContext);
        }
        // GET: PackagesController
        public ActionResult Index()
        {
            var packages = _repository.GetAllPackages();
            return View("Index", packages);
        }

        // GET: PackagesController/Details/5
        public ActionResult Details(Guid id)
        {
            var package = _repository.GetPackageByID(id);
            return View("PackageDetails", package);
        }

        // GET: PackagesController/Create
        public ActionResult Create()
        {
            return View("CreatePackage");
        }

        // POST: PackagesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Models.PackageModel model = new Models.PackageModel();
                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    _repository.InsertPackage(model);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View("CreatePackage");
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: PackagesController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var model = _repository.GetPackageByID(id);
            return View("EditPackage", model);
        }

        // POST: PackagesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var model = new PackageModel();
                var task = TryUpdateModelAsync(model); 
                task.Wait();
                if (task.Result)
                {
                    _repository.UpdatePackage(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", id);
                }
            }
            catch
            {
                return RedirectToAction("Index", id);
            }
        }

        // GET: PackagesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PackagesController/Delete/5
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
