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
        [Display(Name = "Mortgage Start Date")]
        public string StartDate { get; set; } 
        [Required(ErrorMessage = "NOPE")]
        [Display(Name = "Loan Value")]
        public decimal LoanValue { get; set; } 
        [Required]
        [Display(Name = "Mortgage Term")]
        public int TermYears { get; set; } 
        [Display(Name = "Standard Rate")]
        public decimal TermRate { get; set; } 
        [Display(Name = "Fixed Rate Period")]
        public int? FixedTermYears { get; set; } 
        [Display(Name = "Fixed Interest Rate")]
        public decimal? FixedTermRate { get; set; } 
        [Display(Name = "Regular Overpayment Interval")]
        public int? ExtraPaymentInterval { get; set; } 
        [Display(Name = "Regular Overpayment Amount")]
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