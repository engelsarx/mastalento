using System;
using SendGrid;
using System.Net;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using InWorkWebApp.Models;
using System.Globalization;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Collections.Generic;
using InWorkWebApp.Custom.Handlers;
using System.Web.Script.Serialization;

namespace InWorkWebApp.Controllers
{
    [GlobalExceptionHandler]
    public class HomeController : Controller
    {
        // Contexto de la base de datos
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // Datos para el envío de mensajes por correo electrónico
        private readonly string apiKey = WebConfigurationManager.AppSettings["SendGridApiKey"];
        private readonly string defaultEmail = WebConfigurationManager.AppSettings["SendGridDefaultEmail"];
        private readonly string defaultName = WebConfigurationManager.AppSettings["SendGridDefaultName"];
        private readonly bool allowEmailForwarding = bool.Parse(WebConfigurationManager.AppSettings["AllowEmailForwarding"]);
        private readonly bool allowSuscriptionEmails = bool.Parse(WebConfigurationManager.AppSettings["AllowSuscriptionEmails"]);

        public ActionResult Index()
        {
            LoadData("Index");
            return View();
        }

        public ActionResult About()
        {
            LoadData("About");
            return View();
        }

        public ActionResult Divisions()
        {
            LoadData("Divisions");
            return View();
        }

        public ActionResult Solutions()
        {
            LoadData("Solutions");
            return View();
        }

        public ActionResult News()
        {
            LoadData("News");
            return View();
        }

        public ActionResult FAQ()
        {
            LoadData("FAQ");
            return View();
        }

        public ActionResult Contact()
        {
            LoadData("Contact");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact([Bind(Include = "Id,Name,Email,Affair,Message,RegisterDate")] MessageModel model)
        {
            LoadData("Contact");
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Message))
            {
                ViewBag.FormSubmissionResult = new FormSubmissionViewModel
                {
                    Origin = "CONTACT",
                    Message = "Se requiere un nombre, un correo electrónico y un mensaje",
                    MessageType = "ERROR"
                };

                return View("Contact", model);
            }

            var captchaSucceeded = await ReCaptchaPassed(Request.Form["g-recaptcha-response"]);
            if (captchaSucceeded)
            {
                // Completamos el modelo
                model.Id = Guid.NewGuid();
                model.Affair = "General"; // Por el momento envíamos un asunto general
                model.RegisterDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                // Guardamos los datos
                db.MessageModels.Add(model);
                await db.SaveChangesAsync();

                var emailResponse = await SendContactEmail(model);
                if (emailResponse.StatusCode == HttpStatusCode.Accepted)
                {
                    ViewBag.FormSubmissionResult = new FormSubmissionViewModel
                    {
                        Origin = "CONTACT",
                        Message = "Gracias por contactarnos, en breve nos comunicaremos contigo",
                        MessageType = "SUCCESS"
                    };
                }
                else
                {
                    ViewBag.FormSubmissionResult = new FormSubmissionViewModel
                    {
                        Origin = "CONTACT",
                        Message = "Lo sentimos, no pudimos enviar tu mensaje",
                        MessageType = "ERROR"
                    };
                }
            }
            else
            {
                ViewBag.FormSubmissionResult = new FormSubmissionViewModel
                {
                    Origin = "CONTACT",
                    Message = "Lo sentimos, debes superar la validación del reCaptcha",
                    MessageType = "ERROR"
                };
            }

            return View("Contact");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Subscribe(string origin, string email)
        {

            // Evaluamos el origen antes que el correo para que se recarguen los datos antes de refrescar la página
            if (origin.Contains("Subscribe"))
            {
                origin = "Index"; // Para evitar el error a la hora de redireccionar al usuario
            }

            LoadData(origin);

            // Evaluamos el correo electrónico
            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.FormSubmissionResult = new FormSubmissionViewModel
                {
                    Origin = "NEWSLETTER",
                    Message = "Se requiere un correo electrónico",
                    MessageType = "ERROR"
                };

                return View(origin);
            }

