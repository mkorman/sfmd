using CommandLine;

namespace sfmd
{
    public class CreateSubOptions
    {
        [Option('o', "object", HelpText = "The name (type) of the custom object to create.")]
        public string ObjectType { get; set; }
        [Option('l', "label", DefaultValue = "Custom Object", HelpText = "The label of the custom object to create.")]
        public string Label { get; set; }
        [Option('p', "plural", DefaultValue = "Custom Objects", HelpText = "The plural label of the custom object to create.")]
        public string PluralLabel { get; set; }
    }
}
