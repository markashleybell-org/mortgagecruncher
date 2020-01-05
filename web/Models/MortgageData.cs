using System.Collections.Generic;

namespace web.Models
{
    public class MortgageData
    {
        public string StartDate { get; set; }

        public double LoanValue { get; set; }

        public int TermYears { get; set; }

        public double TermRate { get; set; }

        public int? FixedTermYears { get; set; }

        public double? FixedTermRate { get; set; }

        public AmortisationScheduleTypeViewModel AmortisationScheduleType { get; set; }

        public IList<double> OverPayments { get; set; }
    }
}
