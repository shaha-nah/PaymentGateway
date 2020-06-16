
using System; 

namespace PaymentGateway.Models{
    public class Log{
        public int logID {get; set;}
        public DateTime date {get; set;}
        public string action {get; set;}

        public Log(DateTime date, string action){
            this.date = date;
            this.action = action;
        }
    }
}