using System;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AngOE.API.Installer;
using AngOE.Common;
using AngOE.Common.Logging;
using Logger = AngOE.Common.Logging.Logger;

namespace AngOE.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WindsorBootstrapper.Initialize();

            // Configurate WebApi
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator),
                new WindsorCompositionRoot(WindsorBootstrapper.Container));
            GlobalConfiguration.Configuration.DependencyResolver = 
                new WindsorDependencyResolver(WindsorBootstrapper.Container);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Logger.Info("=====Application Start at:{0}=====", DateTime.Now);
        }

        protected void Application_End()
        {
            Logger.Info("=====Application Stop at:{0}=====", DateTime.Now);
        }
    }
}
