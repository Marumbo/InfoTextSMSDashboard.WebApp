using InfoTextSMSDashboard.DataModels.Models;
using InfoTextSMSDashboard.WebApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoTextSMSDashboard.WebApp.ViewModels
{
    public class GroupContactViewModel
    {
        public List<GroupsDTOMVC> Groups { get; set; }

        public List<ContactDTOMVC> Contacts { get; set; }

        public string ApiResponse { get; set; }

        public string StatusCode { get; set; }
    }
}
