using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PaymentGateway.Controllers{
    [Route("api/PaymentGateway")]
    [ApiController]
    public class PaymentsController : ControllerBase{
        private readonly PaymentContext _paymentContext;
        private readonly ShopperContext _shopperContext;

        public PaymentsController(PaymentContext paymentContext, ShopperContext shopperContext){
            _paymentContext = paymentContext;
            _shopperContext = shopperContext;
        }
  

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

        public string bankSimulation(string cardNumber, double amount){
           Shopper shopper = (  from s in _shopperContext.Shopper
                                where s.cardNumber == cardNumber
                                select s).FirstOrDefault<Shopper>();
           if (shopper == null){
               return "unsuccessful";
           }
           else{
               if (shopper.credit - amount < 0){
                   return "unsuccessful";
               }
           }
           return "successful";
        }

        //GET: /api/paymentgateway/n
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<string>> GetPayment(int id){
            var payment = _paymentContext.Payment.Find(id);
            if (payment == null){
                return new string[] {"No payments are associated with this ID!"};
            }
            string cardNumber = maskCardNumber(payment.cardNumber);

            return new string[] {"Payment ID: " + payment.paymentID.ToString(), "Card Number: " + cardNumber, "Expiry Date: " + payment.expiryDate, "Amount " + payment.amount, "Currency: " + payment.currency, "CVV: " + payment.cvv, "Status: " + payment.status};
        }

        //POST: /api/paymentgateway
        [HttpPost]
        public ActionResult<IEnumerable<string>> PostPayment(Payment payment){
            payment.status = bankSimulation(payment.cardNumber, payment.amount);
            _paymentContext.Payment.Add(payment);
            _paymentContext.SaveChanges();
            
            return new string[] {"Payment ID: " + payment.paymentID.ToString(), "Status: " + payment.status};
        }
    }
}