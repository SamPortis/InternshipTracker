using InternshipTracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace InternshipTracker.Pages.Applications
{
    public class IndexModel : PageModel
    {
        private readonly InternshipTracker.Data.InternshipTrackerDbContext _context;

        public IndexModel(InternshipTracker.Data.InternshipTrackerDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InternshipApplication InternshipApplication { get; set; } = default!;

        public IList<InternshipApplication> InternshipApplications { get;set; } = default!;
        public IList<InternshipApplication> TempList { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
           
        public int? CompanySearchId { get; set; }

        [TempData]
        public int OldCompanyId { get; set; }

        public async Task OnGetAsync()
        {
            if (string.IsNullOrEmpty(SearchString))
            {
                InternshipApplications = await _context.InternshipApplications
                    .Include(i => i.Company)
                    .Include(i => i.Status)
                    .ToListAsync();

                SortApplications();
            } else
            {
                IList<Company> companies = await _context.Companies.Where(i => i.Name.Contains(SearchString)).ToListAsync();

                InternshipApplications = new List<InternshipApplication>();

                if (companies.Count > 0)
                {
                    foreach (var company in companies)
                    {
                        TempList = await _context.InternshipApplications
                            .Where(i => i.CompanyId == company.Id)
                            .Include(i => i.Company)
                            .Include(i => i.Status)
                            .ToListAsync();

                        InternshipApplications.AddRange(TempList);
                    }
                }

                SortApplications();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internshipApplication = await _context.InternshipApplications.FindAsync(id);
            if (internshipApplication != null)
            {
                OldCompanyId = internshipApplication.CompanyId;

                _context.InternshipApplications.Remove(internshipApplication);
                await _context.SaveChangesAsync();

                //check database for remaining applications with OldCompanyId

                //if none, delete the company as well

                bool hasOtherCompanyEntries = _context.InternshipApplications.Any(e => e.CompanyId == OldCompanyId);

                if (!hasOtherCompanyEntries)
                {
                    var oldCompany = await _context.Companies.FindAsync(OldCompanyId);
                    if (oldCompany != null && oldCompany.Name != "Add Company")
                    {
                        _context.Companies.Remove(oldCompany);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostSetRejectedAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internshipApplication = await _context.InternshipApplications.FindAsync(id);
            if (internshipApplication != null)
            {
                InternshipApplication = internshipApplication;
                InternshipApplication.StatusId = 6; // Set status to Rejected
                _context.Attach(InternshipApplication).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            return NotFound(); //shoudlnt get this far
        }
        
        private void SortApplications()
        {
            InternshipApplications = InternshipApplications
                .OrderBy(i => i.Company.Name) // Primary sort by StatusId
                .ToList();
        }
    }
}
