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

        [HttpPost]
        [Route("GetContactDatatableData")]
        public async Task<IActionResult> GetContactDatatableData()
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

            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var customerData = (from tempcustomer in contactList select tempcustomer);
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
               // customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                customerData = contactList.Where(m => m.FirstName.Contains(searchValue)
                                            || m.LastName.Contains(searchValue)
                                            || m.EmailAddress.Contains(searchValue));
                                           
            }
            recordsTotal = customerData.Count();
            var data = customerData.Skip(skip).Take(pageSize).ToList();
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(jsonData);




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