namespace PRN231_Project.Dto
{
    public class ContactDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
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
        public DateTime? TrashDate { get; set; }
    }
}
