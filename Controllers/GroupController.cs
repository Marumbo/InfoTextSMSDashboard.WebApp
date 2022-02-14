using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InfoTextSMSDashboard.DataModels.Models;
using InfoTextSMSDashboard.WebApp.DTOs;
using InfoTextSMSDashboard.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InfoTextSMSDashboard.WebApp.Controllers
{
    public class GroupController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<GroupsDTO> groups = new List<GroupsDTO>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44382/api/group/getallgroups"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    groups = JsonConvert.DeserializeObject<List<GroupsDTO>>(apiResponse);
                }
            }

            return View(groups);
        }

        public ActionResult Create()
        {
            var group = new GroupsDTO();

            return View(group);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GroupsDTO group)
        {
            using (var httpClient = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(group), Encoding.UTF8, "application/json");


                using (var response = await httpClient.PostAsync("https://localhost:44382/api/group/AddGroup", content))
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

        [HttpPost]
        public async Task<IActionResult> AddContactsToGroup([FromForm] List<ContactDTOMVC> contacts)
        {

            var groupContactViewModel = new GroupContactViewModel()
            {
                Contacts = new List<ContactDTOMVC>()
            };


            List<GroupsDTOMVC> groups = new List<GroupsDTOMVC>();



            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44382/api/group/getallgroups"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    groups = JsonConvert.DeserializeObject<List<GroupsDTOMVC>>(apiResponse);
                }
            }
            foreach (var contact in contacts)
            {
                if (contact.IsChecked)
                {
                    groupContactViewModel.Contacts?.Add(contact);
                }
            }

            groupContactViewModel.Groups = groups;

            return View(groupContactViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ContactsToGroup(GroupContactViewModel groupContactViewModel)
        {
            var groupContactList = new GroupContactListDTO()
            {
                GroupId = 0,
                ContactIds = new List<int>()
            };

            foreach (var group in groupContactViewModel.Groups)
            {

                groupContactList.GroupId = group.GroupId;
            }

            foreach (var contact in groupContactViewModel.Contacts)
            {
                groupContactList.ContactIds.Add((int)contact.ContactId);
            }

            using (var httpClient = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(groupContactList), Encoding.UTF8, "application/json");


                using (var response = await httpClient.PostAsync("https://localhost:44382/api/GroupContact/AddContactToGroup", content))
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

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var groupContactViewModel = new GroupContactViewModel()
            {
                Groups = new List<GroupsDTOMVC>(),
                Contacts = new List<ContactDTOMVC>()
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:44382/api/GroupContact/GetContactInGroupId/?id={id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        groupContactViewModel.Contacts = JsonConvert.DeserializeObject<List<ContactDTOMVC>>(apiResponse);
                    }
                }
            }

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:44382/api/group/GetGroupById/?id={id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var group = JsonConvert.DeserializeObject<GroupsDTOMVC>(apiResponse);
                        groupContactViewModel.Groups.Add(group);
                    }
                }
            }
            return View(groupContactViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var groupContactViewModel = new GroupContactViewModel()
            {
                Groups = new List<GroupsDTOMVC>(),
                Contacts = new List<ContactDTOMVC>()
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:44382/api/GroupContact/GetContactInGroupId/?id={id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        groupContactViewModel.Contacts = JsonConvert.DeserializeObject<List<ContactDTOMVC>>(apiResponse);
                    }
                }
            }

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:44382/api/group/GetGroupById/?id={id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var group = JsonConvert.DeserializeObject<GroupsDTOMVC>(apiResponse);
                        groupContactViewModel.Groups.Add(group);
                    }
                }
            }
            return View(groupContactViewModel);
        }

        [HttpPost]
        [Route("/Group/EditGroup", Name ="EditGroup")]
        public async Task<IActionResult> EditGroup ([FromForm]GroupsDTOMVC group)
        {

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(group), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"https://localhost:44382/api/group/UpdateGroup", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        //Todo: add handling of response message
                    }
                    else
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        //Todo: Add handling of errr message
                    }
                }
            }


            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [Route("/Group/DeleteContact/", Name = "GroupDeleteContact")]
        public async Task<IActionResult> DeleteContact(int contactId, int groupId)
        {
            var groupContact = new GroupContactDTO()
            {
                GroupId = groupId,
                ContactId = contactId
            };

            using (var httpClient = new HttpClient())
            {
               

                using (var response = await httpClient.DeleteAsync($"https://localhost:44382/api/groupcontact/DeleteContactInGroup/?contactId={contactId}/?groupId={groupId}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        //Todo: add handling of response message
                    }
                    else
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        //Todo: Add handling of errr message
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
        }

        public async Task<IActionResult>Delete(int id)
        {

            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.DeleteAsync($"https://localhost:44382/api/group/DeleteGroup/?Id={id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        //Todo: add handling of response message
                    }
                    else
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        //Todo: Add handling of errr message
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
        }

    }
}