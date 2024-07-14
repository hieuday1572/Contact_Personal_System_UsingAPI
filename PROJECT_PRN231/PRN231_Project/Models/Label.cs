using System;
using System.Collections.Generic;

namespace PRN231_Project.Models
{
    public partial class Label
    {
        public Label()
        {
            ContactLabels = new HashSet<ContactLabel>();
        }

        public int Id { get; set; }
        public string? LabelName { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<ContactLabel> ContactLabels { get; set; }
    }
}
