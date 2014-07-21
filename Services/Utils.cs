using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace Services {
    static class Utils {
        public static bool IsRunningOnAWS() {
            try {
                var wr = WebRequest.Create("http://169.254.169.254/latest/meta-data/");
                wr.Timeout = 250;
                wr.Method = "GET";
                var resp = wr.GetResponse();
                return !resp.IsErrorResponse();
            } catch {
                return false;
            }
        }
    }
}
