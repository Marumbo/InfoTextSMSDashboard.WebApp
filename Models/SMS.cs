using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoTextSMSDashboard.WebApp.Models
{
    public class SMS
    {
        public List<string> Recipients { get; set; }
        public string Message { get; set; }
        public string From { get; set; }
    }
}
