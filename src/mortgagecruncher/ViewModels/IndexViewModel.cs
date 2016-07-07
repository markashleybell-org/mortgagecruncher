using Microsoft.AspNetCore.Mvc.Rendering;
using mortgagecruncher.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        [Display(Name = "Regular overpayment interval")]
        public int? ExtraPaymentInterval { get; set; } 
        [Display(Name = "Regular overpayment amount")]
        public decimal? ExtraPaymentAmount { get; set; } 

        public string[] MonthNames { get; private set; }

        public IEnumerable<AmortisationScheduleEntry> ScheduleEntries { get; set; }

        public IndexViewModel()
        {
            MonthNames = new[] {
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December"
            };

            ScheduleEntries = new List<AmortisationScheduleEntry>();
        }
    }
}