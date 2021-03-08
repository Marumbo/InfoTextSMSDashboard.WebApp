using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InfoTextSMSDashboard.WebApp.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InfoTextSMSDashboard.WebApp.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public async Task<IActionResult> Index()
        {
            List<Contact> contactList = new List<Contact>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44382/api/contacts/GetAllContacts"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    contactList = JsonConvert.DeserializeObject<List<Contact>>(apiResponse);
                }
            }

            return View(contactList);
        }
        public async Task<IActionResult> DataTable()
        {
            List<Contact> contactList = new List<Contact>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44382/api/contacts/GetAllContacts"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    contactList = JsonConvert.DeserializeObject<List<Contact>>(apiResponse);
                }
            }

            return View(contactList);
        }

        // GET: Contact/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var contact = new Contact();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:44382/api/contacts/GetContactById/?id={id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        contact = JsonConvert.DeserializeObject<Contact>(apiResponse);
                        return View(contact);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));

                    }
                }

            }

           
        }

        // GET: Contact/Create
        public ActionResult Create()
        {
            var contact = new Contact();

            return View(contact);
        }


        [HttpPost]
        [Route("/Contact/Save", Name = "SaveContact")]
        public async Task<IActionResult> Save([FromForm]Contact contact)
        {

            if (contact.ContactId == null)
            {

                using (var httpClient = new HttpClient())
                {

                    var contactDTO = new Contact
                    {
                       FirstName = contact.FirstName,
                       LastName = contact.LastName,
                       EmailAddress = contact.EmailAddress,
                       Organization = contact.Organization,
                       PhoneNumber = contact.PhoneNumber,
                       AddedBy = "Test addition"
                    };

                    StringContent content = new StringContent(JsonConvert.SerializeObject(contactDTO), Encoding.UTF8, "application/json");


                    using (var response = await httpClient.PostAsync("https://localhost:44382/api/contacts/AddContact", content))
                    {

                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            Debug.WriteLine($"api response= {apiResponse}");

                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {

                            Debug.WriteLine("Didnt save");
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            Debug.WriteLine($"api response= {apiResponse}");

                            return RedirectToAction(nameof(Index));
                        }

                    }

                }
            }

            if (contact.ContactId != 0)
            {
                using (var httpClient = new HttpClient())
                {

                    var contactDTO = new Contact
                    {
                        ContactId= contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        PhoneNumber = contact.PhoneNumber,
                        EmailAddress = contact.EmailAddress,
                        Organization = contact.Organization,
                        DateAdded = contact.DateAdded,
                        AddedBy = contact.AddedBy,
                        GroupId = contact.GroupId
                    };

                    StringContent content = new StringContent(JsonConvert.SerializeObject(contactDTO), Encoding.UTF8, "application/json");


                    using (var response = await httpClient.PutAsync("https://localhost:44382/api/contacts/UpdateContact", content))
                    {

                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            Debug.WriteLine($"api response= {apiResponse}");

                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            Debug.WriteLine("Didnt save");
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            Debug.WriteLine($"api response= {apiResponse}");

                            return RedirectToAction(nameof(Index));
                        }

                    }

                }
            }
            else
                return RedirectToAction(nameof(Index));


        }
      
        // GET: Contact/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var contact = new Contact();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:44382/api/contacts/GetContactById/?id={id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        contact = JsonConvert.DeserializeObject<Contact>(apiResponse);
                        return View(contact);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));

                    }
                }

            }
        }

       
        // GET: Contact/Delete/5
    
        public async Task<IActionResult> Delete(int id)
        {
            var contact = new Contact();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:44382/api/contacts/GetContactById/?id={id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        contact = JsonConvert.DeserializeObject<Contact>(apiResponse);
                        return View(contact);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));

                    }
                }

            }
            
        }

        // POST: Contact/Delete/5
        [HttpPost("DeleteContact")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = new Contact();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:44382/api/contacts/GetContactById/?id={id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        contact = JsonConvert.DeserializeObject<Contact>(apiResponse);
                        return View(contact);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));

                    }
                }

            }

        }
    }
}