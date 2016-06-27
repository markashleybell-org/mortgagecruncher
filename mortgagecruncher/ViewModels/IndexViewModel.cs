using mortgagecruncher.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mortgagecruncher.ViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "Term Start Month")]
        public int StartMonth { get; set; } 
        [Display(Name = "Term Start Year")]
        public int StartYear { get; set; } 
        [Display(Name = "Loan Value")]
        public decimal LoanValue { get; set; } 
        [Display(Name = "Term Years")]
        public int TermYears { get; set; } 
        [Display(Name = "Term Rate")]
        public decimal TermRate { get; set; } 
        [Display(Name = "Fixed Term Years")]
        public int FixedTermYears { get; set; } 
        [Display(Name = "Fixed Term Rate")]
        public decimal FixedTermRate { get; set; } 
        [Display(Name = "Extra Payment Interval")]
        public int ExtraPaymentInterval { get; set; } 
        [Display(Name = "Extra Payment Amount")]
        public decimal ExtraPaymentAmount { get; set; } 

        public string[] MonthNames { get; private set; }

        public IEnumerable<SelectListItem> MonthOptions { get; set; }
        public IEnumerable<SelectListItem> YearOptions { get; set; }

        public IEnumerable<AmortisationScheduleEntry> ScheduleEntries { get; set; }

        public IndexViewModel()
        {
            var now = DateTime.Now;

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

            MonthOptions = Enumerable.Range(1, 12)
                               .Select(x => new SelectListItem {
                                   Value = x.ToString(),
                                   Text = MonthNames[x - 1]
                               });
            
            YearOptions = Enumerable.Range(now.Year - 4, 9)
                              .Select(x => new SelectListItem {
                                  Value = x.ToString(),
                                  Text = x.ToString()
                              });

            ScheduleEntries = new List<AmortisationScheduleEntry>();
        }
    }
}