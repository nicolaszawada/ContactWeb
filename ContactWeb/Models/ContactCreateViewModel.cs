using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ContactWeb.Models
{
    public class ContactCreateViewModel
    {
        [DisplayName("First name*")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Verplicht")]
        [MaxLength(25, ErrorMessage = "Voornaam mag slechts 25 karakters lang zijn")]
        public string FirstName { get; set; }

        [DisplayName("Last name*")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Verplicht")]
        [MaxLength(25, ErrorMessage = "Achternaam mag slechts 25 karakters lang zijn")]
        public string LastName { get; set; }
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

        [DisplayName("Photo")]
        public IFormFile Photo { get; set; }
    }
}
