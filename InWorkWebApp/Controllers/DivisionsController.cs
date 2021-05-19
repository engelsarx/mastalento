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
    public class DivisionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Divisions
        public ActionResult Index()
        {
            return View(db.DivisionModels.ToList());
        }

        // GET: Divisions/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DivisionModel divisionModel = db.DivisionModels.Find(id);

            if (divisionModel != null)
            {
                divisionModel.Content = HttpUtility.HtmlDecode(divisionModel.Content);
                if (divisionModel.Image != null && divisionModel.Image.Length > 0)
                {
                    var base64 = Convert.ToBase64String(divisionModel.Image);
                    ViewBag.image = string.Format("data:image/jpg;base64,{0}", base64);
                }
                return View(divisionModel);
            }

            return HttpNotFound();
        }

        // GET: Divisions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Divisions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Image,Name,Summary,Content,PublishDate,ExpirationDate")] DivisionModel divisionModel, HttpPostedFileBase file)
        {
            if (string.IsNullOrWhiteSpace(divisionModel.Name) || string.IsNullOrWhiteSpace(divisionModel.Content))
                return View(divisionModel);

            var user = User.Identity.GetUserName();
            var date = DateTime.Now;

            // Completamos el modelo
            divisionModel.PublishDate = (string.IsNullOrWhiteSpace(divisionModel.PublishDate)) ? date.ToString("dd/MM/yyyy") : divisionModel.PublishDate;
            divisionModel.ExpirationDate = (string.IsNullOrWhiteSpace(divisionModel.ExpirationDate)) ? DateTime.MaxValue.ToString("dd/MM/yyyy") : divisionModel.ExpirationDate;
            divisionModel.CreatedBy = user;
            divisionModel.CreationDate = date;
            divisionModel.ModifiedBy = user;
            divisionModel.LastUpdateDate = date;

            var encodedContent = HttpUtility.HtmlEncode(divisionModel.Content);
            divisionModel.Content = encodedContent;

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
                    divisionModel.Image = target.ToArray();
                }
            }

            db.DivisionModels.Add(divisionModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Divisions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DivisionModel divisionModel = db.DivisionModels.Find(id);

            if (divisionModel != null)
            {
                divisionModel.Content = HttpUtility.HtmlDecode(divisionModel.Content);
                if (divisionModel.Image != null && divisionModel.Image.Length > 0)
                {
                    var base64 = Convert.ToBase64String(divisionModel.Image);
                    ViewBag.image = string.Format("data:image/jpg;base64,{0}", base64);
                }
                return View(divisionModel);
            }

            return HttpNotFound();
        }

        // POST: Divisions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Image,Name,Summary,Content,PublishDate,ExpirationDate,CreationDate,CreatedBy")] DivisionModel divisionModel, HttpPostedFileBase file)
        {
            var user = User.Identity.GetUserName();
            var date = DateTime.Now;

            // Actualizamos el modelo
            divisionModel.PublishDate = (string.IsNullOrWhiteSpace(divisionModel.PublishDate)) ? date.ToString("dd/MM/yyyy") : divisionModel.PublishDate;
            divisionModel.ExpirationDate = (string.IsNullOrWhiteSpace(divisionModel.ExpirationDate)) ? divisionModel.ExpirationDate : DateTime.MaxValue.ToString("dd/MM/yyyy");
            divisionModel.LastUpdateDate = date;
            divisionModel.ModifiedBy = user;

            var encodedContent = HttpUtility.HtmlEncode(divisionModel.Content);
            divisionModel.Content = encodedContent;

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
                    divisionModel.Image = target.ToArray();
                }
            }

            db.Entry(divisionModel).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Divisions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DivisionModel divisionModel = db.DivisionModels.Find(id);
            if (divisionModel == null)
            {
                return HttpNotFound();
            }
            return View(divisionModel);
        }

        // POST: Divisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = User.Identity.GetUserName();

            DivisionModel divisionModel = db.DivisionModels.Find(id);
            divisionModel.Image = null; // Inicializamos el valor de la imagen para poder serializar el modelo

            var deletedInfoModel = new DeletedInfoModel()
            {
                Id = Guid.NewGuid(),
                ContentType = ContentTypes.DIVISIONS,
                DataModel = new JavaScriptSerializer().Serialize(divisionModel),
                DeletedDate = DateTime.Now,
                DeletedBy = user
            };

            db.DivisionModels.Remove(divisionModel);
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

            DivisionModel divisionModel = db.DivisionModels.Find(id);

            if (divisionModel != null)
            {
                ViewBag.image = string.Empty;
                divisionModel.Content = HttpUtility.HtmlDecode(divisionModel.Content);
                return View("Edit", divisionModel);
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