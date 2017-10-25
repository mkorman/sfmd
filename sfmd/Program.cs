using System;
using System.Configuration;

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
            string invokedVerb = string.Empty;
            object invokedVerbInstance = null;

            var options = new CommandLineOptions();
            if (!CommandLine.Parser.Default.ParseArguments(args, options, (verb, subOptions) =>
                {
                    invokedVerb = verb;
                    invokedVerbInstance = subOptions;
                }))
            {
                Console.WriteLine("Wrong args!");
                return;
            }

            var metadataClient = CreateMetadataClient();
            metadataClient.Login();

            switch (invokedVerb)
            {
                case "get":
                    metadataClient.GetcustomObjects(((GetSubOptions)invokedVerbInstance).ObjectNames);
                    break;
                case "create":
                    metadataClient.CreateCustomObject("My_Second_Object", "My custom object", "My custom objects");
                    break;
                default:
                    Console.WriteLine("Unsupported verb");
                    break;
            }
            metadataClient.Logout();
            Console.ReadLine();
        }
    }
}
