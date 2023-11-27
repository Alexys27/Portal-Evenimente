using EventsPortal.Data;
using EventsPortal.Models;
using EventsPortal.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Core.Types;
using System.Linq.Expressions;

namespace EventsPortal.Controllers
{
    public class EventController : Controller
    {
        private Repository.EventRepository _repository;
        private Repository.LocationRepository _locationRepository;
        private Repository.PackageRepository _packageRepository;
        private Repository.AdditionalServiceRepository _additionalServicesRepository;
        private Repository.EventPackageRepository _eventPackageRepository;
        private Repository.EventServiceRepository _eventServiceRepository;
        public EventController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.EventRepository(dbContext);
            _locationRepository = new Repository.LocationRepository(dbContext);
            _packageRepository = new Repository.PackageRepository(dbContext);
            _additionalServicesRepository = new Repository.AdditionalServiceRepository(dbContext);
            _eventPackageRepository = new Repository.EventPackageRepository(dbContext);
            _eventServiceRepository = new Repository.EventServiceRepository(dbContext);

        }
        // GET: EventsController
        public ActionResult Index(string searchString)
        {
            var events = string.IsNullOrWhiteSpace(searchString)
                ? _repository.GetAllEvents()
                : _repository.GetEventsBySearch(searchString);

            ViewBag.SearchString = searchString;

            return View("Index", events);
        }

        // GET: EventsController/Details/5
        public ActionResult Details(Guid id)
        {
            var model = _repository.GetEventsByID(id);
            return View("EventDetails", model);
        }

        // GET: EventsController/Create
        public ActionResult Create()
        {
            try
            {
                var locations = _locationRepository.GetAllLocations();
                ViewBag.Locations = new SelectList(locations, "LocationID", "LocationName");

                // Retrieve packages and additional services
                ViewBag.Packages = new SelectList(_packageRepository.GetAllPackages(), "PackageID", "PackageName");
                ViewBag.AdditionalServices = _additionalServicesRepository.GetAllAdditionalServices();

                return View("CreateEvent");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return View("CreateEvent");
            }
        }


        // POST: EventsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Insert records into the appropriate tables
                    _repository.InsertEvent(model);

                    var evPackages = new EventPackageModel
                    {
                        EventID = model.EventID,
                        PackageID = model.SelectedPackageID
                    };
                    _eventPackageRepository.InsertEventPackage(evPackages);

                    foreach (var selectedServiceID in model.SelectedAdditionalServices)
                    {
                        var evAddServices = new EventServiceModel
                        {
                            EventID = model.EventID,
                            ServiceID = selectedServiceID
                        };
                        _eventServiceRepository.InsertEventService(evAddServices);
                    }

                    var locations = _locationRepository.GetAllLocations();
                    ViewBag.Locations = new SelectList(locations, "LocationID", "LocationName");

                    // Retrieve packages and additional services
                    ViewBag.Packages = new SelectList(_packageRepository.GetAllPackages(), "PackageID", "PackageName");
                    ViewBag.AdditionalServices = _additionalServicesRepository.GetAllAdditionalServices();

