using InWorkWebApp.Custom.Classes;
using InWorkWebApp.Custom.Handlers;
using InWorkWebApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace InWorkWebApp.Controllers
{
    [Authorize(Roles = "Administrador"), GlobalExceptionHandler]
    public class MessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Messages
        public ActionResult Index()
        {
            return View(db.MessageModels.ToList());
        }

        // GET: Messages/Details/id
        public ActionResult Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MessageModel messageModel = db.MessageModels.Find(id);

            if (messageModel != null)
            {
                return View(messageModel);
            }

            return HttpNotFound();
        }

        // GET: Messages/Delete/id
        public ActionResult Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MessageModel messageModel = db.MessageModels.Find(id);

            if (messageModel == null)
            {
                return HttpNotFound();
            }

            return View(messageModel);
        }

        // POST: Messages/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var user = User.Identity.GetUserName();

            MessageModel messageModel = db.MessageModels.Find(id);

            var deletedInfoModel = new DeletedInfoModel()
            {
                Id = Guid.NewGuid(),
                ContentType = ContentTypes.MESSAGES,
                DataModel = new JavaScriptSerializer().Serialize(messageModel),
                DeletedDate = DateTime.Now,
                DeletedBy = user
            };

            db.MessageModels.Remove(messageModel);
            db.DeletedInfoModels.Add(deletedInfoModel);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}