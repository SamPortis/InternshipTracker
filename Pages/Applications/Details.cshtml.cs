using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InternshipTracker.Data;

namespace InternshipTracker.Pages.Applications
{
    public class DetailsModel : PageModel
    {
        private readonly InternshipTracker.Data.InternshipTrackerDbContext _context;

        public DetailsModel(InternshipTracker.Data.InternshipTrackerDbContext context)
        {
            _context = context;
        }

        public InternshipApplication InternshipApplication { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internshipapplication = await _context.InternshipApplications
                .Include(i => i.Company)
                .Include(i => i.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (internshipapplication == null)
            {
                return NotFound();
            }
            else
            {
                InternshipApplication = internshipapplication;
            }
            return Page();
        }
    }
}
