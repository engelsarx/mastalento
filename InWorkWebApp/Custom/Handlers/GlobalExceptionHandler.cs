using log4net;
using System;
using System.Web.Mvc;

namespace InWorkWebApp.Custom.Handlers
{
    public class GlobalExceptionHandler : HandleErrorAttribute
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(GlobalExceptionHandler));

        public override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            var error = $"Lo sentimos, algo salió mal y no pudimos completar tu petición. Detalle: {ex.Message} - {ex.TargetSite}";

            LOGGER.Error(error, ex);

            var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");

            var data = new TempDataDictionary
            {
                { "ErrorMessage", error }
            };

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model),
                TempData = data
            };
        }
    }
}