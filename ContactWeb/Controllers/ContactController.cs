using ContactWeb.Database;
using ContactWeb.Domain;
using ContactWeb.Models;
using ContactWeb.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private readonly PhotoService _photoService;

        public ContactController(IContactDatabase contactDatabase, IWebHostEnvironment hostEnvironment)
        {
            _contactDatabase = contactDatabase;
            _hostEnvironment = hostEnvironment;
            _photoService = new PhotoService();
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
                string uniqueFileName = UploadContactPhoto(vm.Photo);
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Contact contact = _contactDatabase.GetContact(id);

            ContactEditViewModel vm = new ContactEditViewModel()
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Address = contact.Address,
                Email = contact.Email,
                Birthdate = contact.Birthdate,
                Category = contact.Category,
                PhoneNumber = contact.PhoneNumber,
                PhotoUrl = contact.PhotoUrl
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Contact contact = _contactDatabase.GetContact(id);
            ContactDeleteViewModel vm = new ContactDeleteViewModel()
            {
                Id = contact.Id,
                Category = contact.Category,
                FullName = $"{contact.FirstName} {contact.LastName}"
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            Contact contact = _contactDatabase.GetContact(id);

            if (!string.IsNullOrEmpty(contact.PhotoUrl))
            {
                _photoService.DeletePicture(_hostEnvironment.WebRootPath, contact.PhotoUrl);
            }

            _contactDatabase.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(int id, ContactEditViewModel vm)
        {
            if (!TryValidateModel(vm))
            {
                return View(vm);
            }

            Contact contact = new Contact()
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Address = vm.Address,
                Birthdate = vm.Birthdate,
                Category = vm.Category,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber
            };

            Contact bestaandeContactUitDatabase = _contactDatabase.GetContact(id);

            if (vm.Photo == null)
            {
                contact.PhotoUrl = bestaandeContactUitDatabase.PhotoUrl;
            }
            else
            {
                if (!string.IsNullOrEmpty(bestaandeContactUitDatabase.PhotoUrl))
                {
                    _photoService.DeletePicture(_hostEnvironment.WebRootPath, bestaandeContactUitDatabase.PhotoUrl);
                }

                string uniqueFileName = UploadContactPhoto(vm.Photo);
                contact.PhotoUrl = "/photos/" + uniqueFileName;
            }

            _contactDatabase.Update(id, contact);

            return RedirectToAction("Detail", new { Id = id });
        }



        private string UploadContactPhoto(IFormFile photo)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            string pathName = Path.Combine(_hostEnvironment.WebRootPath, "photos");
            string fileNameWithPath = Path.Combine(pathName, uniqueFileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                photo.CopyTo(stream);
            }

            return uniqueFileName;
        }
    }
}
