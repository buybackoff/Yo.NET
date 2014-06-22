using ServiceImplementations.Common;

namespace WebHost
{
    public class Application : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AppStarter.Start();
        }

    }

}
