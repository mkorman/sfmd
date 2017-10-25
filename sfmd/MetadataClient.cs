using sfmd.SalesforceMetadataClient;
using System;
using System.Linq;
using System.Net;
using sfmd.SalesforceEnterpriseClient;
using SessionHeader = sfmd.SalesforceMetadataClient.SessionHeader;

namespace sfmd
{
    class MetadataClient
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string OrgId { get; set; }
        public string SessionId { get; set; }
        public string MetadataUrl { get; set; }

        private MetadataPortTypeClient client;

        public MetadataClient()
        {
            // SF requires TLS 1.1 or 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
        }

        public void Login()
        {
            var soapClient = new SoapClient();
            var loginResult = soapClient.login(null, Username, Password + Token);

            SessionId = loginResult.sessionId;
            MetadataUrl = loginResult.metadataServerUrl;
            //Console.WriteLine($"Logged in. Session ID is {SessionId} and URL is {MetadataUrl}");
        }

        public Metadata[] GetMetadata(string type, string[] names)
        {
            client = new MetadataPortTypeClient("Metadata", MetadataUrl);
            var sessionHeader = new SessionHeader { sessionId = SessionId };
            var metadata = client.readMetadata(sessionHeader, null, type, names);
            metadata.ToList().ForEach(md => Console.WriteLine(md.fullName));
            return metadata;
            //metadata = client.readMetadata(sessionHeader, null, "ApexClass", new[] { "ActionProcessor", "AgentDetails" });
            //metadata.ToList().ForEach(md => Console.WriteLine($"Class: {md.fullName}, ApiVersion: {((ApexClass)md).apiVersion}"));
        }
    }
}
