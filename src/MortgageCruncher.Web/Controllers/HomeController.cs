using Microsoft.AspNetCore.Mvc;
using mortgagecruncher.ViewModels;
using System;
using System.Globalization;
using System.Collections.Generic;
using MortgageCruncher.Core;

namespace mortgagecruncher.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var tmp = AmortisationSchedule.create(AmortisationScheduleType.FixedPayments, 0M, 0, DateTime.Now, 0M, 0, 0M, 0M);

            return View("Calculate", new IndexViewModel {
                StartDate = DateTime.Now.ToString("dd/MM/yyyy"),
                LoanValue = 100000,
                TermYears = 25,
                TermRate = 4.49M
            });
        }

        public IActionResult Calculate(IndexViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            model.Valid = true;

            if(!string.IsNullOrWhiteSpace(model.StartDate))
            { 
                DateTime startDate;
                DateTime? overpaymentStartDate = null;

                if(!DateTime.TryParseExact(model.StartDate, "dd/MM/yyyy", null, DateTimeStyles.None, out startDate))
                    ModelState.AddModelError("StartDate", "Date must be in DD/MM/YYYY format.");

                if(!string.IsNullOrWhiteSpace(model.OverpaymentStartDate))
                {
                    DateTime opStart;
                    if(DateTime.TryParseExact(model.OverpaymentStartDate, "dd/MM/yyyy", null, DateTimeStyles.None, out opStart))
                        overpaymentStartDate = opStart;
                    else
                        ModelState.AddModelError("OverpaymentStartDate ", "Date must be in DD/MM/YYYY format.");
                }

                if(!ModelState.IsValid)
                    return View(model);

                var scheduleType = model.AmortisationScheduleType == AmortisationScheduleTypeViewModel.FixedPayments 
                                 ? AmortisationScheduleType.FixedPayments
                                 : AmortisationScheduleType.FixedTerm;

                //loanValue mortgageTerm mortgageStartDate standardInterestRate fixedRateTerm fixedInterestRate overPayment 
                var schedule = AmortisationSchedule.create(
                    scheduleType,
                    model.LoanValue,
                    (model.TermYears * 12),
                    startDate,
                    model.TermRate,
                    (model.FixedTermYears.HasValue ? model.FixedTermYears.Value * 12 : 0),
                    model.FixedTermRate ?? 0,
                    // model.OverpaymentInterval ?? 0,
                    // overpaymentStartDate,
                    model.OverpaymentAmount ?? 0
                );

                var scheduleEntries = new List<AmortisationScheduleEntryViewModel>();

                foreach(var entry in schedule)
                {
                    var interestType = entry.InterestRate.Type == InterestRateType.Fixed
                                     ? InterestType.Fixed
                                     : InterestType.Variable;
                    var scheduleEntry = new AmortisationScheduleEntryViewModel(
                        paymentNumber: entry.PaymentNumber,
                        paymentDate: entry.PaymentDate,
                        payment: entry.Payment,
                        principal: entry.Principal,
                        interest: entry.Interest,
                        interestType: interestType,
                        interestRate: entry.InterestRate.Rate,
                        balance: entry.Balance
                    );
                    scheduleEntries.Add(scheduleEntry);
                }

                model.ScheduleEntries = scheduleEntries;
            }

            return View(model);
        }
    }
}
