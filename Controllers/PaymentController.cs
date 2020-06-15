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

        public string maskCardNumber(string cardNumber){
            string masked = "";

            for (int i = 0; i < cardNumber.Length; i++){
                masked += (i > 5 && i<cardNumber.Length - 4) ? 'X' : cardNumber[i];
                if ((i+1) % 4 == 0){
                    masked += " ";
                }
            }
            return masked;
        }

        //GET: /api/paymentgateway/n
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<string>> GetPayment(int id){
            var payment = _context.Payment.Find(id);
            if (payment == null){
                return new string[] {"Incorrect identifier. Please try again!"};
            }
            string cardNumber = maskCardNumber(payment.cardNumber);

            return new string[] {"Payment ID: " + payment.paymentID.ToString(), "Card Number: " + cardNumber, "Expiry Date: " + payment.expiryDate, "Amount " + payment.amount, "Currency: " + payment.currency, "CVV: " + payment.cvv, "Status: " + payment.status};
        }

        //POST: /api/paymentgateway
        [HttpPost]
        public ActionResult<IEnumerable<string>> PostPayment(Payment payment){
            try{
                payment.status = "successful";
                _context.Payment.Add(payment);
                _context.SaveChanges();
            }
            catch{
                payment.status = "unsuccessful";
                _context.Payment.Add(payment);
                _context.SaveChanges();
            }
            
            return new string[] {"Payment ID: " + payment.paymentID.ToString(), "Status: " + payment.status};
        }
    }
}