using Microsoft.AspNetCore.Mvc;
using mortgagecruncher.Models;
using mortgagecruncher.ViewModels;
using System;
using System.Globalization;

namespace mortgagecruncher.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("Calculate", new IndexViewModel {
                LoanValue = 100000,
                TermYears = 25,
                TermRate = 4.49M
            });
        }

        public IActionResult Calculate(IndexViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            if(!string.IsNullOrWhiteSpace(model.StartDate))
            { 
                DateTime startDate;

                if(!DateTime.TryParseExact(model.StartDate, "dd/MM/yyyy", null, DateTimeStyles.None, out startDate))
                {
                    ModelState.AddModelError("StartDate", "Date must be in DD/MM/YYYY format.");
                    return View(model);
                }

                var schedule = new AmortisationSchedule(
                    startDate.Month,
                    startDate.Year,
                    model.LoanValue,
                    (model.TermYears * 12),
                    model.TermRate,
                    (model.FixedTermYears.HasValue ? model.FixedTermYears.Value * 12 : 0),
                    model.FixedTermRate ?? 0,
                    model.ExtraPaymentInterval ?? 0,
                    model.ExtraPaymentAmount ?? 0
                );

                model.ScheduleEntries = schedule.ScheduleEntries;
            }

            return View(model);
        }
    }
}
