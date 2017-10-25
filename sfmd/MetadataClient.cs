﻿using sfmd.SalesforceMetadataClient;
using System;
using System.Linq;
using System.Net;
using System.Text;
using sfmd.SalesforceEnterpriseClient;
using SaveResult = sfmd.SalesforceMetadataClient.SaveResult;
using SessionHeader = sfmd.SalesforceMetadataClient.SessionHeader;

namespace sfmd
{
    class MetadataClient
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
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
            metadata.ToList().ForEach(md => Console.WriteLine(ToFriendlyString((CustomObject)md)));
            return metadata;
            //metadata = client.readMetadata(sessionHeader, null, "ApexClass", new[] { "ActionProcessor", "AgentDetails" });
            //metadata.ToList().ForEach(md => Console.WriteLine($"Class: {md.fullName}, ApiVersion: {((ApexClass)md).apiVersion}"));
        }

        public void CreateCustomObject(string name, string label, string pluralLabel)
        {
            client = new MetadataPortTypeClient("Metadata", MetadataUrl);
            var sessionHeader = new SessionHeader { sessionId = SessionId };

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

            var result = client.createMetadata(sessionHeader, null, null, new Metadata[] {customObject});
            result.ToList().ForEach(res => Console.WriteLine(ToFriendlyString(res)));
        }

        public static string ToFriendlyString(SaveResult result)
        {
            var sb = new StringBuilder();

            sb.Append(result.fullName);
            sb.Append(": ");
            sb.Append(result.success ? "Success!" : "Failure: ");
            if (result.errors != null)
            {
                sb.Append(string.Join(",", result.errors.Select(err => $"{err.message}: {err.extendedErrorDetails}")));
            }
            return sb.ToString();
        }

        public static string ToFriendlyString(CustomObject obj)
        {
            var sb = new StringBuilder();

            sb.AppendLine(obj.fullName);
            sb.AppendLine($"  Name field: {obj.nameField}");

            return sb.ToString();
        }
    }
}
