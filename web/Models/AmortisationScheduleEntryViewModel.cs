using core;
using System;

namespace web.Models
{
    public class AmortisationScheduleEntryViewModel
    {
        public int PaymentNumber { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public double Payment { get; private set; }
        public double Principal { get; private set; }
        public double Interest { get; private set; }
        public InterestRateType InterestRateType { get; private set; }
        public double InterestRate { get; private set; }
        public double Balance { get; private set; }

        public AmortisationScheduleEntryViewModel(int paymentNumber, DateTime paymentDate, double payment, double principal, double interest, InterestRateType interestRateType, double interestRate, double balance)
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
    }
}