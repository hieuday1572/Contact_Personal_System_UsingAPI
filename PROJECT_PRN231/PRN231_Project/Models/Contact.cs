using System;
using System.Collections.Generic;

namespace PRN231_Project.Models
{
    public partial class Contact
    {
        public Contact()
        {
            ContactLabels = new HashSet<ContactLabel>();
        }

        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Company { get; set; }
        public string? JobTitle { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Note { get; set; }
        public bool? IsInTrash { get; set; }
        public int? UserId { get; set; }
        public int? VisitedCount { get; set; }
        public string? Address { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? FacebookLink { get; set; }
        public string? InstaLink { get; set; }
        public string? Banking { get; set; }
        public string? Image { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<ContactLabel> ContactLabels { get; set; }
    }
}
