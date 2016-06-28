using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mortgagecruncher.Models
{
    public class AmortisationSchedule
    {
        List<AmortisationScheduleEntry> _scheduleEntries = new List<AmortisationScheduleEntry>();
        public IEnumerable<AmortisationScheduleEntry> ScheduleEntries { get { return _scheduleEntries; } }

        public AmortisationSchedule(int startMonth, int startYear, decimal value, int termMonths, decimal termRate, int fixedTermMonths = 0, decimal fixedTermRate = 0, int extraPaymentInterval = 0, decimal extraPaymentAmount = 0)
        {
            CalculateAmortisationSchedule(_scheduleEntries, startMonth, startYear, value, termMonths, termRate, fixedTermMonths, fixedTermRate, extraPaymentInterval, extraPaymentAmount);
        }

        private void CalculateAmortisationSchedule(List<AmortisationScheduleEntry> scheduleEntries, int startMonth, int startYear, decimal value, int termMonths, decimal termRate, int fixedTermMonths = 0, decimal fixedTermRate = 0, int extraPaymentInterval = 0, decimal extraPaymentAmount = 0)
        {
            int fullTermMonths = termMonths;
            int variableTermMonths = termMonths - fixedTermMonths;
            decimal balance = value;
            decimal e = 0;

            // Add a month because the first payment won't go out until the following month
            DateTime date = new DateTime(startYear, startMonth, 1).AddMonths(1);

            int i = 1;

            // Calculate the schedule entries for the fixed term period
            for(; i <= fixedTermMonths; i++)
            {
                e = (extraPaymentInterval > 0 && (i % extraPaymentInterval == 0)) ? extraPaymentAmount : 0;

                var entry = CalculateAmortisationScheduleEntry(value, fullTermMonths, fixedTermRate, i, date.Month, date.Year, balance, e);
                scheduleEntries.Add(entry);
                if(e > 0)
                { 
                     value = entry.Balance;
                     fullTermMonths = termMonths - i;
                }
                balance = entry.Balance;
                date = date.AddMonths(1);
            }

            decimal remainingValue = balance;

            // Calculate the schedule entries for the remaining period (or the whole term if there was no fixed period)
            for(; i <= termMonths; i++)
            {
                e = (extraPaymentInterval > 0 && (i % extraPaymentInterval == 0)) ? extraPaymentAmount : 0;

                var entry = CalculateAmortisationScheduleEntry(remainingValue, variableTermMonths, termRate, i, date.Month, date.Year, balance, e);
                scheduleEntries.Add(entry);

                if(e > 0)
                {
                    remainingValue = entry.Balance;
                    variableTermMonths = termMonths - i;
                }
                balance = entry.Balance;
                date = date.AddMonths(1);
            }
        }

        private AmortisationScheduleEntry CalculateAmortisationScheduleEntry(decimal value, int term, decimal rate, int paymentNumber, int month, int year, decimal balance, decimal extraPaymentAmount = 0)
        {
            var i = MonthlyInterestRate(rate);
            var a = Power(1.00M + i, term);

            var payment = value * ((i * a) / (a - 1.00M));

            if(extraPaymentAmount > 0 && (payment + extraPaymentAmount) <= balance)
                 payment += extraPaymentAmount;

            var interest = i * balance;
            var principal = payment - interest;

            var newbalance = ((balance - payment) + interest);

            return new AmortisationScheduleEntry(paymentNumber, month, year, payment, principal, interest, newbalance);
        }

        private static decimal Power(decimal val, int pow)
        {
            decimal ret = 1;
            for(int i = 0; i < pow; i++)
                ret *= val;
            return ret;
        }

        private decimal MonthlyInterestRate(decimal termRate)
        {
            return termRate / 100.00M / 12.00M;
        }
    }
}