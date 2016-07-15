using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mortgagecruncher.Models
{
    public enum AmortisationScheduleType
    {
        [Display(Name = "Monthly Payments")]
        FixedTerm,
        [Display(Name = "Mortgage Term")]
        FixedPayments
    }
}
