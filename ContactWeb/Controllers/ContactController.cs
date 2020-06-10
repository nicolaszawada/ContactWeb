using ContactWeb.Database;
using ContactWeb.Domain;
using ContactWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace ContactWeb.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactDatabase _contactDatabase;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ContactController(IContactDatabase contactDatabase, IWebHostEnvironment hostEnvironment)
        {
            _contactDatabase = contactDatabase;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Contact> contacts = _contactDatabase.GetContacts();
            List<ContactListViewModel> viewModel = new List<ContactListViewModel>();

            foreach (Contact contact in contacts)
            {
                viewModel.Add(new ContactListViewModel()
                {
                    Id = contact.Id,
                    FullName = $"{contact.FirstName} {contact.LastName}"
                });
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ContactCreateViewModel vm = new ContactCreateViewModel();
            vm.Birthdate = new DateTime(1990, 1, 1);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(ContactCreateViewModel vm)
        {
            if (!TryValidateModel(vm))
            {
                return View(vm);
            }

            Contact newContact = new Contact()
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Birthdate = vm.Birthdate,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber,
                Address = vm.Address,
                Category = vm.Category
            };

            if (vm.Photo != null)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.Photo.FileName);
                string pathName = Path.Combine(_hostEnvironment.WebRootPath, "photos");
                string fileNameWithPath = Path.Combine(pathName, uniqueFileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    vm.Photo.CopyTo(stream);
                }

                newContact.PhotoUrl = "/photos/" + uniqueFileName;
            }

            _contactDatabase.Insert(newContact);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            Contact contact = _contactDatabase.GetContact(id);
            ContactDetailViewModel vm = new ContactDetailViewModel()
            {
                FullName = $"{contact.FirstName} {contact.LastName}",
                Address = contact.Address,
                Birthdate = contact.Birthdate,
                Category = contact.Category,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                PhotoUrl = contact.PhotoUrl
            };

            return View(vm);
        }
    }
}
