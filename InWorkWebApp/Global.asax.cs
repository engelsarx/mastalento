using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace InWorkWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            Console.WriteLine("Error capturado desde el método global (Application_Error)", ex);
            Server.ClearError();
            Response.Redirect("/Shared/Error");
        }
    }
}
