using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TwitterAnalyzer.WebUI.Startup))]
namespace TwitterAnalyzer.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
