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

        public AmortisationSchedule(int startMonth, int startYear, double value, int termMonths, double termRate, int fixedTermMonths = 0, double fixedTermRate = 0)
        {
            CalculateAmortisationSchedule(_scheduleEntries, startMonth, startYear, value, termMonths, termRate, fixedTermMonths, fixedTermRate);
        }

        private void CalculateAmortisationSchedule(List<AmortisationScheduleEntry> scheduleEntries, int startMonth, int startYear, double value, int termMonths, double termRate, int fixedTermMonths = 0, double fixedTermRate = 0)
        {
            int variableTermMonths = termMonths - fixedTermMonths;
            double balance = value;

            DateTime date = new DateTime(startYear, startMonth, 1);

            int i = 0;

            // Calculate the schedule entries for the fixed term period
            for(; i < fixedTermMonths; i++)
            {
                var entry = CalculateAmortisationScheduleEntry(value, termMonths, fixedTermRate, (i + 1), date.Month, date.Year, balance);
                scheduleEntries.Add(entry);

                balance = entry.Balance;
                date = date.AddMonths(1);
            }

            double remainingValue = balance;

            // Calculate the schedule entries for the remaining period (or the whole term if there was no fixed period)
            for(; i < termMonths; i++)
            {
                var entry = CalculateAmortisationScheduleEntry(remainingValue, variableTermMonths, termRate, (i + 1), date.Month, date.Year, balance);
                scheduleEntries.Add(entry);

                balance = entry.Balance;
                date = date.AddMonths(1);
            }
        }

        private AmortisationScheduleEntry CalculateAmortisationScheduleEntry(double value, double term, double rate, int paymentNumber, int month, int year, double balance)
        {
            var i = MonthlyInterestRate(rate);
            var a = Math.Pow(1.00 + i, term);

            var payment = value * ((i * a) / (a - 1.00));

            var interest = i * balance;
            var principal = payment - interest;

            var newbalance = (balance - payment) + interest;

            return new AmortisationScheduleEntry(paymentNumber, month, year, payment, principal, interest, newbalance);
        }

        private double MonthlyInterestRate(double termRate)
        {
            return termRate / 100.00 / 12.00;
        }
    }
}