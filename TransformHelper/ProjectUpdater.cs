using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Collections;
using System;

namespace TransformHelper
{
    public class ProjectUpdater
    {
        private readonly ProjectInfo projectInfo;
        private readonly string existingTransformation;
        private readonly string newTransformation;

        public ProjectUpdater(ProjectInfo projectInfo, string existingTransformation, string newTransformation)
        {
            this.projectInfo = projectInfo;
            this.existingTransformation = existingTransformation;
            this.newTransformation = newTransformation;
        }

        public void Update()
        {
            XDocument document = XDocument.Parse(File.ReadAllText(projectInfo.ProjectFilePath));

            Console.WriteLine(string.Format("================================{0}================================", projectInfo.ProjectName));

            Console.WriteLine(string.Format("Processing project {0} located in {1}", projectInfo.ProjectName, projectInfo.ProjectFilePath));

            var transformationItems = document.Descendants("{http://schemas.microsoft.com/developer/msbuild/2003}Content")
                .Where(x => x.Attributes("Include") != null && x.Attributes("Include").FirstOrDefault().Value.EndsWith(string.Format(".{0}.config", existingTransformation)));

            if (transformationItems.Count() == 0)
            {
                Console.WriteLine(string.Format("The project {0} contains no transformation files!", projectInfo.ProjectName, projectInfo.ProjectFilePath));
                return;
            }

            string projectLocation = Path.GetDirectoryName(projectInfo.ProjectFilePath);

            foreach (XElement transformationItem in transformationItems)
            {
                string transformationFilePath = transformationItem.Attributes("Include").FirstOrDefault().Value;
                string newTransformationFilePath = transformationFilePath.Replace(string.Format(".{0}.config", existingTransformation), string.Format(".{0}.config", newTransformation));

                var existingNewTransformation = document.Descendants("{http://schemas.microsoft.com/developer/msbuild/2003}Content")
                    .FirstOrDefault(x => x.Attributes("Include") != null && x.Attributes("Include").FirstOrDefault().Value == newTransformationFilePath);
                if (existingNewTransformation != null)
                {
                    Console.WriteLine(string.Format("Content item for transformation file {0} already exists", newTransformationFilePath));
                    continue;
                }

                string transformationFileFullPath = Path.Combine(projectLocation, transformationFilePath);
                string newTransformationFileFullPath = Path.Combine(projectLocation, newTransformationFilePath);

                if (File.Exists(newTransformationFileFullPath))
                {
                    Console.WriteLine(string.Format("Transformation file {0} already exists.", newTransformationFilePath));
                }
                else
                {
                    Console.WriteLine(string.Format("Creating file {0} copying content from file {1}", newTransformationFilePath, transformationFilePath));
                    File.Copy(transformationFileFullPath, newTransformationFileFullPath);
                }

                var newItem = new XElement(transformationItem);

                newItem.Attributes("Include").FirstOrDefault().Value = newTransformationFilePath;

                Console.WriteLine(string.Format("Adding Content item for transformation file {0}", newTransformationFilePath));
                transformationItem.AddAfterSelf(newItem);
            }

            Console.WriteLine(string.Format("Saving changes for project {0} located in {1}", projectInfo.ProjectName, projectInfo.ProjectFilePath));
            document.Save(projectInfo.ProjectFilePath);
        }
    }
}
