using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace sfmd
{
    class CommandLineOptions
    {
        [VerbOption("get", HelpText = "Retrieve metadata from your org.")]
        public GetSubOptions GetSubOptions { get; set; }

        [VerbOption("create", HelpText = "Create metadata in your org.")]
        public CreateSubOptions CreateSubOptions { get; set; }

        [VerbOption("delete", HelpText = "Delete metadata in your org.")]
        public DeleteSubOptions DeleteSubOptions { get; set; }
    }
}
