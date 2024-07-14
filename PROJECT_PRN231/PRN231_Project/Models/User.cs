using System;
using System.Collections.Generic;

namespace PRN231_Project.Models
{
    public partial class User
    {
        public User()
        {
            Contacts = new HashSet<Contact>();
            Labels = new HashSet<Label>();
        }

        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Adress { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Image { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Label> Labels { get; set; }
    }
}
