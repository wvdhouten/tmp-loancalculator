using Microsoft.VisualBasic;

namespace LoanApp.Core;

public class LoanCalculator
{
    // TODO: Handle Fee Processing

    public LoanDetails Calculate(LoanParameters parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        parameters.StartDate ??= DateOnly.FromDateTime(DateTime.Now);

        var monthlyRate = GetMonthlyRate(parameters.InterestRate);
        var monthlyPayment = GetPayment(monthlyRate, parameters.Terms, parameters.Amount);

        var details = new LoanDetails(parameters);
        var extraPayments = new List<ExtraPayment>(parameters.ExtraPayments);

        var balance = parameters.Amount;
        for (int period = 1; period <= parameters.Terms; period++)
        {
            var dueDate = parameters.StartDate.Value.AddMonths(period);

            foreach (var extraPayment in extraPayments.Where(x => x.Date < dueDate).ToList())
            {
                var amount = Math.Min(balance, extraPayment.Amount);
                
                balance -= amount;
                if (extraPayment.Type == ExtraPayment.PaymentType.Rate)
                    monthlyPayment = GetPayment(monthlyRate, parameters.Terms - (period - 1), balance);

                details.Payments.Add(new PaymentDetails {
                    Date = extraPayment.Date,
                    Interest = 0,
                    Payment = amount,
                    Principal = amount,
                    Balance = balance,
                    Type = extraPayment.Type == ExtraPayment.PaymentType.Duration
                        ? PaymentDetails.PaymentType.ReduceDuration 
                        : PaymentDetails.PaymentType.ReduceRate
                });

                extraPayments.Remove(extraPayment);
            }

            if (balance <= 0)
                break;

            var payment = GetPaymentDetails(period, dueDate, monthlyRate, monthlyPayment, balance);
            UpdateLoanDetails(details, payment);

            balance = payment.Balance;
            if (balance <= 0)
                break;
        }

        return details;
    }

    private static void UpdateLoanDetails(LoanDetails details, PaymentDetails payment)
    {
        details.TotalInterest += payment.Interest;
        details.TotalPaid += payment.Payment;
        details.Payments.Add(payment);
    }

    private static PaymentDetails GetPaymentDetails(int period, DateOnly dueDate, double rate, double payment, double balance)
    {
        var interest = balance * rate;
        var principal = Math.Min(balance, payment - interest);

        return new PaymentDetails
        {
            Type = PaymentDetails.PaymentType.Installment,
            Period = period,
            Date = dueDate,
            Interest = interest,
            Principal = principal,
            Payment = interest + principal,
            Balance = Math.Abs(balance - principal)
        };
    }

    private static double GetMonthlyRate(double interestRate) =>
        interestRate / 100 / 12;

    private static double GetPayment(double monthlyRate, int terms, double amount) =>
        Math.Abs(Financial.Pmt(monthlyRate, terms, amount));
}
