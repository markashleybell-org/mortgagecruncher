using System.ComponentModel.DataAnnotations;

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
