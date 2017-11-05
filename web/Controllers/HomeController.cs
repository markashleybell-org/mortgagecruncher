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
        public ActionResult Index(IndexViewModel model)
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
                DateTime startDate;

                if (!DateTime.TryParseExact(model.StartDate, "dd/MM/yyyy", null, DateTimeStyles.None, out startDate))
                    ModelState.AddModelError("StartDate", "Date must be in DD/MM/YYYY format.");

                if (!ModelState.IsValid)
                    return View(model);

                var scheduleType = model.AmortisationScheduleType == AmortisationScheduleTypeViewModel.FixedPayments
                                 ? ScheduleType.FixedPayments
                                 : ScheduleType.FixedTerm;

                var overPayments = new Dictionary<double, double> { };

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
                        payment: pmt,
                        principal: ppmt,
                        interest: ipmt,
                        interestRateType: rate.Type,
                        interestRate: rate.APR,
                        balance: bal
                    );
                });

                //var scheduleEntries = new List<AmortisationScheduleEntryViewModel>();

                //foreach (var entry in schedule)
                //{
                //    var interestType = entry.InterestRate.Type == InterestRateType.Fixed
                //                     ? InterestType.Fixed
                //                     : InterestType.Variable;
                //    var scheduleEntry = new AmortisationScheduleEntryViewModel(
                //        paymentNumber: entry.PaymentNumber,
                //        paymentDate: entry.PaymentDate,
                //        payment: entry.Payment,
                //        principal: entry.Principal,
                //        interest: entry.Interest,
                //        interestType: interestType,
                //        interestRate: entry.InterestRate.Rate,
                //        balance: entry.Balance
                //    );
                //    scheduleEntries.Add(scheduleEntry);
                //}

                model.ScheduleEntries = schedule;
            }

            return View(model);
        }
    }
}