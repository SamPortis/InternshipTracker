using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipTracker.Data
{
    public class Company : BaseDomainEntity
    {
        [Required]
        public string? Name { get; set; }
    }
}
