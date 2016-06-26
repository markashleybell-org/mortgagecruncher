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
        public decimal Balance { get; private set; }

        public AmortisationScheduleEntry(int paymentNumber, int month, int year, decimal payment, decimal principal, decimal interest, decimal balance)
        {
            PaymentNumber = paymentNumber;
            Month = month;
            Year = year;
            //Payment = Math.Round(payment, 2, MidpointRounding.ToEven);
            //Principal = Math.Round(principal, 2, MidpointRounding.ToEven);
            //Interest = Math.Round(interest, 2, MidpointRounding.ToEven);
            //Balance = Math.Round(balance, 2, MidpointRounding.ToEven);
            Payment = payment;
            Principal = principal;
            Interest = interest;
            Balance = balance;
        }
    }
}