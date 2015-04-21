using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TransformHelper.Arguments;
using TransformHelper.Transform;

namespace TransformHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new ArgumentsParser();

            var parameters = parser.Parse(args);
            if (parameters == null || !parameters.Validate())
            {
                Environment.Exit(1);
            }

            switch (parameters.Mode)
            {
                case Mode.Add:
                    if (!File.Exists(parameters.SolutionPath))
                    {
                        throw new Exception(string.Format("Solution file {0} does not exist!", parameters.SolutionPath));
                    }

                    var projectsProvider = new ProjectsProvider();

                    var projects = projectsProvider.GetProjects(parameters.SolutionPath);

                    foreach (var project in projects)
                    {
                        var projectUpdater = new ProjectUpdater(project, parameters.ExsistingTransformation, parameters.NewTransformation);
                        projectUpdater.Update();
                    }
                    break;
                case Mode.Apply:
                    if (!File.Exists(parameters.SourceFile))
                    {
                        throw new Exception(string.Format("Source file {0} does not exist!", parameters.SolutionPath));
                    }
                    if (!File.Exists(parameters.TransformationFile))
                    {
                        throw new Exception(string.Format("Transformation file {0} does not exist!", parameters.SolutionPath));
                    }

                    var configTransformer = new ConfigTransformer();
                    var transformationResult = configTransformer.ApplyTransformation(parameters.SourceFile, parameters.TransformationFile, parameters.TargetFile);
                    if (!transformationResult.Success)
                    {
                        Console.WriteLine(transformationResult.Errors);
                        Environment.Exit(2);
                    }
                    break;
                default:
                    break;
            }

            Environment.Exit(0);
        }
    }
}
