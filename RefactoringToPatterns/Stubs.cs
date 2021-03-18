using System;

namespace RefactoringToPatterns
{
    public class UnusedRiskFactorsRepository
    {
        public static double Factor { get; set; } = 0.2;
        public static double GetFactorForRating(double riskRating)
        {
            return Factor;
        }
    }

    public class RiskFactorRepository
    {
        public static double Factor { get; set; } = 0.3;

        public static double GetFactorForRating(double riskRating)
        {
            return Factor;
        }
    }

    public class Payment
    {
        public double PaymentAmount { get; set; } = 2000.00;

        public DateTime PaymentDate { get; set; } = new DateTime(2021, 1, 6, 0, 0, 0);
        
        public double Amount()
        {
            return PaymentAmount;
        }

        public DateTime Date()
        {
            return PaymentDate;
        }
    }
}