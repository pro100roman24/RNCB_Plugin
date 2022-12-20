using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResponsegetOperation
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AccountData
    {
        public string number { get; set; }
        public BankData bankData { get; set; }
    }

    public class Amounts
    {
        public TransactionAmount transactionAmount { get; set; }
    }

    public class Attribute
    {
        public string attribute { get; set; }
        public string code { get; set; }
    }

    public class BankData
    {
        public string bic { get; set; }
    }

    public class Body
    {
        public Response response { get; set; }
        public Txn txn { get; set; }
    }

    public class Credit
    {
        public AccountData accountData { get; set; }
        public Parameters parameters { get; set; }
        public IncomingAttributes incomingAttributes { get; set; }
        public TotalAmount totalAmount { get; set; }
    }

    public class Debit
    {
        public AccountData accountData { get; set; }
        public PersonData personData { get; set; }
        public Parameters parameters { get; set; }
        public IncomingAttributes incomingAttributes { get; set; }
        public ResponseAttributes responseAttributes { get; set; }
        public ResponseCodes responseCodes { get; set; }
        public TotalAmount totalAmount { get; set; }
    }

    public class FullName
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string patronymic { get; set; }
    }

    public class Header
    {
        public Protocol protocol { get; set; }
        public string messageId { get; set; }
        public DateTime messageDate { get; set; }
        public Originator originator { get; set; }
    }

    public class IncomingAttributes
    {
        public List<Attribute> attribute { get; set; }
    }

    public class OperationTypeRef
    {
        public Parameters parameters { get; set; }
    }

    public class Originator
    {
        public string system { get; set; }
    }

    public class Parameter
    {
        public string parameter { get; set; }
        public string code { get; set; }
    }

    public class Parameters
    {
        public string code { get; set; }
        public List<Parameter> parameter { get; set; }
    }

    public class PersonData
    {
        public FullName fullName { get; set; }
    }

    public class Protocol
    {
        public string name { get; set; }
        public string version { get; set; }
    }

    public class Reference
    {
        public string origRefNumber { get; set; }
    }

    public class Response
    {
        public string type { get; set; }
        public string code { get; set; }
        public string message { get; set; }
    }

    public class ResponseAttributes
    {
        public List<Attribute> attribute { get; set; }
    }

    public class ResponseCodes
    {
        public List<string> responseCode { get; set; }
    }

    public class getOperationResponse
    {
        public Header header { get; set; }
        public Body body { get; set; }
    }

    public class TotalAmount
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }

    public class TransactionAmount
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }

    public class Txn
    {
        public Reference reference { get; set; }
        public OperationTypeRef operationTypeRef { get; set; }
        public DateTime txnDate { get; set; }
        public string details { get; set; }
        public string direction { get; set; }
        public DateTime processingDate { get; set; }
        public string state { get; set; }
        public Debit debit { get; set; }
        public Credit credit { get; set; }
        public Amounts amounts { get; set; }
    }


}
