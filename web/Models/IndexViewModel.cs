using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        [Display(Name = "Extra payments reduce")]
        public AmortisationScheduleTypeViewModel AmortisationScheduleType { get; set; }

        public IEnumerable<SelectListItem> FixedRatePeriods =>
            new List<SelectListItem> {
                new SelectListItem { Value = string.Empty, Text = "No fixed rate period" },
                new SelectListItem { Value = "2", Text = "2 years" },
                new SelectListItem { Value = "5", Text = "5 years" },
                new SelectListItem { Value = "10", Text = "10 years" }
            };

        public IEnumerable<AmortisationScheduleEntryViewModel> ScheduleEntries { get; set; }
            = new List<AmortisationScheduleEntryViewModel>();

        public IList<double> OverPayments { get; set; }
            = new List<double>();

        public bool Valid { get; set; }

        public void PopulateFrom(MortgageData mortgageData)
        {
            // Set so that the form is initially hidden when we retrieve data from a user cookie
            Valid = true;

            StartDate = mortgageData.StartDate;
            LoanValue = mortgageData.LoanValue;
            TermYears = mortgageData.TermYears;
            TermRate = mortgageData.TermRate;
            FixedTermYears = mortgageData.FixedTermYears;
            FixedTermRate = mortgageData.FixedTermRate;
            AmortisationScheduleType = mortgageData.AmortisationScheduleType;
            OverPayments = mortgageData.OverPayments;
        }

        public MortgageData AsMortgageData() =>
            new MortgageData {
                StartDate = StartDate,
                LoanValue = LoanValue,
                TermYears = TermYears,
                TermRate = TermRate,
                FixedTermYears = FixedTermYears,
                FixedTermRate = FixedTermRate,
                AmortisationScheduleType = AmortisationScheduleType,
                OverPayments = OverPayments
            };
    }
}
