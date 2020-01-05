using System.ComponentModel.DataAnnotations;

namespace web.Models
{
    public enum AmortisationScheduleTypeViewModel
    {
        [Display(Name = "Monthly Payments")]
        FixedTerm,

        [Display(Name = "Mortgage Term")]
        FixedPayments
    }
}