            var isEmailSuscribed = db.MessageModels.Any(r => r.Email.Equals(email.ToLower())) ? true : false;

            if (isEmailSuscribed)
            {
                ViewBag.FormSubmissionResult = new FormSubmissionViewModel
                {
                    Origin = "NEWSLETTER",
                    Message = "Ya te encuentras suscrito a nuestras publicaciones",
                    MessageType = "ERROR"
                };

                return View(origin);
            }

            var captchaSucceeded = await ReCaptchaPassed(Request.Form["g-recaptcha-response"]);
            if (captchaSucceeded)
            {
                // Construímos un mensaje similar al de contacto con datos predeterminados
                var model = new MessageModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Nuevo usuario",
                    Email = email,
                    Affair = "Suscripción",
                    Message = "Deseo suscribirme para recibir información de sus productos, servicios y publicaciones",
                    RegisterDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
                };

                // Guardamos los datos
                db.MessageModels.Add(model);
                await db.SaveChangesAsync();

                // Sí los correos de suscripción están permitidos, envíamos correo
                if (allowSuscriptionEmails)
                {
                    var emailResponse = await SendSubscriptionEmail(model);
                    if (emailResponse.StatusCode == HttpStatusCode.Accepted)
                    {
                        ViewBag.FormSubmissionResult = new FormSubmissionViewModel
                        {
                            Origin = "NEWSLETTER",
                            Message = "Gracias por suscribirte, a partir de este momento recibirás información de nuestros productos, servicios y publicaciones",
                            MessageType = "SUCCESS"
                        };
                    }
                    else
                    {
                        ViewBag.FormSubmissionResult = new FormSubmissionViewModel
                        {
                            Origin = "NEWSLETTER",
                            Message = "Lo sentimos, no pudimos enviar tu mensaje",
                            MessageType = "ERROR"
                        };
                    }
                }
                else
                {
                    // Sí no se admiten correos de suscripción, sólo notificamos al usuario
                    ViewBag.FormSubmissionResult = new FormSubmissionViewModel
                    {
                        Origin = "NEWSLETTER",
                        Message = "Gracias por suscribirte, a partir de este momento recibirás información de nuestros productos, servicios y publicaciones",
                        MessageType = "SUCCESS"
                    };
                }
            }
            else
            {
                ViewBag.FormSubmissionResult = new FormSubmissionViewModel
                {
                    Origin = "NEWSLETTER",
                    Message = "Lo sentimos, debes superar la validación del reCaptcha",
                    MessageType = "ERROR"
                };
            }

