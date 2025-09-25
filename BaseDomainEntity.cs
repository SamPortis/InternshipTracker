using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipTracker.Data
{
    public class BaseDomainEntity
    {
        [Required]
        public int Id { get; set; } //primary key

        [Required]
        public DateOnly DateCreated { get; set; } = DateOnly.FromDateTime(DateTime.Now); //date created
    }
}
