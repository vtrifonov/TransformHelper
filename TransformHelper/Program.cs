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
                        var projectUpdater = new ProjectHelper(project);
                        projectUpdater.AddTransformation(parameters.ExsistingTransformation, parameters.NewTransformation);
                    }
                    break;
                case Mode.Remove:
                    if (!File.Exists(parameters.SolutionPath))
                    {
                        throw new Exception(string.Format("Solution file {0} does not exist!", parameters.SolutionPath));
                    }

                    projectsProvider = new ProjectsProvider();

                    projects = projectsProvider.GetProjects(parameters.SolutionPath);

                    foreach (var project in projects)
                    {
                        var projectUpdater = new ProjectHelper(project);
                        projectUpdater.RemoveTransformation(parameters.ExsistingTransformation);
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
                case Mode.ApplySLN:
                    if (!File.Exists(parameters.SolutionPath))
                    {
                        throw new Exception(string.Format("Solution file {0} does not exist!", parameters.SolutionPath));
                    }

                    projectsProvider = new ProjectsProvider();

                    projects = projectsProvider.GetProjects(parameters.SolutionPath);

                    foreach (var project in projects)
                    {
                        var projectUpdater = new ProjectHelper(project);
                        projectUpdater.ApplyTransformation(parameters.ExsistingTransformation);
                    }
                    break;
                default:
                    break;
            }

            Environment.Exit(0);
        }
    }
}
