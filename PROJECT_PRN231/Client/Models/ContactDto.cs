using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class ContactDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name can't be longer than 100 characters.")]
        public string FullName { get; set; } = "";

        [StringLength(100, ErrorMessage = "Company name can't be longer than 100 characters.")]
        public string? Company { get; set; }

        [StringLength(100, ErrorMessage = "Job title can't be longer than 100 characters.")]
        public string? JobTitle { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(100, ErrorMessage = "Email can't be longer than 100 characters.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(15, ErrorMessage = "Phone number can't be longer than 15 characters.")]
        public string? Phone { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(500, ErrorMessage = "Note can't be longer than 500 characters.")]
        public string? Note { get; set; }

        public bool? IsInTrash { get; set; }

        public int? UserId { get; set; }

        public int? VisitedCount { get; set; }

        [StringLength(200, ErrorMessage = "Address can't be longer than 200 characters.")]
        public string? Address { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedDate { get; set; }

        [Url(ErrorMessage = "Invalid URL.")]
        [StringLength(200, ErrorMessage = "Facebook link can't be longer than 200 characters.")]
        public string? FacebookLink { get; set; }

        [Url(ErrorMessage = "Invalid URL.")]
        [StringLength(200, ErrorMessage = "Instagram link can't be longer than 200 characters.")]
        public string? InstaLink { get; set; }

        [StringLength(100, ErrorMessage = "Banking information can't be longer than 100 characters.")]
        public string? Banking { get; set; }

        [StringLength(200, ErrorMessage = "Image link can't be longer than 200 characters.")]
        public string? Image { get; set; }
        public DateTime? TrashDate
        {
            get; set;

        }
    }
}
