using InWorkWebApp.Custom.Handlers;
using InWorkWebApp.Models;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace InWorkWebApp.Controllers
{
    [Authorize(Roles = "Administrador"), GlobalExceptionHandler]
    public class DeletedInfoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DeletedInfo
        public ActionResult Index()
        {
            return View(db.DeletedInfoModels.ToList());
        }

        // GET: DeletedInfo/Details/id
        public ActionResult Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DeletedInfoModel deletedInfoModel = db.DeletedInfoModels.Find(id);

            if (deletedInfoModel != null)
            {
                if (!string.IsNullOrWhiteSpace(deletedInfoModel.DataModel))
                {
                    ViewBag.model = deletedInfoModel.DataModel;
                }
                return View(deletedInfoModel);
            }

            return HttpNotFound();
        }
    }
}