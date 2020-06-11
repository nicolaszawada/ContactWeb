using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ContactWeb.Models
{
    public class ContactEditViewModel
    {
        [DisplayName("First name*")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Verplicht")]
        [MaxLength(25, ErrorMessage = "Voornaam mag slechts 25 karakters lang zijn")]
        public string FirstName { get; set; }

        [DisplayName("Last name*")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Verplicht")]
        [MaxLength(25, ErrorMessage = "Achternaam mag slechts 25 karakters lang zijn")]
        public string LastName { get; set; }

        [DisplayName("Birthdate")]
        public DateTime Birthdate { get; set; }

        [DisplayName("Email Address")]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Category")]
        public string Category { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>()
        {
            new SelectListItem(){Text = "Family", Value = "Family"},
            new SelectListItem(){Text = "Colleague", Value = "Colleague"},
            new SelectListItem(){Text = "Friend", Value = "Friend"},
            new SelectListItem(){Text = "Enemy", Value = "Enemy"},
        };

        [DisplayName("Photo")]
        public IFormFile Photo { get; set; }

        public string PhotoUrl { get; set; }
    }
}
