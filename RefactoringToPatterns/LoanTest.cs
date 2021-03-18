using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace RefactoringToPatterns
{
    
    public class LoanTest
    {
        [Test]
        public void TermLoanFromStart()
        {
            var payments = new List<Payment>
            {
                new Payment {PaymentAmount = 100, PaymentDate = DateTime.Now.AddYears(-3)},
                new Payment {PaymentAmount = 100, PaymentDate = DateTime.Now.AddYears(-2)},
                new Payment {PaymentAmount = 100, PaymentDate = DateTime.Now.AddYears(-1)}
                
            };
            var loan = Loan.NewTermLoan(400, DateTime.Now.AddYears(-4), DateTime.Now, 42);
            loan.payments = payments;
            Assert.That(loan.Capital(),Is.EqualTo(160.0).Within(0.01));
            Assert.That(loan.Duration(),Is.EqualTo(1.33).Within(0.01));
        }
        
        [Test]
        public void RevolverLoan()
        {
            var loan = Loan.NewRevolver(500, DateTime.Now, DateTime.Now.AddYears(8), 4);
            Assert.That(loan.Capital(),Is.EqualTo(800).Within(0.01));
            Assert.That(loan.Duration(),Is.EqualTo(8).Within(0.01));
        }
        
        [Test]
        public void AdvisedLineLoan()
        {
            var loan = Loan.NewAdvisedLine(1200, DateTime.Now, DateTime.Now.AddYears(6), 2);
            Assert.That(loan.Capital(),Is.EqualTo(216).Within(0.01));
            Assert.That(loan.Duration(),Is.EqualTo(6).Within(0.01));
        }
    }
}