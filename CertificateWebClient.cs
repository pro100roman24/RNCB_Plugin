using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace RNCB_Plugin
{
    public class CertificateWebClient : WebClient
    {
        private readonly X509Certificate2 certificate;

        public CertificateWebClient(X509Certificate2 cert) => this.certificate = cert;

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest webRequest = (HttpWebRequest)base.GetWebRequest(address);
            ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)((obj, X509certificate, chain, errors) => true);
            webRequest.ClientCertificates.Add((X509Certificate)this.certificate);
            return (WebRequest)webRequest;
        }
    }
}
