
namespace LoanApp.Core
{
    public class ExtraPayment
    {
        public DateOnly Date { get; set; }

        public double Amount { get; set; }

        public PaymentType Type { get; set; }

        public enum PaymentType
        {
            Duration,
            Rate
        }
    }
}