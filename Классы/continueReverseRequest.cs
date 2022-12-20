using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestcontinueReverse
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Body
    {
        public Txn txn { get; set; }
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
        public string id { get; set; }
    }

    public class continueReverseRequest
    {
        public Header header { get; set; }
        public Body body { get; set; }
    }

    public class Txn
    {
        public Reference reference { get; set; }
    }


}
