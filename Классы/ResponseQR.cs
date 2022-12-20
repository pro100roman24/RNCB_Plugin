using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResponseQR
{
    public class Amounts
    {
        public TransactionAmount transactionAmount { get; set; }
    }

    public class Body
    {
        public Response response { get; set; }
        public PaymentCodeDocument paymentCodeDocument { get; set; }
    }

    public class Header
    {
        public Protocol protocol { get; set; }
        public string messageId { get; set; }
        public DateTime messageDate { get; set; }
        public Originator originator { get; set; }
    }

    public class MerchantData
    {
        public string country { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string name { get; set; }
        public string mcc { get; set; }
    }

    public class Originator
    {
        public string system { get; set; }
    }

    public class PaymentCodeDocument
    {
        public string externalId { get; set; }
        public string paymentCodeType { get; set; }
        public Terminal terminal { get; set; }
        public Provider provider { get; set; }
        public Amounts amounts { get; set; }
        public string payload { get; set; }
        public string status { get; set; }
        public MerchantData merchantData { get; set; }
        public string description { get; set; }
        public string traceReferenceNumber { get; set; }
    }

    public class Protocol
    {
        public string name { get; set; }
        public string version { get; set; }
    }

    public class Provider
    {
        public string code { get; set; }
    }

    public class Response
    {
        public string type { get; set; }
        public string code { get; set; }
        public string message { get; set; }
    }

    public class RespQR
    {
        public Header header { get; set; }
        public Body body { get; set; }
    }

    public class Terminal
    {
        public string number { get; set; }
    }

    public class TransactionAmount
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }


}
