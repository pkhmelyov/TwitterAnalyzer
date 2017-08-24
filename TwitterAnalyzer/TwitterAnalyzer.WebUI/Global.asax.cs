using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using TwitterAnalyzer.WebUI.Infrastructure;
using LinqToTwitter;
using Microsoft.AspNet.Identity.Owin;
using TwitterAnalyzer.WebUI.Domain;

namespace TwitterAnalyzer.WebUI
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SetupDI();
        }

        private void SetupDI()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModule<AutofacWebTypesModule>();

            builder.Register(
                context =>
                    context.Resolve<HttpContextBase>()
                        .GetOwinContext()
                        .GetUserManager<ApplicationUserManager>())
                .AsSelf()
                .ExternallyOwned();

            builder.Register(
                context =>
                    context.Resolve<HttpContextBase>()
                        .GetOwinContext()
                        .Get<ApplicationSignInManager>())
                .AsSelf()
                .ExternallyOwned();

            builder.RegisterType<FixedSessionStateCredentialStore>().As<ICredentialStore>().InstancePerLifetimeScope();

            builder.Register(context => new MvcSignInAuthorizer {CredentialStore = context.Resolve<ICredentialStore>()})
                .As<IAuthorizer>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<TwitterContext>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<ReportBuilder>().As<IReportBuilder>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
