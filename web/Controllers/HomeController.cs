using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using web.Models;

namespace web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _cookieName = "mc_settings";
        private readonly HttpContext _ctx;

        public HomeController(IHttpContextAccessor httpContextAccessor) =>
            _ctx = httpContextAccessor.HttpContext;

        public ActionResult Index()
        {
            var model = new IndexViewModel {
                StartDate = DateTime.Now.ToString("dd/MM/yyyy"),
                LoanValue = 100000,
                TermYears = 25,
                TermRate = 4.49,
                FixedTermYears = 5,
                FixedTermRate = 2.95
            };

            var cookieValue = _ctx.Request.Cookies[_cookieName];

            if (cookieValue is object)
            {
                var mortgageData = JsonConvert.DeserializeObject<MortgageData>(cookieValue);

                model.PopulateFrom(mortgageData);

                model.ScheduleEntries = GetSchedule(mortgageData);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Valid = true;

            if (!string.IsNullOrWhiteSpace(model.StartDate))
            {
                if (!DateTime.TryParseExact(model.StartDate, "dd/MM/yyyy", null, DateTimeStyles.None, out var startDate))
                {
                    ModelState.AddModelError("StartDate", "Start date must be in DD/MM/YYYY format.");
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var cookieOptions = new CookieOptions {
                    Expires = DateTime.Now.AddDays(365)
                };

                var mortgageData = model.AsMortgageData();

                _ctx.Response.Cookies.Append(_cookieName, JsonConvert.SerializeObject(mortgageData), cookieOptions);

                model.ScheduleEntries = GetSchedule(mortgageData);
            }

            return View(model);
        }

        private IEnumerable<AmortisationScheduleEntryViewModel> GetSchedule(MortgageData mortgageData)
        {
            if (!DateTime.TryParseExact(mortgageData.StartDate, "dd/MM/yyyy", null, DateTimeStyles.None, out var startDate))
            {
                throw new Exception("Start date must be in DD/MM/YYYY format.");
            }

            var scheduleType = mortgageData.AmortisationScheduleType == AmortisationScheduleTypeViewModel.FixedPayments
                ? ScheduleType.FixedPayments
                : ScheduleType.FixedTerm;

            var overPayments = mortgageData.OverPayments
                .Select((amt, idx) => new { idx = (double)(idx + 1), amt = amt * -1 })
                .ToDictionary(x => x.idx, x => x.amt);

            return AmortisationSchedule.create(
                mortgageData.TermYears,
                mortgageData.LoanValue,
                scheduleType,
                mortgageData.TermRate,
                mortgageData.FixedTermYears ?? 0,
                mortgageData.FixedTermRate ?? 0,
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
        }
    }
}
