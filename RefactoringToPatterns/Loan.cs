using System;
using System.Collections.Generic;

namespace RefactoringToPatterns
{
    public class Loan
    {
        public IList<Payment> payments { get; set; }
        public double UnusedPercentage { get; set; } = 1;
        
        private const long MILLIS_PER_DAY = 86400000;
        private const long DAYS_PER_YEAR = 365;
        private const long TICKS_PER_MILLIS = 10000;
        
        private DateTime? expiry;
        private DateTime? maturity;
        private double commitment;
        private double outstanding;
        private double riskRating;
        private DateTime? today;
        private DateTime start;

        private Loan(double commitment, double outstanding, DateTime start, DateTime? expiry, DateTime? maturity,
            double riskRating)
        {
            this.expiry = expiry;
            this.maturity = maturity;
            this.commitment = commitment;
            this.outstanding = outstanding;
            this.riskRating = riskRating;
            this.start = start;
        }
        
        public static Loan NewTermLoan(double commitment, DateTime start, DateTime maturity, int riskRating)
        {
            return new Loan(commitment, commitment, start, null, maturity, riskRating);
        }
        
        public static Loan NewRevolver(double commitment, DateTime start, DateTime expiry, int riskRating) {

            return new Loan(commitment, 0, start, expiry,
                null, riskRating);
        }

        public static Loan NewAdvisedLine(double commitment, DateTime start, DateTime expiry, int riskRating) {
            if (riskRating > 3) return null;
            var advisedLine = new Loan(commitment, 0, start, expiry, null, riskRating);
            advisedLine.UnusedPercentage = 0.1;
            return advisedLine;
        }

        public double Capital() {
            if (expiry == null && maturity != null)
                return commitment * Duration() * RiskFactor();
            if (expiry != null && maturity == null) {
                if (UnusedPercentage != 1.0)
                    return commitment * UnusedPercentage * Duration() * RiskFactor();
                else
                {
                    return (outstanding * Duration() * RiskFactor())
                           + (UnusedRiskAmount() * Duration() * UnusedRiskFactor());
                }
            }
            return 0.0;
        }

        private double UnusedRiskFactor()
        {
            return UnusedRiskFactorsRepository.GetFactorForRating(riskRating);
        }

        private double UnusedRiskAmount()
        {
            return (commitment - outstanding);
        }

        private double RiskFactor()
        {
            return RiskFactorRepository.GetFactorForRating(riskRating);
        }

        public double Duration()
        {
            if (expiry == null && maturity != null)
                return WeightedAverageDuration();
            else if (expiry != null && maturity == null)
                return YearsTo(expiry);
            return 0.0;
        }

        private double YearsTo(DateTime? endDate)
        {
            var now = DateTime.Now;
            var day = DateTime.Now.AddDays(-1);
            var dayTicks = now.Ticks - day.Ticks;
            var beginDate = (today == null ? start : today);
            return ((endDate.Value.Ticks - beginDate.Value.Ticks) / MILLIS_PER_DAY) / TICKS_PER_MILLIS / DAYS_PER_YEAR;
        }

        private double WeightedAverageDuration()
        {
            double duration = 0.0;
            double weightedAverage = 0.0;
            double sumOfPayments = 0.0;
            foreach(var payment in payments) {
                sumOfPayments += payment.Amount();
                var yearsTo = YearsTo(payment.Date());
                weightedAverage += yearsTo * payment.Amount();
            }
            if (commitment != 0.0)
                duration = weightedAverage / sumOfPayments;
            return duration;
        }
    }
}