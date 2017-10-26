using sfmd.SalesforceMetadataClient;
using System;
using System.Linq;
using System.Net;
using sfmd.SalesforceEnterpriseClient;
using DeleteResult = sfmd.SalesforceMetadataClient.DeleteResult;
using SaveResult = sfmd.SalesforceMetadataClient.SaveResult;
using SessionHeader = sfmd.SalesforceMetadataClient.SessionHeader;

namespace sfmd
{
    public class MetadataClient : IDisposable
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        private string SessionId { get; set; }
        private string MetadataUrl { get; set; }
        private string EnterpriseUrl { get; set; }

        private MetadataPortTypeClient client;

        public MetadataClient()
        {
            // SF requires TLS 1.1 or 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Logout();
            }
        }

        public void Login()
        {
            var soapClient = new SoapClient();
            var loginResult = soapClient.login(null, Username, Password + Token);

            SessionId = loginResult.sessionId;
            MetadataUrl = loginResult.metadataServerUrl;
            EnterpriseUrl = loginResult.serverUrl;
            Console.WriteLine($"Logged in with session ID: {SessionId}");
        }

        public void Logout()
        {
            if (SessionId == null) return;
            var soapClient = new SoapClient("Soap", EnterpriseUrl);
            soapClient.logout(new SalesforceEnterpriseClient.SessionHeader {sessionId = SessionId});
            Console.WriteLine($"Logged out of session ID {SessionId}");
        }

        public Metadata[] GetCustomObjects(string[] names)
        {
            var result = GetMetadata("CustomObject", names);
            result.ToList().ForEach(md => Console.WriteLine(((CustomObject)md).ToFriendlyString()));
            return result;
        }

        public Metadata[] GetMetadata(string type, string[] names)
        {
            client = new MetadataPortTypeClient("Metadata", MetadataUrl);
            var sessionHeader = new SessionHeader { sessionId = SessionId };
            var metadata = client.readMetadata(sessionHeader, null, type, names);
            return metadata;
        }

        public SaveResult[] CreateCustomObject(string name, string label, string pluralLabel)
        {
            var customObject = new CustomObject {
                fullName = name + "__c",
                label = label,
                pluralLabel = pluralLabel,
                deploymentStatus = DeploymentStatus.Deployed,
                deploymentStatusSpecified = true,
                description = "Created by metadata API",
                enableActivities = true,
                sharingModel = SharingModel.ReadWrite,
                sharingModelSpecified = true
            };
            var nameField = new CustomField
            {
                type = FieldType.Text,
                typeSpecified = true,
                label = label + " Name"
            };

            customObject.nameField = nameField;

            return CreateMetadata(new Metadata[] {customObject});
        }

        public SaveResult[] CreateMetadata(Metadata[] objects)
        {
            client = new MetadataPortTypeClient("Metadata", MetadataUrl);
            var sessionHeader = new SessionHeader { sessionId = SessionId };
            var result = client.createMetadata(sessionHeader, null, null, objects);
            result.ToList().ForEach(res => Console.WriteLine(res.ToFriendlyString()));
            return result;
        }

        public DeleteResult[] DeleteCustomObjects(string[] names)
        {
            return DeleteMetadata("CustomObject", names);
        }

        public DeleteResult[] DeleteMetadata(string type, string[] names)
        {
            client = new MetadataPortTypeClient("Metadata", MetadataUrl);
            var sessionHeader = new SessionHeader { sessionId = SessionId };
            var result = client.deleteMetadata(sessionHeader, null, null, type, names);
            result.ToList().ForEach(res => Console.WriteLine(res.ToFriendlyString()));
            return result;
        }
    }
}
