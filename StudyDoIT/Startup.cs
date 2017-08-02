using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudyDoIT.Startup))]
namespace StudyDoIT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
