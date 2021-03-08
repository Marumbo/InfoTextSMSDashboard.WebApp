using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using InfoTextSMSDashboard.WebApp.DTOs;
using InfoTextSMSDashboard.WebApp.Models;
using InfoTextSMSDashboard.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace InfoTextSMSDashboard.WebApp.Controllers
{
    public class MessageController : Controller
    {
        public async Task<IActionResult> Index( )
        {
            // List<OutGoingSMSDTO> messageList = new List<OutGoingSMSDTO>();

            var messageView = new MessageViewModel { };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44382/api/sms/GetMessages"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    messageView.Messages= JsonConvert.DeserializeObject<List<OutGoingSMSDTO>>(apiResponse);
                }
            }


            return View(messageView);
        }

        // go to send message
        //[Authorize]
        public ActionResult CreateSMS()
        {
            var sms = new SMS();

            return View(sms);
        }
        [HttpPost("SendSMS")]
        [Route("/Message/SendSMS", Name = "SendSMS")]
        public async Task<IActionResult> SendSMS([FromForm]SMS sms)
        {

            //  var smsContent =  new StringContent(System.Text.Json.JsonSerializer.Serialize(sms), Encoding.UTF8, "application/json");


            var messageView = new MessageViewModel { };

            using (var httpClient = new HttpClient())
            {

                

                var content = new StringContent(JsonConvert.SerializeObject(sms), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:44382/api/sms/sendSMS", content))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"api response= {apiResponse}");

                        messageView.StatusCode = "true";
                        messageView.StatusCode.ToLower();

                        messageView.ApiResponse = "Sms was sent and saved in database.";

                    
                        
                           using (var getMessages = await httpClient.GetAsync("https://localhost:44382/api/sms/GetMessages"))
                            {
                                string messageList = await getMessages.Content.ReadAsStringAsync();
                                messageView.Messages = JsonConvert.DeserializeObject<List<OutGoingSMSDTO>>(messageList);
                            }
                        



                        return View("Index", messageView);
                    }
                    else
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                    
                        messageView.StatusCode = "false";
                        messageView.StatusCode.ToLower();

                        messageView.ApiResponse = "Sms did not send please try again later. Check balance, contact adming if problem persists.";

                        using (var getMessages = await httpClient.GetAsync("https://localhost:44382/api/sms/GetMessages"))
                        {
                            string messageList = await getMessages.Content.ReadAsStringAsync();
                            messageView.Messages = JsonConvert.DeserializeObject<List<OutGoingSMSDTO>>(messageList);
                        }


                      

                        return View("Index", messageView);
                    }

                }

            }


        }
    }
}