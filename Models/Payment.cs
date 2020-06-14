namespace PaymentGateway.Models{
    public class Payment{
        public int paymentID {get; set;}
        public string cardNumber{get; set;}
        public string expiryDate{get; set;}
        public double amount{get; set;}
        public string currency{get; set;}
        public string cvv {get; set;}
        public string status{get; set;}
    }
}