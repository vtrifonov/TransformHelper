using Ookii.CommandLine;

namespace TransformHelper
{
    public class CommandLineArguments
    {
        [CommandLineArgument("solution", IsRequired = true, Position = 0)]
        public string SolutionPath { get; set; }

        [CommandLineArgument("new", IsRequired = true, Position = 1)]
        public string NewTransformation { get; set; }

        [CommandLineArgument("existing", IsRequired = true, Position = 2)]
        public string ExsistingTransformation { get; set; }
    }
}
