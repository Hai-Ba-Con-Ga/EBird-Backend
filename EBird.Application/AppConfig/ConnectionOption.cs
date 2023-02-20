using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.AppConfig
{
    public class ConnectionOption
    {
        public static string ConnectionStrings = "ConnectionStrings" ;
        
        public ConnectionOption()
        {
           
        }

        public string DefaultConnection { get; set; } = "";
        public string CloudConnection { get; set; } = "";

    }
}
