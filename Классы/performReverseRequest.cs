using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestperformReverse
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Amounts
    {
        public TransactionAmount transactionAmount { get; set; }
    }

    public class Attribute
    {
        public string code { get; set; }
        public string attribute { get; set; }
    }

    public class Attributes
    {
        public List<Attribute> attribute { get; set; }
    }

    public class Body
    {
        public Txn txn { get; set; }
    }

    public class Credit
    {
        public Attributes attributes { get; set; }
    }

    public class Debit
    {
        public Attributes attributes { get; set; }
    }

    public class Header
    {
        public Protocol protocol { get; set; }
        public string messageId { get; set; }
        public DateTime messageDate { get; set; }
        public Originator originator { get; set; }
    }

    public class Identifier
    {
        public string code { get; set; }
        public string identifier { get; set; }
    }

    public class Identifiers
    {
        public List<Identifier> identifier { get; set; }
    }

    public class Originator
    {
        public string system { get; set; }
    }

    public class OrigOperationRef
    {
        public Identifiers identifiers { get; set; }
    }

    public class Protocol
    {
        public string name { get; set; }
        public string version { get; set; }
    }

    public class performReverseRequest
    {
        public Header header { get; set; }
        public Body body { get; set; }
    }

    public class TransactionAmount
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }

    public class Txn
    {
        public bool isPartial { get; set; }
        public OrigOperationRef origOperationRef { get; set; }
        public Debit debit { get; set; }
        //public Credit credit { get; set; }
        public string details { get; set; }
        public Amounts amounts { get; set; }
    }


}
