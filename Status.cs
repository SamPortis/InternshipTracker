using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipTracker.Data
{
    public class Status
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
