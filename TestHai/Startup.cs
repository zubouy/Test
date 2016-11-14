using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestHai.Startup))]
namespace TestHai
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
