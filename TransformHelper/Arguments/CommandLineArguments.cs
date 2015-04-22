using Ookii.CommandLine;
using System;
using System.Text;

namespace TransformHelper.Arguments
{
    public class CommandLineArguments
    {
        [CommandLineArgument("mode", DefaultValue = Mode.Add, Position = 0), Alias("m")]
        public Mode Mode { get; set; }

        [CommandLineArgument("solution"), Alias("sln")]
        public string SolutionPath { get; set; }

        [CommandLineArgument("new"), Alias("n")]
        public string NewTransformation { get; set; }

        [CommandLineArgument("existing"), Alias("e")]
        public string ExsistingTransformation { get; set; }

        [CommandLineArgument("source"), Alias("src")]
        public string SourceFile { get; set; }

        [CommandLineArgument("transformFile"), Alias("tf")]
        public string TransformationFile { get; set; }

        [CommandLineArgument("target"), Alias("t")]
        public string TargetFile { get; set; }

        public bool Validate()
        {
            StringBuilder errors = new StringBuilder();
            switch (this.Mode)
            {
                case Mode.Add:
                    if (string.IsNullOrWhiteSpace(this.SolutionPath))
                    {
                        errors.AppendLine("The required argument 'solution' for 'Add' mode was not supplied.");
                    }
                    if (string.IsNullOrWhiteSpace(this.NewTransformation))
                    {
                        errors.AppendLine("The required argument 'new' for 'Add' mode was not supplied.");
                    }
                    if (string.IsNullOrWhiteSpace(this.ExsistingTransformation))
                    {
                        errors.AppendLine("The required argument 'existing' for 'Add' mode was not supplied.");
                    }
                    break;
                case Mode.Remove:
                    if (string.IsNullOrWhiteSpace(this.SolutionPath))
                    {
                        errors.AppendLine("The required argument 'solution' for 'Remove' mode was not supplied.");
                    }
                    if (string.IsNullOrWhiteSpace(this.ExsistingTransformation))
                    {
                        errors.AppendLine("The required argument 'existing' for 'Remove' mode was not supplied.");
                    }
                    break;
                case Mode.Apply:
                    if (string.IsNullOrWhiteSpace(this.SourceFile))
                    {
                        errors.AppendLine("The required argument 'source' for 'Apply' mode was not supplied.");
                    }
                    if (string.IsNullOrWhiteSpace(this.TransformationFile))
                    {
                        errors.AppendLine("The required argument 'transformFile' for 'Apply' mode was not supplied.");
                    }
                    break;
                case Mode.ApplySLN:
                    if (string.IsNullOrWhiteSpace(this.SolutionPath))
                    {
                        errors.AppendLine("The required argument 'solution' for 'ApplySLN' mode was not supplied.");
                    }
                    if (string.IsNullOrWhiteSpace(this.ExsistingTransformation))
                    {
                        errors.AppendLine("The required argument 'existing' for 'ApplySLN' mode was not supplied.");
                    }
                    break;
                default:
                    break;
            }

            var errorsList = errors.ToString();
            if (!string.IsNullOrWhiteSpace(errorsList))
            {
                Console.WriteLine(errorsList);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
