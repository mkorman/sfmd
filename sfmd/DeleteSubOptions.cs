using CommandLine;

namespace sfmd
{
    public class DeleteSubOptions
    {
        [OptionArray('o', "object", HelpText = "The name (type) of the custom object to create.")]
        public string[] ObjectTypes { get; set; }
    }
}
