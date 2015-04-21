using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TransformHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new ArgumentsParser();

            var parameters = parser.Parse(args);

            if (parameters == null)
            {
                Environment.Exit(1);
            }

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

            Environment.Exit(0);
        }
    }
}
