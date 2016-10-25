using System;

namespace mortgagecruncher.Models
{
    public class AmortisationScheduleEntry
    {
        public int PaymentNumber { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public decimal Payment { get; private set; }
        public decimal Principal { get; private set; }
        public decimal Interest { get; private set; }
        public InterestType InterestType { get; private set; }
        public decimal InterestRate { get; private set; }
        public decimal Balance { get; private set; }

        public AmortisationScheduleEntry(int paymentNumber, DateTime paymentDate, decimal payment, decimal principal, decimal interest, InterestType interestType, decimal interestRate, decimal balance)
        {
            PaymentNumber = paymentNumber;
            PaymentDate = paymentDate;
            Payment = payment;
            Principal = principal;
            Interest = interest;
            InterestType = interestType;
            InterestRate = interestRate;
            Balance = balance;
        }
    }
}