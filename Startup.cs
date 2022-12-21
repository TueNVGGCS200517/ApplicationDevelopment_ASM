using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GreenwichBooks.Startup))]
namespace GreenwichBooks
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
