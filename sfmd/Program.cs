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
            metadataClient.GetMetadata("CustomObject", new[] { "Product_Category__c", "Contact", "Account" });
            metadataClient.CreateCustomObject("My_Second_Object", "My custom object", "My custom objects");
            Console.ReadLine();
        }
    }
}
