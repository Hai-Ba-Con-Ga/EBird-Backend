using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.AppConfig
{
    public class VnpayConfig
    {
        public static string VnpayConfigString = "VnpayConfig" ;
        
        public string ReturnUrl { get; set; }
        public string Url { get; set; }
        public string HashSecret { get; set; }
        public string TmnCode { get; set; }
        public string FrontendCallBack {get; set; }

        
    }
}
