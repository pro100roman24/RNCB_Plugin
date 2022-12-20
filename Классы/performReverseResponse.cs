using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResponseperformReverse
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

    public class Response
    {
        public string type { get; set; }
        public string code { get; set; }
        public string message { get; set; }
    }

    public class performReverseResponse
    {
        public Header header { get; set; }
        public Body body { get; set; }
    }


}
