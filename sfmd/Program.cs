using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmd
{
    class Program
    {

        private static MetadataClient CreateMetadataClient()
        {
            return new MetadataClient
            {
                Username = ConfigurationManager.AppSettings["username"],
                Password = ConfigurationManager.AppSettings["password"],
                Token = ConfigurationManager.AppSettings["token"]
            };
        }

        static void Main(string[] args)
        {
            var metadataClient = CreateMetadataClient();
            metadataClient.Login();
            metadataClient.GetMetadata("CustomObject", new[] { "NvmNodes__c", "Contact", "Account" });
            //metadataClient.GetMetadata("CustomObject", new[] { "Contact" });
            Console.ReadLine();
        }
    }
}
