using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResponsecontinueReverse
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Attribute
    {
        public string attribute { get; set; }
        public string code { get; set; }
    }

    public class Attributes
    {
        public List<Attribute> attribute { get; set; }
    }

    public class Body
    {
        public Response response { get; set; }
        public Reference reference { get; set; }
        public Debit debit { get; set; }
        public Credit credit { get; set; }
        public RemainingAmount remainingAmount { get; set; }
    }

    public class Credit
    {
        public TotalAmount totalAmount { get; set; }
    }

    public class Debit
    {
        public TotalAmount totalAmount { get; set; }
        public Attributes attributes { get; set; }
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

    public class Protocol
    {
        public string name { get; set; }
        public string version { get; set; }
    }

    public class Reference
    {
        public int id { get; set; }
    }

    public class RemainingAmount
    {
        public int amount { get; set; }
        public string currency { get; set; }
    }

    public class Response
    {
        public string type { get; set; }
        public string code { get; set; }
        public string message { get; set; }
    }

    public class continueReverseResponse
    {
        public Header header { get; set; }
        public Body body { get; set; }
    }

    public class TotalAmount
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }


}
