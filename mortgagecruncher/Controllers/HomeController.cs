﻿using mortgagecruncher.Models;
using mortgagecruncher.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mortgagecruncher.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(IndexViewModel model)
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
                    model.FixedTermRate
                );

                model.ScheduleEntries = schedule.ScheduleEntries;
            }

            return View(model);
        }
    }
}