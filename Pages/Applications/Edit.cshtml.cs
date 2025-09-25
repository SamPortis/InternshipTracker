using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InternshipTracker.Data;

namespace InternshipTracker.Pages.Applications
{
    public class EditModel : PageModel
    {
        private readonly InternshipTracker.Data.InternshipTrackerDbContext _context;

        public EditModel(InternshipTracker.Data.InternshipTrackerDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InternshipApplication InternshipApplication { get; set; } = default!;

        [TempData]
        public int OldCompanyId { get; set; }

        public SelectList Companies { get; set; } = default!;
        public SelectList Statuses { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internshipapplication =  await _context.InternshipApplications.FirstOrDefaultAsync(m => m.Id == id);

            if (internshipapplication == null)
            {
                return NotFound();
            }

            InternshipApplication = internshipapplication;
            OldCompanyId = InternshipApplication.CompanyId;

            Companies = new SelectList(_context.Companies, "Id", "Name");
            Statuses = new SelectList(_context.Statuses, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Companies = new SelectList(_context.Companies, "Id", "Name");
            Statuses = new SelectList(_context.Statuses, "Id", "Name");

            ModelState.Remove("InternshipApplication.Company.Name");
            ModelState.Remove("InternshipApplication.Company");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            InternshipApplication.Company = null;
            _context.Attach(InternshipApplication).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InternshipApplicationExists(InternshipApplication.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            if (InternshipApplication.CompanyId != OldCompanyId)
            {
                bool HasOtherCompanyEntries = _context.InternshipApplications.Any(e => e.CompanyId == OldCompanyId);

                if (!HasOtherCompanyEntries)
                {
                    //delete old id from database
                    var OldCompany = new Company { Id = OldCompanyId };
                    _context.Companies.Remove(OldCompany);

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!InternshipApplicationExists(InternshipApplication.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }

            return RedirectToPage("./Index");
        }

        private bool InternshipApplicationExists(int id)
        {
            return _context.InternshipApplications.Any(e => e.Id == id);
        }
    }
}
