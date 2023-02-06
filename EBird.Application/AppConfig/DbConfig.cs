using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.AppConfig
{
    public class DbConfig
    {
        public string DefaultConnection { get; set; }
        public string CloudConnection { get; set; }
    }
}
