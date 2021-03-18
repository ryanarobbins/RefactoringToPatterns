namespace RefactoringToPatterns
{
    public class CapitalStrategy
    {
        public double Capital(Loan loan)
        {
            if (loan.expiry == null && loan.maturity != null)
                return loan.commitment * loan.Duration() * RiskFactor(loan);
            if (loan.expiry != null && loan.maturity == null) {
                if (loan.unusedPercentage != 1.0)
                    return loan.commitment * loan.unusedPercentage * loan.Duration() * RiskFactor(loan);
                else
                {
                    return (loan.outstanding * loan.Duration() * RiskFactor(loan))
                           + (UnusedRiskAmount(loan) * loan.Duration() * UnusedRiskFactor(loan));
                }
            }
            return 0.0;
        }

        private double UnusedRiskFactor(Loan loan)
        {
            return UnusedRiskFactorsRepository.GetFactorForRating(loan.riskRating);
        }

        private double RiskFactor(Loan loan)
        {
            return RiskFactorRepository.GetFactorForRating(loan.riskRating);
        }

        private double UnusedRiskAmount(Loan loan)
        {
            return (loan.commitment - loan.outstanding);
        }

    }
}
