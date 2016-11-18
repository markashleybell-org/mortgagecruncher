using Microsoft.AspNetCore.Mvc.Rendering;
using mortgagecruncher.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mortgagecruncher.ViewModels
{
    public class IndexViewModel
    {
        [Required]
        [Display(Name = "Date of first payment")]
        public string StartDate { get; set; } 
        [Required(ErrorMessage = "NOPE")]
        [Display(Name = "Loan value")]
        public decimal LoanValue { get; set; } 
        [Required]
        [Display(Name = "Mortgage term")]
        public int TermYears { get; set; } 
        [Display(Name = "Standard interest rate")]
        public decimal TermRate { get; set; } 
        [Display(Name = "Fixed rate period")]
        public int? FixedTermYears { get; set; } 
        [Display(Name = "Fixed interest rate")]
        public decimal? FixedTermRate { get; set; } 
        [Display(Name = "Regular overpayments?")]
        public int? OverpaymentInterval { get; set; } 
        [Display(Name = "Overpayment start date")]
        public string OverpaymentStartDate { get; set; } 
        [Display(Name = "Overpayment amount")]
        public decimal? OverpaymentAmount { get; set; } 
        [Display(Name = "Overpayments reduce")]
        public AmortisationScheduleType AmortisationScheduleType { get; set; }  

        public IEnumerable<SelectListItem> OverpaymentIntervals { get; set; }
        public IEnumerable<SelectListItem> FixedRatePeriods { get; set; }

        public IEnumerable<AmortisationScheduleEntry> ScheduleEntries { get; set; }

        public bool Valid { get; set; }

        public IndexViewModel()
        {
            OverpaymentIntervals = new List<SelectListItem> {
                new SelectListItem { Value = "", Text = "None" },
                new SelectListItem { Value = "1", Text = "Monthly" },
                new SelectListItem { Value = "12", Text = "Yearly" },
                new SelectListItem { Value = "2", Text = "Every 2 months" },
                new SelectListItem { Value = "3", Text = "Every 3 months" },
                new SelectListItem { Value = "4", Text = "Every 4 months" },
                new SelectListItem { Value = "5", Text = "Every 5 months" },
                new SelectListItem { Value = "6", Text = "Every 6 months" },
                new SelectListItem { Value = "7", Text = "Every 7 months" },
                new SelectListItem { Value = "8", Text = "Every 8 months" },
                new SelectListItem { Value = "9", Text = "Every 9 months" },
                new SelectListItem { Value = "10", Text = "Every 10 months" },
                new SelectListItem { Value = "11", Text = "Every 11 months" }
            };

            FixedRatePeriods = new List<SelectListItem> {
                new SelectListItem { Value = "", Text = "No fixed rate period" },
                new SelectListItem { Value = "2", Text = "2 years" },
                new SelectListItem { Value = "5", Text = "5 years" },
                new SelectListItem { Value = "10", Text = "10 years" }
            };

            ScheduleEntries = new List<AmortisationScheduleEntry>();
        }
    }
}