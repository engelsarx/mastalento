using InWorkWebApp.Custom.Classes;
using InWorkWebApp.Custom.Handlers;
using InWorkWebApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace InWorkWebApp.Controllers
{
    [Authorize(Roles = "Administrador, Editor"), GlobalExceptionHandler]
    public class FrequentlyAskedQuestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FrequentlyAskedQuestions
        public ActionResult Index()
        {
            return View(db.FrequentlyAskedQuestionModels.ToList());
        }

        // GET: FrequentlyAskedQuestions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            FrequentlyAskedQuestionModel frequentlyAskedQuestionModel = db.FrequentlyAskedQuestionModels.Find(id);

            if (frequentlyAskedQuestionModel != null)
            {
                frequentlyAskedQuestionModel.Answer = HttpUtility.HtmlDecode(frequentlyAskedQuestionModel.Answer);
                return View(frequentlyAskedQuestionModel);
            }

            return HttpNotFound();
        }

        // GET: FrequentlyAskedQuestions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FrequentlyAskedQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Question,Answer,PublishDate,ExpirationDate")] FrequentlyAskedQuestionModel frequentlyAskedQuestionModel)
        {
            if (string.IsNullOrWhiteSpace(frequentlyAskedQuestionModel.Question) || string.IsNullOrWhiteSpace(frequentlyAskedQuestionModel.Answer))
                return View(frequentlyAskedQuestionModel);

            var user = User.Identity.GetUserName();
            var date = DateTime.Now;

            // Completamos el modelo
            frequentlyAskedQuestionModel.PublishDate = (string.IsNullOrWhiteSpace(frequentlyAskedQuestionModel.PublishDate)) ? date.ToString("dd/MM/yyyy") : frequentlyAskedQuestionModel.PublishDate;
            frequentlyAskedQuestionModel.ExpirationDate = (string.IsNullOrWhiteSpace(frequentlyAskedQuestionModel.ExpirationDate)) ? DateTime.MaxValue.ToString("dd/MM/yyyy") : frequentlyAskedQuestionModel.ExpirationDate;
            frequentlyAskedQuestionModel.CreatedBy = user;
            frequentlyAskedQuestionModel.CreationDate = date;
            frequentlyAskedQuestionModel.ModifiedBy = user;
            frequentlyAskedQuestionModel.LastUpdateDate = date;

            var encodedContent = HttpUtility.HtmlEncode(frequentlyAskedQuestionModel.Answer);
            frequentlyAskedQuestionModel.Answer = encodedContent;

            db.FrequentlyAskedQuestionModels.Add(frequentlyAskedQuestionModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: FrequentlyAskedQuestions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            FrequentlyAskedQuestionModel frequentlyAskedQuestionModel = db.FrequentlyAskedQuestionModels.Find(id);

            if (frequentlyAskedQuestionModel != null)
            {
                frequentlyAskedQuestionModel.Answer = HttpUtility.HtmlDecode(frequentlyAskedQuestionModel.Answer);
                return View(frequentlyAskedQuestionModel);
            }

            return HttpNotFound();
        }

        // POST: FrequentlyAskedQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Question,Answer,PublishDate,ExpirationDate,CreationDate,CreatedBy")] FrequentlyAskedQuestionModel frequentlyAskedQuestionModel)
        {
            var user = User.Identity.GetUserName();
            var date = DateTime.Now;

            // Actualizamos el modelo
            frequentlyAskedQuestionModel.PublishDate = (string.IsNullOrWhiteSpace(frequentlyAskedQuestionModel.PublishDate)) ? date.ToString("dd/MM/yyyy") : frequentlyAskedQuestionModel.PublishDate;
            frequentlyAskedQuestionModel.ExpirationDate = (string.IsNullOrWhiteSpace(frequentlyAskedQuestionModel.ExpirationDate)) ? frequentlyAskedQuestionModel.ExpirationDate : DateTime.MaxValue.ToString("dd/MM/yyyy");
            frequentlyAskedQuestionModel.LastUpdateDate = date;
            frequentlyAskedQuestionModel.ModifiedBy = user;

            var encodedContent = HttpUtility.HtmlEncode(frequentlyAskedQuestionModel.Answer);
            frequentlyAskedQuestionModel.Answer = encodedContent;

            db.Entry(frequentlyAskedQuestionModel).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: FrequentlyAskedQuestions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FrequentlyAskedQuestionModel frequentlyAskedQuestionModel = db.FrequentlyAskedQuestionModels.Find(id);
            if (frequentlyAskedQuestionModel == null)
            {
                return HttpNotFound();
            }
            return View(frequentlyAskedQuestionModel);
        }

        // POST: FrequentlyAskedQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = User.Identity.GetUserName();

            FrequentlyAskedQuestionModel frequentlyAskedQuestionModel = db.FrequentlyAskedQuestionModels.Find(id);

            var deletedInfoModel = new DeletedInfoModel()
            {
                Id = Guid.NewGuid(),
                ContentType = ContentTypes.FAQ,
                DataModel = new JavaScriptSerializer().Serialize(frequentlyAskedQuestionModel),
                DeletedDate = DateTime.Now,
                DeletedBy = user
            };

            db.FrequentlyAskedQuestionModels.Remove(frequentlyAskedQuestionModel);
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
