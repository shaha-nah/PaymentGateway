using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;

namespace PaymentGateway.Controllers{
    [Route("api/PaymentGateway")]
    [ApiController]
    public class PaymentsController : ControllerBase{
        private readonly PaymentContext _paymentContext;
        private readonly ShopperContext _shopperContext;
        private readonly LogContext _logContext;

        public PaymentsController(PaymentContext paymentContext, ShopperContext shopperContext, LogContext logContext){
            _paymentContext = paymentContext;
            _shopperContext = shopperContext;
            _logContext = logContext;
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

        public int bankSimulation(Payment payment){
           Shopper shopper = (  from    s in _shopperContext.Shopper
                                where   s.cardNumber == payment.cardNumber
                                select  s).FirstOrDefault<Shopper>();
            if (shopper == null){
                return 500;
            }
            if (shopper.credit - payment.amount < 0){
                return 402;
            }
            if (Convert.ToDateTime(payment.expiryDate) < DateTime.Now){
                return 402;
            }
           return 200;
        }

        public string obtainStatus(Payment payment){
            if (bankSimulation(payment) == 200){
                return "successful";
            }
            return "unsuccessful";
        }
        //GET: /api/paymentgateway/n
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<IEnumerable<string>> GetPayment(int id){
            var payment = _paymentContext.Payment.Find(id);
            if (payment == null){
                return new string[] {"No payments are associated with this ID!"};
            }
            string cardNumber = maskCardNumber(payment.cardNumber);

            Log log = new Log(DateTime.Now, "Merchant viewed payment " + payment.paymentID);
            _logContext.Log.Add(log);
            _logContext.SaveChanges();

            return new string[] {"Payment ID: " + payment.paymentID.ToString(), "Card Number: " + cardNumber, "Expiry Date: " + payment.expiryDate, "Amount " + payment.amount, "Currency: " + payment.currency, "CVV: " + payment.cvv, "Status: " + payment.status};
        }

        //POST: /api/paymentgateway
        [HttpPost]
        [Authorize]
        public ActionResult<IEnumerable<string>> PostPayment(Payment payment){
            payment.status = obtainStatus(payment);
            _paymentContext.Payment.Add(payment);
            _paymentContext.SaveChanges();

            Log log = new Log(DateTime.Now, "Merchant processed payment " + payment.paymentID);
            _logContext.Log.Add(log);
            _logContext.SaveChanges();
            
            return new string[] {"Payment ID: " + payment.paymentID.ToString(), "Status: " + payment.status};
        }
    }
}