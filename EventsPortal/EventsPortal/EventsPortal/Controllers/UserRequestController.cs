using EventsPortal.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventsPortal.Controllers
{
    public class UserRequestController : Controller
    {
        private Repository.UserRequestRepository _repository;
        public UserRequestController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.UserRequestRepository(dbContext);
        }
        // GET: UserRequestsController
        public ActionResult Index()
        {
            var requests = _repository.GetAllUserRequests();
            return View("Index", requests);
        }

        // GET: UserRequestsController/Details/5
        public ActionResult Details(Guid id)
        {
            var request = _repository.GetUserRequestByID(id);
            return View("RequestDetails", request);
        }

        // GET: UserRequestsController/Create
        public ActionResult Create()
        {
            return View("CreateUserRequest");
        }

        // POST: UserRequestsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Models.UserRequestModel model = new Models.UserRequestModel();
                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    _repository.InsertUserRequest(model);
                }
                return View("CreateUserRequest");
            }
            catch
            {
                return View("CreateUserRequest");
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: UserRequestsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserRequestsController/Edit/5
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

        // GET: UserRequestsController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var model = _repository.GetUserRequestByID(id);
            return View("DeleteUserRequest", model);
        }

        // POST: UserRequestsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _repository.DeleteUserRequest(id);
                return RedirectToAction("Index", id);

            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return View("DeleteAdditionalService", id);
            }
        }
    }
}
