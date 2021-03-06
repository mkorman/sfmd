﻿using CommandLine;

namespace sfmd
{
    public class GetSubOptions
    {
        [OptionArray('o', "object", HelpText = "The name (type) of the custom object to retrieve.")]
        public string[] ObjectTypes { get; set; }
    }
}
