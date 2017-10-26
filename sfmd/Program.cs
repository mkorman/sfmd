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

            using (var metadataClient = CreateMetadataClient())
            {
                metadataClient.Login();

                switch (invokedVerb)
                {
                    case "get":
                        metadataClient.GetCustomObjects(((GetSubOptions)invokedVerbInstance).ObjectTypes);
                        break;
                    case "create":
                        var createOptions = (CreateSubOptions)invokedVerbInstance;
                        metadataClient.CreateCustomObject(createOptions.ObjectType, createOptions.Label,
                            createOptions.PluralLabel);
                        break;
                    case "delete":
                        metadataClient.DeleteCustomObjects(((DeleteSubOptions)invokedVerbInstance).ObjectTypes);
                        break;
                    default:
                        Console.WriteLine("Unsupported verb");
                        break;
                }
            }
            Console.ReadLine();
        }
    }
}
