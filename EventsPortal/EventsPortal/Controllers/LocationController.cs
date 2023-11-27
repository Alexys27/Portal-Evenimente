using EventsPortal.Data;
using EventsPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventsPortal.Controllers
{
    public class LocationController : Controller
    {
        private Repository.LocationRepository _repository;
        public LocationController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.LocationRepository(dbContext);
        }
        // GET: LocationController
        [AllowAnonymous]
        public ActionResult Index()
        {
            var locations = _repository.GetAllLocations();
            return View("Index", locations);
        }

        // GET: LocationController/Details/5
        [AllowAnonymous]
        public ActionResult Details(Guid id)
        {
            var location = _repository.GetLocationByID(id);
            return View("LocationDetails", location);
        }

        // GET: LocationController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View("CreateLocation");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(LocationModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HandleImageUpload(model);

                    // Insert the location
                    _repository.InsertLocation(model);

                    return RedirectToAction(nameof(Index));
                }

                return View("CreateLocation", model);
            }
            catch (Exception ex)
            {
                // Log the exception
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return View("CreateLocation", model);
            }
        }

        // GET: LocationController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Guid id)
        {
            var model = _repository.GetLocationByID(id);
            return View("EditLocation", model);
        }

        // POST: LocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Guid id, LocationModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Handle image upload only if a new file is provided
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        // Save the file to a folder
                        var imagePath = "/uploads/" + model.ImageFile.FileName;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", model.ImageFile.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            model.ImageFile.CopyTo(stream);
                        }

                        // Set the new image path in the model
                        model.ImagePath = imagePath;
                    }

                    // Update the location
                    _repository.UpdateLocation(model);

                    return RedirectToAction(nameof(Index));
                }

                // Log or inspect the errors
                var errors = ModelState.Values.SelectMany(v => v.Errors);

                return View("EditLocation", model);
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("EditLocation", model);
            }
        }

        // Helper method to handle image upload
        private void HandleImageUpload(LocationModel model)
        {
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                // Save the file to a folder
                var imagePath = "/uploads/" + model.ImageFile.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", model.ImageFile.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageFile.CopyTo(stream);
                }

                // Set the image path in the model
                model.ImagePath = imagePath;
            }
        }
        // GET: LocationController/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
