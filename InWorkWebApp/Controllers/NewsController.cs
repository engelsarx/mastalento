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
    public class NewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: News
        public ActionResult Index()
        {
            var newsModels = db.NewsModels.Include(n => n.Category);
            return View(newsModels.ToList());
        }

        // GET: News/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NewsModel newsModel = db.NewsModels.Find(id);

            if (newsModel != null)
            {
                newsModel.Description = HttpUtility.HtmlDecode(newsModel.Description);
                if (newsModel.Image != null && newsModel.Image.Length > 0)
                {
                    var base64 = Convert.ToBase64String(newsModel.Image);
                    ViewBag.image = string.Format("data:image/jpg;base64,{0}", base64);
                }
                return View(newsModel);
            }

            return HttpNotFound();
        }

        // GET: News/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.CategoryModels, "Id", "Name");
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Image,Title,Subtitle,Summary,Description,NewsType,Link,CategoryId,PublishDate,ExpirationDate")] NewsModel newsModel, HttpPostedFileBase file)
        {
            if (string.IsNullOrWhiteSpace(newsModel.Title) || string.IsNullOrWhiteSpace(newsModel.Description))
            {
                ViewBag.CategoryId = new SelectList(db.CategoryModels, "Id", "Name", newsModel.CategoryId);
                return View(newsModel);
            }

            var user = User.Identity.GetUserName();
            var date = DateTime.Now;

            // Completamos el modelo
            newsModel.PublishDate = (string.IsNullOrWhiteSpace(newsModel.PublishDate)) ? date.ToString("dd/MM/yyyy") : newsModel.PublishDate;
            newsModel.ExpirationDate = (string.IsNullOrWhiteSpace(newsModel.ExpirationDate)) ? DateTime.MaxValue.ToString("dd/MM/yyyy") : newsModel.ExpirationDate;
            newsModel.CreatedBy = user;
            newsModel.CreationDate = date;
            newsModel.ModifiedBy = user;
            newsModel.LastUpdateDate = date;

            var encodedContent = HttpUtility.HtmlEncode(newsModel.Description);
            newsModel.Description = encodedContent;

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
                    newsModel.Image = target.ToArray();
                }
            }

            db.NewsModels.Add(newsModel);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NewsModel newsModel = db.NewsModels.Find(id);

            if (newsModel != null)
            {
                ViewBag.CategoryId = new SelectList(db.CategoryModels, "Id", "Name", newsModel.CategoryId);
                newsModel.Description = HttpUtility.HtmlDecode(newsModel.Description);
                if (newsModel.Image != null && newsModel.Image.Length > 0)
                {
                    var base64 = Convert.ToBase64String(newsModel.Image);
                    ViewBag.image = string.Format("data:image/jpg;base64,{0}", base64);
                }
                return View(newsModel);
            }

            return HttpNotFound();
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Image,Title,Subtitle,Summary,Description,NewsType,Link,CategoryId,PublishDate,ExpirationDate,CreationDate,CreatedBy")] NewsModel newsModel, HttpPostedFileBase file)
        {
            ViewBag.CategoryId = new SelectList(db.CategoryModels, "Id", "Name", newsModel.CategoryId);

            var user = User.Identity.GetUserName();
            var date = DateTime.Now;

            // Actualizamos el modelo
            newsModel.PublishDate = (string.IsNullOrWhiteSpace(newsModel.PublishDate)) ? date.ToString("dd/MM/yyyy") : newsModel.PublishDate;
            newsModel.ExpirationDate = (string.IsNullOrWhiteSpace(newsModel.ExpirationDate)) ? DateTime.MaxValue.ToString("dd/MM/yyyy") : newsModel.ExpirationDate;
            newsModel.LastUpdateDate = date;
            newsModel.ModifiedBy = user;

            var encodedContent = HttpUtility.HtmlEncode(newsModel.Description);
            newsModel.Description = encodedContent;

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
                    newsModel.Image = target.ToArray();
                }
            }

            db.Entry(newsModel).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsModel newsModel = db.NewsModels.Find(id);
            if (newsModel == null)
            {
                return HttpNotFound();
            }
            return View(newsModel);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = User.Identity.GetUserName();

            NewsModel newsModel = db.NewsModels.Find(id);
            newsModel.Image = null; // Inicializamos el valor de la imagen para poder serializar el modelo

            var deletedInfoModel = new DeletedInfoModel()
            {
                Id = Guid.NewGuid(),
                ContentType = ContentTypes.NEWS,
                DataModel = new JavaScriptSerializer().Serialize(newsModel),
                DeletedDate = DateTime.Now,
                DeletedBy = user
            };

            db.NewsModels.Remove(newsModel);
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

            NewsModel newsModel = db.NewsModels.Find(id);

            if (newsModel != null)
            {
                ViewBag.image = string.Empty;
                newsModel.Description = HttpUtility.HtmlDecode(newsModel.Description);
                ViewBag.CategoryId = new SelectList(db.CategoryModels, "Id", "Name", newsModel.CategoryId);
                return View("Edit", newsModel);
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
