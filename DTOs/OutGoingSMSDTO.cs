using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfoTextSMSDashboard.WebApp.DTOs
{
    public class OutGoingSMSDTO
    {
        public int? SmsId { get; set; }

        [Required(ErrorMessage = "Message cannot be left blank.")]
        [MaxLength(160, ErrorMessage = "Maximum length of acceptable characters for message is 160 per message.")]
        public string Message { get; set; }

        [Required(ErrorMessage = "User name sending message needed.")]
        [MaxLength(30, ErrorMessage = "Maximum length of acceptable characters for message is 30 for username.")]
        public string SenderUsername { get; set; }

        public DateTime? CreatedOn { get; set; }

        [Required(ErrorMessage = "Phone number needed for message to be sent.")]
        [MaxLength(40, ErrorMessage = "Maximum length of phone number is 40.")]
        public string RecipientNumber { get; set; }

        [Required(ErrorMessage = "message status from at response required.")]
        public string RecipientStatus { get; set; }

        [Required(ErrorMessage = "AT generated id needed.")]
        public string AtMessageid { get; set; }

        [Required(ErrorMessage = "cost sms required.")]
        public string MessageCost { get; set; }
    }
}
