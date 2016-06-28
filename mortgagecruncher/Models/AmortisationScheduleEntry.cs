using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mortgagecruncher.Models
{
    public class AmortisationScheduleEntry
    {
        public int PaymentNumber { get; set; }
        public int Month { get; private set; }
        public int Year { get; private set; }
        public decimal Payment { get; private set; }
        public decimal Principal { get; private set; }
        public decimal Interest { get; private set; }
        public InterestType InterestType { get; private set; }
        public decimal InterestRate { get; private set; }
        public decimal Balance { get; private set; }

        public AmortisationScheduleEntry(int paymentNumber, int month, int year, decimal payment, decimal principal, decimal interest, InterestType interestType, decimal interestRate, decimal balance)
        {
            PaymentNumber = paymentNumber;
            Month = month;
            Year = year;
            Payment = payment;
            Principal = principal;
            Interest = interest;
            InterestType = interestType;
            InterestRate = interestRate;
            Balance = balance;
        }
    }
}