                    return RedirectToAction("Index", "Home");
                }

                // If ModelState is not valid, reload the locations and return to the view
                ViewBag.Locations = new SelectList(_locationRepository.GetAllLocations(), "LocationID", "LocationName");
                ViewBag.Packages = new SelectList(_packageRepository.GetAllPackages(), "PackageID", "PackageName");
                ViewBag.AdditionalServices = _additionalServicesRepository.GetAllAdditionalServices();

                return View("CreateEvent");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request.");

                ViewBag.Locations = new SelectList(_locationRepository.GetAllLocations(), "LocationID", "LocationName");
                ViewBag.Packages = new SelectList(_packageRepository.GetAllPackages(), "PackageID", "PackageName");
                ViewBag.AdditionalServices = _additionalServicesRepository.GetAllAdditionalServices();

                return View("CreateEvent");
            }
        }

        // GET: EventsController/Edit/5
        public ActionResult Edit(Guid id)
        {
            try
            {
                var locations = _locationRepository.GetAllLocations();
                ViewBag.Locations = new SelectList(locations, "LocationID", "LocationName");

                // Get the main event details
                var model = _repository.GetEventsByID(id);

                // Retrieve packages and additional services
                ViewBag.Packages = new SelectList(_packageRepository.GetAllPackages(), "PackageID", "PackageName");
                ViewBag.AdditionalServices = _additionalServicesRepository.GetAllAdditionalServices();

                // Retrieve associated packages for the event
                var selectedPackageID = _eventPackageRepository.GetAllEventPackages()
                    .Where(p => p.EventID == id)
                    .Select(p => p.PackageID)
                    .FirstOrDefault();

                // Retrieve associated services for the event
                var selectedServiceIDs = _eventServiceRepository.GetAllEventServices()
                    .Where(s => s.EventID == id)
                    .Select(s => s.ServiceID)
                    .ToList();

                // Assign selected package to the model
                model.SelectedPackageID = selectedPackageID;

                // Assign selected additional services to the model
                model.SelectedAdditionalServices = selectedServiceIDs;

                return View("EditEvent", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return View("EditEvent");
            }
        }


        // POST: EventsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, EventModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Update records in the appropriate tables
                    _repository.UpdateEvents(model);

                    // Update EventPackage records
                    var evPackages = new EventPackageModel
                    {
                        EventID = model.EventID,
                        PackageID = model.SelectedPackageID
                    };
                    _eventPackageRepository.UpdateEventPackage(evPackages);

                    // Update EventService records
                    var existingEventServices = _eventServiceRepository.GetAllEventServices().Where(es => es.EventID == model.EventID).ToList();
                    foreach (var existingService in existingEventServices)
                    {
                        if (model.SelectedAdditionalServices == null || !model.SelectedAdditionalServices.Contains(existingService.ServiceID))
                        {
                            // If the service is not selected, delete it
                            _eventServiceRepository.DeleteEventService(existingService);
                        }
                    }

                    // Insert new EventServices
                    if (model.SelectedAdditionalServices != null)
                    {
                        foreach (var selectedServiceID in model.SelectedAdditionalServices)
                        {
                            if (!existingEventServices.Any(es => es.ServiceID == selectedServiceID))
                            {
                                var evAddServices = new EventServiceModel
                                {
                                    EventID = model.EventID,
                                    ServiceID = selectedServiceID
                                };
                                _eventServiceRepository.InsertEventService(evAddServices);
                            }
                        }
                    }

                    var locations = _locationRepository.GetAllLocations();
                    ViewBag.Locations = new SelectList(locations, "LocationID", "LocationName");

                    // Retrieve packages and additional services
                    ViewBag.Packages = new SelectList(_packageRepository.GetAllPackages(), "PackageID", "PackageName");
                    ViewBag.AdditionalServices = _additionalServicesRepository.GetAllAdditionalServices();

                    return RedirectToAction("Index");
                }

                // If ModelState is not valid, reload the locations and return to the view
                ViewBag.Locations = new SelectList(_locationRepository.GetAllLocations(), "LocationID", "LocationName");
                ViewBag.Packages = new SelectList(_packageRepository.GetAllPackages(), "PackageID", "PackageName");
                ViewBag.AdditionalServices = _additionalServicesRepository.GetAllAdditionalServices();

                return View("EditEvent", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request.");

                ViewBag.Locations = new SelectList(_locationRepository.GetAllLocations(), "LocationID", "LocationName");
                ViewBag.Packages = new SelectList(_packageRepository.GetAllPackages(), "PackageID", "PackageName");
                ViewBag.AdditionalServices = _additionalServicesRepository.GetAllAdditionalServices();

                return View("EditEvent", model);
            }
        }

        // GET: EventsController/Delete/5
        public ActionResult Delete(Guid id)
        {
            try
            {
                // Get the main event details
                var model = _repository.GetEventsByID(id);
                return View("DeleteEvent", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return RedirectToAction("Index");
            }
        }

        // POST: EventsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _repository.DeleteEvents(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return RedirectToAction("Delete", new { id = id });
            }
        }
    }
}