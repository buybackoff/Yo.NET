//using System;
////using Mono.Unix;
////using Mono.Unix.Native;
//using System.Threading;

//namespace Services {
//    class Program {

//        static void Main(string[] args) {
//            //bool runningOnMono = Type.GetType("Mono.Runtime") != null;
//            //Initialize app host
//            var appHost = new AppHost();
//            appHost.Init();
//            appHost.Start("http://*:8888/api/");
//            Console.WriteLine("ServiceStack running on http://*:8888/");


//            //if (runningOnMono) {
//            //    var signals = new UnixSignal[] {
//            //        new UnixSignal(Signum.SIGINT),
//            //        new UnixSignal(Signum.SIGTERM),
//            //    };

//            //    // Wait for a unix signal
//            //    for (bool exit = false; !exit; ) {
//            //        int id = UnixSignal.WaitAny(signals);

//            //        if (id >= 0 && id < signals.Length) { if (signals[id].IsSet) exit = true; }
//            //    }
//            //} else { Console.ReadLine(); }
//            Thread.Sleep (100000);
//            Console.ReadLine();
//        }
//    }

//}