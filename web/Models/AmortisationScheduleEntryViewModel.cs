using System;
using core;

namespace web.Models
{
    public class AmortisationScheduleEntryViewModel
    {
        public AmortisationScheduleEntryViewModel(
            int paymentNumber,
            DateTime paymentDate,
            double payment,
            double principal,
            double interest,
            InterestRateType interestRateType,
            double interestRate,
            double balance)
        {
            PaymentNumber = paymentNumber;
            PaymentDate = paymentDate;
            Payment = payment;
            Principal = principal;
            Interest = interest;
            InterestRateType = interestRateType;
            InterestRate = interestRate;
            Balance = balance;
        }

        public int PaymentNumber { get; }

        public DateTime PaymentDate { get; }

        public double Payment { get; }

        public double Principal { get; }

        public double Interest { get; }

        public InterestRateType InterestRateType { get; }

        public double InterestRate { get; }

        public double Balance { get; }
    }
}
