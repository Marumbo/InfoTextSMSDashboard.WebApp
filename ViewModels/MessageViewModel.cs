using InfoTextSMSDashboard.WebApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoTextSMSDashboard.WebApp.ViewModels
{
    public class MessageViewModel
    {

        public List<OutGoingSMSDTO> Messages { get; set; }

        public List<Contact> Contacts { get; set; }

        public string ApiResponse { get; set; }

        public string StatusCode { get; set; }
    }
}
