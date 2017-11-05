using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace web.Models
{
    public class IndexViewModel
    {
        [Required]
        [Display(Name = "Date of first payment")]
        public string StartDate { get; set; }
        [Display(Name = "Loan value")]
        public double LoanValue { get; set; }
        [Display(Name = "Mortgage term")]
        public int TermYears { get; set; }
        [Display(Name = "Standard interest rate")]
        public double TermRate { get; set; }
        [Display(Name = "Fixed rate period")]
        public int? FixedTermYears { get; set; }
        [Display(Name = "Fixed interest rate")]
        public double? FixedTermRate { get; set; }
        [Display(Name = "Overpayments reduce")]
        public AmortisationScheduleTypeViewModel AmortisationScheduleType { get; set; }

        public IEnumerable<SelectListItem> FixedRatePeriods { get; set; }

        public IEnumerable<AmortisationScheduleEntryViewModel> ScheduleEntries { get; set; }

        public IList<double> OverPayments { get; set; }

        public bool Valid { get; set; }

        public IndexViewModel()
        {
            FixedRatePeriods = new List<SelectListItem> {
                new SelectListItem { Value = "", Text = "No fixed rate period" },
                new SelectListItem { Value = "2", Text = "2 years" },
                new SelectListItem { Value = "5", Text = "5 years" },
                new SelectListItem { Value = "10", Text = "10 years" }
            };

            ScheduleEntries = new List<AmortisationScheduleEntryViewModel>();

            OverPayments = new List<double>();
        }
    }
}