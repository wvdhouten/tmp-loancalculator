namespace LoanApp.Core;

public class LoanParameters
{
    public double Amount { get; set; }

    public int Terms { get; set; }

    public double InterestRate { get; set; }

    public DateOnly? StartDate { get; set; }

    public IList<ExtraPayment> ExtraPayments { get; set; } = [];
}
