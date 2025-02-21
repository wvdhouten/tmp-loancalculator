namespace LoanApp.Core
{
    public class PaymentDetails
    {
        public int? Period { get; set; }

        public DateOnly Date { get; set; }

        public double Interest { get; set; }

        public double Principal { get; set; }

        public double Balance { get; set; }

        public double Payment { get; internal set; }

        public PaymentType Type { get; set; }

        public enum PaymentType {
            Installment,
            ReduceDuration,
            ReduceRate
        }
    }
}