using System.ComponentModel.DataAnnotations;

namespace mortgagecruncher.ViewModels
{
    public enum AmortisationScheduleTypeViewModel
    {
        [Display(Name = "Monthly Payments")]
        FixedTerm,
        [Display(Name = "Mortgage Term")]
        FixedPayments
    }
}
