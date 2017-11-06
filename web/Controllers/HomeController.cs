using core;
using Newtonsoft.Json;
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
        public const string _cookieName = "mc_settings";

        public ActionResult Index()
        {
            var model = new IndexViewModel {
                StartDate = new DateTime(2015, 7, 25).ToString("dd/MM/yyyy"), // DateTime.Now.ToString("dd/MM/yyyy"),
                LoanValue = 116250,
                TermYears = 25,
                TermRate = 4.49,
                FixedTermYears = 5,
                FixedTermRate = 2.95
            };

            var cookie = HttpContext.Request.Cookies[_cookieName];

            if (cookie != null)
            {
                model = JsonConvert.DeserializeObject<IndexViewModel>(cookie.Value);
                model.ScheduleEntries = GetSchedule(model);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.Valid = true;

            if (!string.IsNullOrWhiteSpace(model.StartDate))
            {
                if (!DateTime.TryParseExact(model.StartDate, "dd/MM/yyyy", null, DateTimeStyles.None, out var startDate))
                    ModelState.AddModelError("StartDate", "Start date must be in DD/MM/YYYY format.");

                if (!ModelState.IsValid)
                    return View(model);
                
                var cookie = HttpContext.Request.Cookies[_cookieName] ?? new HttpCookie(_cookieName);
                cookie.HttpOnly = true;
                cookie.Secure = true;
                cookie.Value = JsonConvert.SerializeObject(model);
                cookie.Expires = DateTime.Now.AddDays(365);
                HttpContext.Response.Cookies.Add(cookie);

                model.ScheduleEntries = GetSchedule(model);
            }

            return View(model);
        }

        private IEnumerable<AmortisationScheduleEntryViewModel> GetSchedule(IndexViewModel model)
        {
            if (!DateTime.TryParseExact(model.StartDate, "dd/MM/yyyy", null, DateTimeStyles.None, out var startDate))
                throw new Exception("Start date must be in DD/MM/YYYY format.");

            var scheduleType = model.AmortisationScheduleType == AmortisationScheduleTypeViewModel.FixedPayments
                             ? ScheduleType.FixedPayments
                             : ScheduleType.FixedTerm;

            var overPayments = model.OverPayments.Select((amt, idx) => new { idx = (double)(idx + 1), amt = (amt * -1) }).ToDictionary(x => x.idx, x => x.amt);

            return AmortisationSchedule.create(
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
        }
    }
}