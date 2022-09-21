using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestQR
{
    public class Amounts
    {
        public TransactionAmount transactionAmount { get; set; }
    }

    public class Body
    {
        public PaymentCodeDocument paymentCodeDocument { get; set; }
    }

    public class Header
    {
        public Protocol protocol { get; set; }
        public string messageId { get; set; }
        public DateTime messageDate { get; set; }
        public Originator originator { get; set; }
    }

    public class Originator
    {
        public string system { get; set; }
    }

    public class Parameters1
    {
        public object number { get; set; }
    }

    public class Parameters2
    {
        public string code { get; set; }
    }

    public class PaymentCodeDocument
    {
        public TerminalRef terminalRef { get; set; }
        public ProviderRef providerRef { get; set; }
        public Amounts amounts { get; set; }
        public string description { get; set; }
    }

    public class Protocol
    {
        public string name { get; set; }
        public string version { get; set; }
    }

    public class ProviderRef
    {
        public Parameters2 parameters { get; set; }
    }

    public class RegQrBody
    {
        public Header header { get; set; }
        public Body body { get; set; }
    }

    public class TerminalRef
    {
        public Parameters1 parameters { get; set; }
    }

    public class TransactionAmount
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }
}
