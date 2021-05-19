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
    public class AggregateValuesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AggregateValues
        public ActionResult Index()
        {
            return View(db.AggregateValueModels.ToList());
        }

        // GET: AggregateValues/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AggregateValueModel aggregateValueModel = db.AggregateValueModels.Find(id);
            if (aggregateValueModel == null)
            {
                return HttpNotFound();
            }
            return View(aggregateValueModel);
        }

        // GET: AggregateValues/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AggregateValues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,IconCode,CreationDate,CreatedBy,LastUpdateDate,ModifiedBy")] AggregateValueModel aggregateValueModel)
        {
            if (string.IsNullOrWhiteSpace(aggregateValueModel.Title) || string.IsNullOrWhiteSpace(aggregateValueModel.Content))
                return View(aggregateValueModel);

            var user = User.Identity.GetUserName();
            var date = DateTime.Now;

            // Completamos el modelo
            aggregateValueModel.CreatedBy = user;
            aggregateValueModel.CreationDate = date;
            aggregateValueModel.ModifiedBy = user;
            aggregateValueModel.LastUpdateDate = date;

            db.AggregateValueModels.Add(aggregateValueModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: AggregateValues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AggregateValueModel aggregateValueModel = db.AggregateValueModels.Find(id);

            if (aggregateValueModel != null)
            {
                return View(aggregateValueModel);
            }

            return HttpNotFound();
        }

        // POST: AggregateValues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,IconCode,CreationDate,CreatedBy,LastUpdateDate,ModifiedBy")] AggregateValueModel aggregateValueModel)
        {
            // Actualizamos el modelo
            aggregateValueModel.LastUpdateDate = DateTime.Now;
            aggregateValueModel.ModifiedBy = User.Identity.GetUserName();

            db.Entry(aggregateValueModel).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: AggregateValues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AggregateValueModel aggregateValueModel = db.AggregateValueModels.Find(id);
            if (aggregateValueModel == null)
            {
                return HttpNotFound();
            }
            return View(aggregateValueModel);
        }

        // POST: AggregateValues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = User.Identity.GetUserName();

            AggregateValueModel aggregateValueModel = db.AggregateValueModels.Find(id);

            var deletedInfoModel = new DeletedInfoModel()
            {
                Id = Guid.NewGuid(),
                ContentType = ContentTypes.VALUES,
                DataModel = new JavaScriptSerializer().Serialize(aggregateValueModel),
                DeletedDate = DateTime.Now,
                DeletedBy = user
            };

            db.AggregateValueModels.Remove(aggregateValueModel);
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
