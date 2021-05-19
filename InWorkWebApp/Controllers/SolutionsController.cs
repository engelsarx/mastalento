using InWorkWebApp.Custom.Classes;
using InWorkWebApp.Custom.Handlers;
using InWorkWebApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace InWorkWebApp.Controllers
{
    [Authorize(Roles = "Administrador, Editor"), GlobalExceptionHandler]
    public class SolutionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Solutions
        public ActionResult Index()
        {
            var solutionModels = db.SolutionModels.Include(s => s.Category);
            return View(solutionModels.ToList());
        }

        // GET: Solutions/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SolutionModel solutionModel = db.SolutionModels.Find(id);

            if (solutionModel != null)
            {
                solutionModel.Description = HttpUtility.HtmlDecode(solutionModel.Description);
                if (solutionModel.Image != null && solutionModel.Image.Length > 0)
                {
                    var base64 = Convert.ToBase64String(solutionModel.Image);
                    ViewBag.image = string.Format("data:image/jpg;base64,{0}", base64);
                }
                return View(solutionModel);
            }

            return HttpNotFound();
        }

        // GET: Solutions/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.CategoryModels, "Id", "Name");
            return View();
        }

        // POST: Solutions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Image,Name,Summary,Description,CategoryId,SolutionType,PublishDate,ExpirationDate")] SolutionModel solutionModel, HttpPostedFileBase file)
        {
            if (string.IsNullOrWhiteSpace(solutionModel.Name) || string.IsNullOrWhiteSpace(solutionModel.Description))
            {
                ViewBag.CategoryId = new SelectList(db.CategoryModels, "Id", "Name", solutionModel.CategoryId);
                return View(solutionModel);
            }

            var user = User.Identity.GetUserName();
            var date = DateTime.Now;

            // Completamos el modelo
            solutionModel.PublishDate = (string.IsNullOrWhiteSpace(solutionModel.PublishDate)) ? date.ToString("dd/MM/yyyy") : solutionModel.PublishDate;
            solutionModel.ExpirationDate = (string.IsNullOrWhiteSpace(solutionModel.ExpirationDate)) ? DateTime.MaxValue.ToString("dd/MM/yyyy") : solutionModel.ExpirationDate;
            solutionModel.CreatedBy = user;
            solutionModel.CreationDate = date;
            solutionModel.ModifiedBy = user;
            solutionModel.LastUpdateDate = date;

            var encodedContent = HttpUtility.HtmlEncode(solutionModel.Description);
            solutionModel.Description = encodedContent;

            if (file != null && file.ContentLength > 0)
            {
                using (Stream inputStream = file.InputStream)
                {
                    var target = inputStream as MemoryStream;
                    if (target == null)
                    {
                        target = new MemoryStream();
                        inputStream.CopyTo(target);
                    }
                    solutionModel.Image = target.ToArray();
                }
            }

            db.SolutionModels.Add(solutionModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Solutions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SolutionModel solutionModel = db.SolutionModels.Find(id);

            if (solutionModel != null)
            {
                ViewBag.CategoryId = new SelectList(db.CategoryModels, "Id", "Name", solutionModel.CategoryId);
                solutionModel.Description = HttpUtility.HtmlDecode(solutionModel.Description);
                if (solutionModel.Image != null && solutionModel.Image.Length > 0)
                {
                    var base64 = Convert.ToBase64String(solutionModel.Image);
                    ViewBag.image = string.Format("data:image/jpg;base64,{0}", base64);
                }
                return View(solutionModel);
            }

            return HttpNotFound();
        }

        // POST: Solutions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Image,Name,Summary,Description,CategoryId,SolutionType,PublishDate,ExpirationDate,CreationDate,CreatedBy")] SolutionModel solutionModel, HttpPostedFileBase file)
        {
            ViewBag.CategoryId = new SelectList(db.CategoryModels, "Id", "Name", solutionModel.CategoryId);

            var user = User.Identity.GetUserName();
            var date = DateTime.Now;

            // Actualizamos el modelo
            solutionModel.PublishDate = (string.IsNullOrWhiteSpace(solutionModel.PublishDate)) ? date.ToString("dd/MM/yyyy") : solutionModel.PublishDate;
            solutionModel.ExpirationDate = (string.IsNullOrWhiteSpace(solutionModel.ExpirationDate)) ? solutionModel.ExpirationDate : DateTime.MaxValue.ToString("dd/MM/yyyy");
            solutionModel.LastUpdateDate = date;
            solutionModel.ModifiedBy = user;

            var encodedContent = HttpUtility.HtmlEncode(solutionModel.Description);
            solutionModel.Description = encodedContent;

            if (file != null && file.ContentLength > 0)
            {
                using (Stream inputStream = file.InputStream)
                {
                    var target = inputStream as MemoryStream;
                    if (target == null)
                    {
                        target = new MemoryStream();
                        inputStream.CopyTo(target);
                    }
                    solutionModel.Image = target.ToArray();
                }
            }

            db.Entry(solutionModel).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Solutions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SolutionModel solutionModel = db.SolutionModels.Find(id);
            if (solutionModel == null)
            {
                return HttpNotFound();
            }
            return View(solutionModel);
        }

        // POST: Solutions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = User.Identity.GetUserName();

            SolutionModel solutionModel = db.SolutionModels.Find(id);
            solutionModel.Image = null; // Inicializamos el valor de la imagen para poder serializar el modelo

            var deletedInfoModel = new DeletedInfoModel()
            {
                Id = Guid.NewGuid(),
                ContentType = ContentTypes.SOLUTIONS,
                DataModel = new JavaScriptSerializer().Serialize(solutionModel),
                DeletedDate = DateTime.Now,
                DeletedBy = user
            };

            db.SolutionModels.Remove(solutionModel);
            db.DeletedInfoModels.Add(deletedInfoModel);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult DeleteImage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SolutionModel solutionModel = db.SolutionModels.Find(id);

            if (solutionModel != null)
            {
                ViewBag.image = string.Empty;
                solutionModel.Description = HttpUtility.HtmlDecode(solutionModel.Description);
                ViewBag.CategoryId = new SelectList(db.CategoryModels, "Id", "Name", solutionModel.CategoryId);
                return View("Edit", solutionModel);
            }

            return HttpNotFound();
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
