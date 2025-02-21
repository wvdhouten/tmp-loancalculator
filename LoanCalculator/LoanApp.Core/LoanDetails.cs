namespace LoanApp.Core
{
    public class LoanDetails
    {
        public LoanDetails(LoanParameters parameters)
        {
            Parameters = parameters;
        }

        public LoanParameters Parameters { get; }

        public IList<PaymentDetails> Payments { get; } = [];

        public double TotalInterest { get; set; }

        public double TotalPaid { get; set; }
    }
}