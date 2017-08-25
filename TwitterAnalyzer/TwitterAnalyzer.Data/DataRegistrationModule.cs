using Autofac;
using TwitterAnalyzer.Data.Repositories;

namespace TwitterAnalyzer.Data
{
    public class DataRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TwitterAnalyzerDbContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ReportRepository>().As<IReportRepository>().InstancePerLifetimeScope();
        }
    }
}
