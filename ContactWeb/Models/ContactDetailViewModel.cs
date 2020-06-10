using System;

namespace ContactWeb.Models
{
    public class ContactDetailViewModel
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime Birthdate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Category { get; set; }
        public string PhotoUrl { get; set; }
    }
}
