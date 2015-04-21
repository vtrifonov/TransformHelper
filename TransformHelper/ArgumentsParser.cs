using Ookii.CommandLine;
using System;
using System.Runtime.InteropServices;

namespace TransformHelper
{
    public class ArgumentsParser
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        public CommandLineArguments Parse(string[] args)
        {
            string errorMessage = null;

            CommandLineParser parser = new CommandLineParser(typeof(CommandLineArguments));
            if (args.Length > 0)
            {
                try
                {
                    return (CommandLineArguments)parser.Parse(args);
                }
                catch (CommandLineArgumentException ex)
                {
                    errorMessage = ex.Message;
                    Console.WriteLine(ex.Message);
                    if (GetConsoleWindow() != IntPtr.Zero)
                    {
                        parser.WriteUsageToConsole();
                    }
                }
            }
            else
            {
                errorMessage = "No arguments provided!";
                if (GetConsoleWindow() != IntPtr.Zero)
                {
                    parser.WriteUsageToConsole();
                }
            }

            Console.WriteLine(errorMessage);
            return null;
        }
    }
}
