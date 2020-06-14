using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Models;
using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.Controllers{
    [Route("api/PaymentGateway")]
    [ApiController]
    public class PaymentsController : ControllerBase{
        private readonly PaymentContext _context;

        public PaymentsController(PaymentContext context) => _context = context;

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<string>> GetPayment(int id){
            var payment = _context.Payment.Find(id);
            if (payment == null){
                return new string[] {"Incorrect identifier. Please try again!"};
            }
            string cardNumber = payment.cardNumber;
            string masked = "";

            for (int i = 0; i < cardNumber.Length; i++){
                masked += (i > 5 && i<cardNumber.Length - 4) ? 'X' : cardNumber[i];
                if ((i+1) % 4 == 0){
                    masked += " ";
                }
            }

            return new string[] {"Payment ID: " + payment.paymentID.ToString(), "Card Number: " + masked, "Expiry Date: " + payment.expiryDate, "Amount " + payment.amount, "Currency: " + payment.currency, "CVV: " + payment.cvv, "Status: " + payment.status};
        }
    }
}