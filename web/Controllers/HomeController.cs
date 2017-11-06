using core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web.Models;

namespace web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(nameof(Calculate), new IndexViewModel {
                StartDate = DateTime.Now.ToString("dd/MM/yyyy"),
                LoanValue = 116250,
                TermYears = 25,
                TermRate = 4.49,
                FixedTermYears = 5,
                FixedTermRate = 2.95
            });
        }

        public ActionResult Calculate(IndexViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.Valid = true;

            if (!string.IsNullOrWhiteSpace(model.StartDate))
            {
                if (!DateTime.TryParseExact(model.StartDate, "dd/MM/yyyy", null, DateTimeStyles.None, out var startDate))
                    ModelState.AddModelError("StartDate", "Date must be in DD/MM/YYYY format.");

                if (!ModelState.IsValid)
                    return View(model);

                var scheduleType = model.AmortisationScheduleType == AmortisationScheduleTypeViewModel.FixedPayments
                                 ? ScheduleType.FixedPayments
                                 : ScheduleType.FixedTerm;

                var overPayments = model.OverPayments.Select((amt, idx) => new { idx = (double)(idx + 1), amt = (amt * -1) }).ToDictionary(x => x.idx, x => x.amt);

                var schedule = AmortisationSchedule.create(
                    model.TermYears,
                    model.LoanValue,
                    scheduleType,
                    model.TermRate,
                    (model.FixedTermYears.HasValue ? model.FixedTermYears.Value : 0),
                    model.FixedTermRate ?? 0,
                    overPayments
                ).Select((row, i) => {
                    var (per, rate, pmt, ipmt, ppmt, bal) = row;
                    return new AmortisationScheduleEntryViewModel(
                        paymentNumber: per,
                        paymentDate: startDate.AddMonths(i),
                        payment: Math.Abs(pmt),
                        principal: Math.Abs(ppmt),
                        interest: Math.Abs(ipmt),
                        interestRateType: rate.Type,
                        interestRate: rate.APR,
                        balance: bal
                    );
                });

                model.ScheduleEntries = schedule;
            }

            return View(model);
        }
    }
}