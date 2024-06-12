using System;
using System.Collections.Generic;

namespace PRN231_Project.Models
{
    public partial class ContactLabel
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public int LabelId { get; set; }

        public virtual Contact Contact { get; set; } = null!;
        public virtual Label Label { get; set; } = null!;
    }
}
