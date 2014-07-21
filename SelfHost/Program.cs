//using Mono.Unix;
//using Mono.Unix.Native;
using System;
using System.Threading;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using SelfHost;
using Services;


[assembly: OwinStartup(typeof(Startup))]


namespace SelfHost {
    class Program {

        static void Main(string[] args) {
            //bool runningOnMono = Type.GetType("Mono.Runtime") != null;
            //Initialize app host
            var appHost = new AppHost();
            appHost.Init();
            appHost.Start("http://*:8888/api/");
            Console.WriteLine("ServiceStack running on http://*:8888/");

            WebApp.Start("http://*:8888/");
            //if (runningOnMono) {
            //    var signals = new UnixSignal[] {
            //        new UnixSignal(Signum.SIGINT),
            //        new UnixSignal(Signum.SIGTERM),
            //    };

            //    // Wait for a unix signal
            //    for (bool exit = false; !exit; ) {
            //        int id = UnixSignal.WaitAny(signals);

            //        if (id >= 0 && id < signals.Length) { if (signals[id].IsSet) exit = true; }
            //    }
            //} else { Console.ReadLine(); }
            //Thread.Sleep(100000);
            Console.ReadLine();
        }
    }

    class Startup {
        public void Configuration(IAppBuilder app) {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }

}