            return View(origin);
        }

        #region Helpers

        private void LoadData(string origin)
        {
            // Evaluamos el origen antes que el correo para que se recarguen los datos antes de refrescar la página
            if (origin.Contains("Subscribe"))
            {
                origin = "Index"; // Para evitar el error a la hora de redireccionar al usuario
            }

            // Realizamos la recarga de los datos dependiendo el origen, de esta forma sólo cargamos lo que se necesitará para la vista
            if (origin.Equals("Index", StringComparison.InvariantCultureIgnoreCase))
            {
                LoadApplicationData();
                LoadAggregateValues();
                LoadDivisions();
                LoadSolutions();
                LoadNews();
                LoadFAQ();
            }

            if (origin.Equals("About", StringComparison.InvariantCultureIgnoreCase))
            {
                LoadApplicationData();
                LoadDivisions();
            }

            if (origin.Equals("Divisions", StringComparison.InvariantCultureIgnoreCase))
            {
                LoadApplicationData();
                LoadDivisions();
            }

            if (origin.Equals("Solutions", StringComparison.InvariantCultureIgnoreCase))
            {
                LoadApplicationData();
                LoadDivisions();
                LoadSolutions();
            }

            if (origin.Equals("Contact", StringComparison.InvariantCultureIgnoreCase))
            {
                LoadApplicationData();
                LoadDivisions();
            }

            if (origin.Equals("News", StringComparison.InvariantCultureIgnoreCase))
            {
                LoadApplicationData();
                LoadDivisions();
                LoadNews();
            }

            if (origin.Equals("FAQ", StringComparison.InvariantCultureIgnoreCase))
            {
                LoadApplicationData();
                LoadDivisions();
                LoadFAQ();
            }
        }

        private void LoadApplicationData()
        {
            ViewBag.ApplicationData = db.ApplicationDataModels.Any() ? db.ApplicationDataModels.First() : null;
        }

        private void LoadAggregateValues()
        {
            ViewBag.AggregateValues = db.AggregateValueModels.Any() ? db.AggregateValueModels.ToList() : null;
        }

        private void LoadDivisions()
        {
            try
            {
                var divisions = db.DivisionModels.Any() ? db.DivisionModels.ToList() : null;
                ViewBag.Divisions = divisions?.Where(r => ShowContent(r.PublishDate, r.ExpirationDate)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error al cargar los grupos. Detalle: {ex.Message}", ex);
            }
        }

        private void LoadSolutions()
        {
            try
            {
                var solutions = db.SolutionModels.Any() ? db.SolutionModels.ToList() : null;
                ViewBag.Solutions = solutions?.Where(r => ShowContent(r.PublishDate, r.ExpirationDate)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error al cargar las soluciones. Detalle: {ex.Message}", ex);
            }
        }

        private void LoadNews()
        {
            try
            {
                var news = db.NewsModels.Any() ? db.NewsModels.ToList() : null;
                ViewBag.News = news?.Where(r => ShowContent(r.PublishDate, r.ExpirationDate)).ToList();
                ViewBag.LatestNews = (ViewBag.News as List<NewsModel>)?.OrderByDescending(r => r.CreationDate).Take(3).ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error al cargar las publicaciones. Detalle: {ex.Message}", ex);
            }
        }

        private void LoadFAQ()
        {
            ViewBag.FrecuentlyAskedQuestions = db.FrequentlyAskedQuestionModels.Any() ? db.FrequentlyAskedQuestionModels.ToList() : null;
        }

        private bool ShowContent(string publishDateStr, string expirationDateStr)
        {

            bool isPublishDateValid = DateTime.TryParse(publishDateStr, out DateTime publishDate);
            bool isExpirationDateValid = DateTime.TryParse(expirationDateStr, out DateTime expirationDate);

            if (isPublishDateValid && isExpirationDateValid)
            {
                var currentDateStr = DateTime.UtcNow.Date.ToString(@"dd\/MM\/yyyy");

                var currentDate = DateTime.ParseExact(currentDateStr, @"dd\/MM\/yyyy", CultureInfo.InvariantCulture);

                var first = DateTime.Compare(currentDate, publishDate.Date);
                var second = DateTime.Compare(currentDate, expirationDate.Date);

                // Sí la fecha actual es mayor o igual a la fecha de publicación y es menor o igual a la fecha de expiración se debería mostrar el contenido
                return (first >= 0 && second <= 0);
            }

            return false;
        }

        private async Task<bool> ReCaptchaPassed(string gRecaptchaResponse)
        {
            if (string.IsNullOrWhiteSpace(gRecaptchaResponse))
            {
                return false;
            }

            HttpClient client = new HttpClient { BaseAddress = new Uri("https://www.google.com") };
            var secret = WebConfigurationManager.AppSettings["ReCaptchaApiSecret"];

            var values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("secret", secret),
                new KeyValuePair<string, string>("response", gRecaptchaResponse)
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await client.PostAsync("/recaptcha/api/siteverify", content);

            string responseReader = await response.Content.ReadAsStringAsync();

            var verificationResult = JsonConvert.DeserializeObject<ReCaptchaValidationResultModel>(responseReader);

            return verificationResult.Success;
        }

        private async Task<Response> SendContactEmail(MessageModel model)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(defaultEmail, defaultName);
            var subject = "Nuevo mensaje de contacto";
            var plainTextContent = new JavaScriptSerializer().Serialize(model);
            SendGridMessage msg;

            if (allowEmailForwarding)
            {
                // Envíamos el mensaje tanto a InWork como al usuario
                var to = new List<EmailAddress>
                {
                    new EmailAddress(defaultEmail, defaultName),
                    new EmailAddress(model.Email, model.Name)
                };

                var htmlContent =
                    $"<div>" +
                    $"<h4>Recibimos tu mensaje</h4>" +
                    $"<dl>" +
                    $"<dt>Fecha y hora:</dt>" +
                    $"<dd>{model.RegisterDate}</dd>" +
                    $"<dt>Nombre:</dt>" +
                    $"<dd>{model.Name}</dd>" +
                    $"<dt>Correo electrónico:</dt>" +
                    $"<dd>{model.Email}</dd>" +
                    $"<dt>Asunto:</dt>" +
                    $"<dd>{model.Affair}</dd>" +
                    $"<dt>Mensaje:</dt>" +
                    $"<dd>{model.Message}</dd>" +
                    $"</dl>" +
                    $"<p>¡Gracias por contactarnos! en breve nos comunicaremos contigo</p>" +
                    $"</div>";

                msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, to, subject, plainTextContent, htmlContent);
            }
            else
            {
                // Envíamos el mensaje sólo a InWork
                var to = new EmailAddress(defaultEmail, defaultName);

                var htmlContent =
                    $"<div>" +
                    $"<h4>Recibiste un nuevo mensaje de contacto</h4>" +
                    $"<dl>" +
                    $"<dt>Fecha y hora:</dt>" +
                    $"<dd>{model.RegisterDate}</dd>" +
                    $"<dt>Nombre:</dt>" +
                    $"<dd>{model.Name}</dd>" +
                    $"<dt>Correo electrónico:</dt>" +
                    $"<dd>{model.Email}</dd>" +
                    $"<dt>Asunto:</dt>" +
                    $"<dd>{model.Affair}</dd>" +
                    $"<dt>Mensaje:</dt>" +
                    $"<dd>{model.Message}</dd>" +
                    $"</dl>" +
                    $"</div>";

                msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            }

            var response = await client.SendEmailAsync(msg);

            return response;
        }

        private async Task<Response> SendSubscriptionEmail(MessageModel model)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(defaultEmail, defaultName);
            var subject = "Nueva suscripción";
            var plainTextContent = $"Gracias por suscribirte {model.Email}, a partir de este momento recibirás información de nuestros productos, servicios y publicaciones";
            SendGridMessage msg;

            if (allowEmailForwarding)
            {
                // Envíamos el mensaje tanto a InWork como al usuario
                var to = new List<EmailAddress>
                {
                    new EmailAddress(defaultEmail, defaultName),
                    new EmailAddress(model.Email)
                };

                var htmlContent =
                    $"<div>" +
                    $"<h4>¡Gracias por suscribirte!</h4>" +
                    $"<dl>" +
                    $"<dt>Folio:</dt>" +
                    $"<dd>{model.Id}</dd>" +
                    $"<dt>Correo electrónico:</dt>" +
                    $"<dd>{model.Email}</dd>" +
                    $"<dt>Fecha y hora:</dt>" +
                    $"<dd>{model.RegisterDate}</dd>" +
                    $"</dl>" +
                    $"<p>A partir de este momento recibirás información de nuestros productos, servicios y publicaciones.</p>" +
                    $"</div>";

                msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, to, subject, plainTextContent, htmlContent);
            }
            else
            {
                // Envíamos el mensaje sólo a InWork
                var to = new EmailAddress(defaultEmail, defaultName);

                var htmlContent =
                    $"<div>" +
                    $"<h4>Recibiste una nueva solicitud de suscripción</h4>" +
                    $"<dl>" +
                    $"<dt>Folio:</dt>" +
                    $"<dd>{model.Id}</dd>" +
                    $"<dt>Correo electrónico:</dt>" +
                    $"<dd>{model.Email}</dd>" +
                    $"<dt>Fecha y hora:</dt>" +
                    $"<dd>{model.RegisterDate}</dd>" +
                    $"</dl>" +
                    $"</div>";

                msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            }

            var response = await client.SendEmailAsync(msg);

            return response;
        }

        #endregion
    }
}