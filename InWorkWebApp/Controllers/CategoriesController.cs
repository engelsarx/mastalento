using InWorkWebApp.Custom.Classes;
using InWorkWebApp.Custom.Handlers;
using InWorkWebApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace InWorkWebApp.Controllers
{
    [Authorize(Roles = "Administrador, Editor"), GlobalExceptionHandler]
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index()
        {
            return View(db.CategoryModels.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryModel categoryModel = db.CategoryModels.Find(id);
            if (categoryModel == null)
            {
                return HttpNotFound();
            }
            return View(categoryModel);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CreationDate,CreatedBy,LastUpdateDate,ModifiedBy")] CategoryModel categoryModel)
        {
            if (string.IsNullOrWhiteSpace(categoryModel.Name))
                return View(categoryModel);

            var user = User.Identity.GetUserName();
            var date = DateTime.Now;

            // Completamos el modelo
            categoryModel.CreatedBy = user;
            categoryModel.CreationDate = date;
            categoryModel.ModifiedBy = user;
            categoryModel.LastUpdateDate = date;

            db.CategoryModels.Add(categoryModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryModel categoryModel = db.CategoryModels.Find(id);

            if (categoryModel != null)
            {
                return View(categoryModel);
            }

            return HttpNotFound();
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CreationDate,CreatedBy,LastUpdateDate,ModifiedBy")] CategoryModel categoryModel)
        {
            // Actualizamos el modelo
            categoryModel.LastUpdateDate = DateTime.Now;
            categoryModel.ModifiedBy = User.Identity.GetUserName();

            db.Entry(categoryModel).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryModel categoryModel = db.CategoryModels.Find(id);
            if (categoryModel == null)
            {
                return HttpNotFound();
            }
            return View(categoryModel);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = User.Identity.GetUserName();

            CategoryModel categoryModel = db.CategoryModels.Find(id);

            var deletedInfoModel = new DeletedInfoModel()
            {
                Id = Guid.NewGuid(),
                ContentType = ContentTypes.CATEGORIES,
                DataModel = new JavaScriptSerializer().Serialize(categoryModel),
                DeletedDate = DateTime.Now,
                DeletedBy = user
            };

            db.CategoryModels.Remove(categoryModel);
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
