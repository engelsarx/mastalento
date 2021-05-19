using InWorkWebApp.Custom.Handlers;
using InWorkWebApp.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace InWorkWebApp.Controllers
{
    [Authorize(Roles = "Administrador"), GlobalExceptionHandler]
    public class ApplicationDataController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApplicationData/Create
        public ActionResult Create()
        {
            return View(new ApplicationDataModel());
        }

        // POST: ApplicationData/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        // public ActionResult Create([Bind(Include = "Id,Logo,Brand,Slogan,Intro,ShowAggregateValuesOnMainPage,ShowDivisionsOnMainPage,ShowSolutionsOnMainPage,ShowNewsOnMainPage,ShowFAQOnMainPage,AboutIntro,AboutDescription,SquareIconOne,SquareTextOne,SquareIconTwo,SquareTextTwo,AboutPrimaryBanner,AboutSecondaryBanner,AboutAlternativeBanner,ItemIconOne,ItemNameOne,ItemDescriptionOne,ItemIconTwo,ItemNameTwo,ItemDescriptionTwo,ItemIconThree,ItemNameThree,ItemDescriptionThree,ItemIconFour,ItemNameFour,ItemDescriptionFour,DivisionsIntro,DivisionsDescription,SolutionsIntro,SolutionsDescription,NewsIntro,NewsDescription,ContactIntro,ContactDescription,ContactEmail,ContactPhoneNumber,FAQIntro,FAQDescription")] ApplicationDataModel applicationDataModel, HttpPostedFileBase file)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ApplicationDataModel applicationDataModel, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
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
                        applicationDataModel.Logo = target.ToArray();
                    }
                }

                applicationDataModel.Id = Guid.NewGuid();
                db.ApplicationDataModels.Add(applicationDataModel);
                db.SaveChanges();

                return View("Edit", applicationDataModel);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: ApplicationData/Edit/5
        public ActionResult Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationDataModel applicationDataModel = db.ApplicationDataModels.Find(id);

            if (applicationDataModel != null)
            {
                if (applicationDataModel.Logo != null && applicationDataModel.Logo.Length > 0)
                {
                    var base64 = Convert.ToBase64String(applicationDataModel.Logo);
                    ViewBag.image = string.Format("data:image/jpg;base64,{0}", base64);
                }
                return View(applicationDataModel);
            }

            return HttpNotFound();
        }

        // POST: ApplicationData/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        // public ActionResult Edit([Bind(Include = "Id,Logo,Brand,Slogan,Intro,ShowAggregateValuesOnMainPage,ShowDivisionsOnMainPage,ShowSolutionsOnMainPage,ShowNewsOnMainPage,ShowFAQOnMainPage,AboutIntro,AboutDescription,SquareIconOne,SquareTextOne,SquareIconTwo,SquareTextTwo,AboutPrimaryBanner,AboutSecondaryBanner,AboutAlternativeBanner,ItemIconOne,ItemNameOne,ItemDescriptionOne,ItemIconTwo,ItemNameTwo,ItemDescriptionTwo,ItemIconThree,ItemNameThree,ItemDescriptionThree,ItemIconFour,ItemNameFour,ItemDescriptionFour,DivisionsIntro,DivisionsDescription,SolutionsIntro,SolutionsDescription,NewsIntro,NewsDescription,ContactIntro,ContactDescription,ContactEmail,ContactPhoneNumber,FAQIntro,FAQDescription")] ApplicationDataModel applicationDataModel, HttpPostedFileBase file)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationDataModel applicationDataModel, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
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
                        applicationDataModel.Logo = target.ToArray();
                    }
                }

                db.Entry(applicationDataModel).State = EntityState.Modified;
                db.SaveChanges();

                return View("Edit", applicationDataModel);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult DeleteImage(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationDataModel applicationDataModel = db.ApplicationDataModels.Find(id);

            if (applicationDataModel != null)
            {
                ViewBag.image = string.Empty;
                return View("Edit", applicationDataModel);
            }

            return HttpNotFound();
        }

        public ActionResult LoadDefaultValues()
        {
            ViewBag.image = string.Empty;

            // Construímos un modelo con los valores de Settings.config
            var applicationDataModel = new ApplicationDataModel
            {
                Brand = WebConfigurationManager.AppSettings["Brand"],
                Slogan = WebConfigurationManager.AppSettings["Slogan"],
                Intro = WebConfigurationManager.AppSettings["Intro"],
                ShowAggregateValuesOnMainPage = bool.Parse(WebConfigurationManager.AppSettings["ShowAggregateValuesOnMainPage"]),
                ShowDivisionsOnMainPage = bool.Parse(WebConfigurationManager.AppSettings["ShowDivisionsOnMainPage"]),
                ShowSolutionsOnMainPage = bool.Parse(WebConfigurationManager.AppSettings["ShowSolutionsOnMainPage"]),
                ShowNewsOnMainPage = bool.Parse(WebConfigurationManager.AppSettings["ShowNewsOnMainPage"]),
                ShowFAQOnMainPage = bool.Parse(WebConfigurationManager.AppSettings["ShowFAQOnMainPage"]),

                AboutIntro = WebConfigurationManager.AppSettings["AboutIntro"],
                AboutDescription = WebConfigurationManager.AppSettings["AboutDescription"],
                SquareIconOne = WebConfigurationManager.AppSettings["SquareIconOne"],
                SquareTextOne = WebConfigurationManager.AppSettings["SquareTextOne"],
                SquareIconTwo = WebConfigurationManager.AppSettings["SquareIconTwo"],
                SquareTextTwo = WebConfigurationManager.AppSettings["SquareTextTwo"],
                AboutPrimaryBanner = WebConfigurationManager.AppSettings["AboutPrimaryBanner"],
                AboutSecondaryBanner = WebConfigurationManager.AppSettings["AboutSecondaryBanner"],
                AboutAlternativeBanner = WebConfigurationManager.AppSettings["AboutAlternativeBanner"],
                ItemIconOne = WebConfigurationManager.AppSettings["ItemIconOne"],
                ItemNameOne = WebConfigurationManager.AppSettings["ItemNameOne"],
                ItemDescriptionOne = WebConfigurationManager.AppSettings["ItemDescriptionOne"],
                ItemIconTwo = WebConfigurationManager.AppSettings["ItemIconTwo"],
                ItemNameTwo = WebConfigurationManager.AppSettings["ItemNameTwo"],
                ItemDescriptionTwo = WebConfigurationManager.AppSettings["ItemDescriptionTwo"],
                ItemIconThree = WebConfigurationManager.AppSettings["ItemIconThree"],
                ItemNameThree = WebConfigurationManager.AppSettings["ItemNameThree"],
                ItemDescriptionThree = WebConfigurationManager.AppSettings["ItemDescriptionThree"],
                ItemIconFour = WebConfigurationManager.AppSettings["ItemIconFour"],
                ItemNameFour = WebConfigurationManager.AppSettings["ItemNameFour"],
                ItemDescriptionFour = WebConfigurationManager.AppSettings["ItemDescriptionFour"],

                DivisionsIntro = WebConfigurationManager.AppSettings["DivisionsIntro"],
                DivisionsDescription = WebConfigurationManager.AppSettings["DivisionsDescription"],

                SolutionsIntro = WebConfigurationManager.AppSettings["SolutionsIntro"],
                SolutionsDescription = WebConfigurationManager.AppSettings["SolutionsDescription"],

                NewsIntro = WebConfigurationManager.AppSettings["NewsIntro"],
                NewsDescription = WebConfigurationManager.AppSettings["NewsDescription"],

                ContactIntro = WebConfigurationManager.AppSettings["ContactIntro"],
                ContactDescription = WebConfigurationManager.AppSettings["ContactDescription"],
                ContactEmail = WebConfigurationManager.AppSettings["ContactEmail"],
                ContactPhoneNumber = WebConfigurationManager.AppSettings["ContactPhoneNumber"],

                FAQIntro = WebConfigurationManager.AppSettings["FAQIntro"],
                FAQDescription = WebConfigurationManager.AppSettings["FAQDescription"]
            };

            return View("Edit", applicationDataModel);
        }

        public ActionResult ResetValues(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationDataModel applicationDataModel = db.ApplicationDataModels.Find(id);

            if (applicationDataModel != null)
            {
                ViewBag.image = string.Empty;

                // Obtenemos los valores de Settings.config
                applicationDataModel.Brand = WebConfigurationManager.AppSettings["Brand"];
                applicationDataModel.Slogan = WebConfigurationManager.AppSettings["Slogan"];
                applicationDataModel.Intro = WebConfigurationManager.AppSettings["Intro"];
                applicationDataModel.ShowAggregateValuesOnMainPage = bool.Parse(WebConfigurationManager.AppSettings["ShowAggregateValuesOnMainPage"]);
                applicationDataModel.ShowDivisionsOnMainPage = bool.Parse(WebConfigurationManager.AppSettings["ShowDivisionsOnMainPage"]);
                applicationDataModel.ShowSolutionsOnMainPage = bool.Parse(WebConfigurationManager.AppSettings["ShowSolutionsOnMainPage"]);
                applicationDataModel.ShowNewsOnMainPage = bool.Parse(WebConfigurationManager.AppSettings["ShowNewsOnMainPage"]);
                applicationDataModel.ShowFAQOnMainPage = bool.Parse(WebConfigurationManager.AppSettings["ShowFAQOnMainPage"]);

                applicationDataModel.AboutIntro = WebConfigurationManager.AppSettings["AboutIntro"];
                applicationDataModel.AboutDescription = WebConfigurationManager.AppSettings["AboutDescription"];
                applicationDataModel.SquareIconOne = WebConfigurationManager.AppSettings["SquareIconOne"];
                applicationDataModel.SquareTextOne = WebConfigurationManager.AppSettings["SquareTextOne"];
                applicationDataModel.SquareIconTwo = WebConfigurationManager.AppSettings["SquareIconTwo"];
                applicationDataModel.SquareTextTwo = WebConfigurationManager.AppSettings["SquareTextTwo"];
                applicationDataModel.AboutPrimaryBanner = WebConfigurationManager.AppSettings["AboutPrimaryBanner"];
                applicationDataModel.AboutSecondaryBanner = WebConfigurationManager.AppSettings["AboutSecondaryBanner"];
                applicationDataModel.AboutAlternativeBanner = WebConfigurationManager.AppSettings["AboutAlternativeBanner"];
                applicationDataModel.ItemIconOne = WebConfigurationManager.AppSettings["ItemIconOne"];
                applicationDataModel.ItemNameOne = WebConfigurationManager.AppSettings["ItemNameOne"];
                applicationDataModel.ItemDescriptionOne = WebConfigurationManager.AppSettings["ItemDescriptionOne"];
                applicationDataModel.ItemIconTwo = WebConfigurationManager.AppSettings["ItemIconTwo"];
                applicationDataModel.ItemNameTwo = WebConfigurationManager.AppSettings["ItemNameTwo"];
                applicationDataModel.ItemDescriptionTwo = WebConfigurationManager.AppSettings["ItemDescriptionTwo"];
                applicationDataModel.ItemIconThree = WebConfigurationManager.AppSettings["ItemIconThree"];
                applicationDataModel.ItemNameThree = WebConfigurationManager.AppSettings["ItemNameThree"];
                applicationDataModel.ItemDescriptionThree = WebConfigurationManager.AppSettings["ItemDescriptionThree"];
                applicationDataModel.ItemIconFour = WebConfigurationManager.AppSettings["ItemIconFour"];
                applicationDataModel.ItemNameFour = WebConfigurationManager.AppSettings["ItemNameFour"];
                applicationDataModel.ItemDescriptionFour = WebConfigurationManager.AppSettings["ItemDescriptionFour"];

                applicationDataModel.DivisionsIntro = WebConfigurationManager.AppSettings["DivisionsIntro"];
                applicationDataModel.DivisionsDescription = WebConfigurationManager.AppSettings["DivisionsDescription"];

                applicationDataModel.SolutionsIntro = WebConfigurationManager.AppSettings["SolutionsIntro"];
                applicationDataModel.SolutionsDescription = WebConfigurationManager.AppSettings["SolutionsDescription"];

                applicationDataModel.NewsIntro = WebConfigurationManager.AppSettings["NewsIntro"];
                applicationDataModel.NewsDescription = WebConfigurationManager.AppSettings["NewsDescription"];

                applicationDataModel.ContactIntro = WebConfigurationManager.AppSettings["ContactIntro"];
                applicationDataModel.ContactDescription = WebConfigurationManager.AppSettings["ContactDescription"];
                applicationDataModel.ContactEmail = WebConfigurationManager.AppSettings["ContactEmail"];
                applicationDataModel.ContactPhoneNumber = WebConfigurationManager.AppSettings["ContactPhoneNumber"];

                applicationDataModel.FAQIntro = WebConfigurationManager.AppSettings["FAQIntro"];
                applicationDataModel.FAQDescription = WebConfigurationManager.AppSettings["FAQDescription"];

                return View("Edit", applicationDataModel);
            }

            return HttpNotFound();
        }
    }
}