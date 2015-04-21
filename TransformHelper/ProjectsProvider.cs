using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TransformHelper
{
    public class ProjectsProvider
    {
        private static Regex projectsRegex = new Regex("Project\\(\\\"{[0-9A-F]{8}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{" +
                "4}-[0-9A-F]{12}\\}\\\"\\)\\s*\\=\\s*\\\"(?<ProjectName>[^\\\"]" +
                "+)\\\"\\s*,\\s*\\\"(?<ProjectPath>[^\\\"]+)\\\"",
                RegexOptions.CultureInvariant | RegexOptions.Compiled);

        public IEnumerable<ProjectInfo> GetProjects(string solutionFileLocation)
        {
            string solutionFileContent = File.ReadAllText(solutionFileLocation);

            MatchCollection matches = projectsRegex.Matches(solutionFileContent);

            string solutionPath = Path.GetDirectoryName(solutionFileLocation);

            foreach (Match match in matches)
            {
                string projectName = match.Groups["ProjectName"].Value;
                string relativePath = match.Groups["ProjectPath"].Value;
                string fullPath = Path.Combine(solutionPath, relativePath);
                if (File.Exists(fullPath))
                {
                    yield return new ProjectInfo
                    {
                        ProjectName = projectName,
                        ProjectFilePath = fullPath
                    };
                }
            }
        }
    }
}
