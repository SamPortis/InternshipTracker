using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InternshipTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace InternshipTracker.Pages.Applications
{
    public class CreateModel : PageModel
    {
        private readonly InternshipTracker.Data.InternshipTrackerDbContext _context;

        public CreateModel(InternshipTracker.Data.InternshipTrackerDbContext context)
        {
            _context = context;
        }

        public SelectList Companies { get; set; } = default!;
        public SelectList Statuses { get; set; } = default!;

        public async Task<IActionResult> OnGet()
        {
            await LoadInitalData();
            return Page();
        }

        [BindProperty]
        public InternshipApplication InternshipApplication { get; set; } = default!;

        public SelectList StatusList { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadInitalData();

            var company = await _context.Companies.FindAsync(InternshipApplication.CompanyId);
            InternshipApplication.Company = company;

            ModelState.Remove("InternshipApplication.Company.Name");
             
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InternshipApplications.Add(InternshipApplication);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostAddApplicationAsync()
        {
            await LoadInitalData();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!CompanyAlreadyUploaded(InternshipApplication.Company.Name))
            {
                _context.Companies.Add(InternshipApplication.Company);
                await _context.SaveChangesAsync();

                InternshipApplication.CompanyId = InternshipApplication.Company.Id;
            } else
            {
                InternshipApplication.Company = await _context.Companies.FirstOrDefaultAsync(c => c.Name == InternshipApplication.Company.Name);
                InternshipApplication.CompanyId = InternshipApplication.Company.Id;
            }

            _context.InternshipApplications.Add(InternshipApplication);
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }

        private bool CompanyAlreadyUploaded(string name)
        {
            return _context.Companies.Any(e => e.Name == name);
        }

        private async Task LoadInitalData()
        {
            Companies = new SelectList(await _context.Companies.ToListAsync(), "Id", "Name");
            Statuses = new SelectList(await _context.Statuses.ToListAsync(), "Id", "Name");
            StatusList = new SelectList(await _context.Statuses.ToListAsync(), "Id", "Name");
        }
    }
}
