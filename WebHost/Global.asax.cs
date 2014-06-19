using ServiceImplementations.Shared;

namespace WebHost
{
    public class Application : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            new AppHost().Init();
        }

    }

}
