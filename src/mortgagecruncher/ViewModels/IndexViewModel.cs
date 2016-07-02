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
        [Display(Name = "Term Start Date")]
        public string StartDate { get; set; } 
        [Required(ErrorMessage = "NOPE")]
        [Display(Name = "Loan Value")]
        public decimal LoanValue { get; set; } 
        [Required]
        [Display(Name = "Term Years")]
        public int TermYears { get; set; } 
        [Display(Name = "Term Rate")]
        public decimal TermRate { get; set; } 
        [Display(Name = "Fixed Term Years")]
        public int? FixedTermYears { get; set; } 
        [Display(Name = "Fixed Term Rate")]
        public decimal? FixedTermRate { get; set; } 
        [Display(Name = "Extra Payment Interval")]
        public int? ExtraPaymentInterval { get; set; } 
        [Display(Name = "Extra Payment Amount")]
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