using Microsoft.AspNetCore.Mvc;
using mortgagecruncher.Models;
using mortgagecruncher.ViewModels;

namespace mortgagecruncher.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(IndexViewModel model)
        {
            if(model.StartMonth > 0)
            { 
                var schedule = new AmortisationSchedule(
                    model.StartMonth,
                    model.StartYear,
                    model.LoanValue,
                    (model.TermYears * 12),
                    model.TermRate,
                    (model.FixedTermYears * 12),
                    model.FixedTermRate,
                    model.ExtraPaymentInterval,
                    model.ExtraPaymentAmount
                );

                model.ScheduleEntries = schedule.ScheduleEntries;
            }

            return View(model);
        }
    }
}
