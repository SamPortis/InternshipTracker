using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipTracker.Data
{
    public class InternshipApplication : BaseDomainEntity
    {
        public InternshipApplication()
        {
            Company = new Company();
        }

        public int CompanyId { get; set; } //foreign key to Company

        public virtual Company? Company { get; set; } //navigation property to Company

        [Required]
        public int StatusId { get; set; }

        public Status? Status { get; set; }

        [Required]
        public string? Position { get; set; }

        public string? Notes { get; set; }

        public bool NeedToTakeOA { get; set; }

    }
}
