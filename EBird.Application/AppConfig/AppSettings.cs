using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.AppConfig
{
    public class AppSettings
    {   
        public DbConfig ConnectionStringsOptions { get; set; }
        public JwtSetting JwtSetting { get; set; }
    }
}
