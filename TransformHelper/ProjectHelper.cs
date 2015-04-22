using System.IO;
using System.Xml.Linq;
using System.Linq;
using System;
using System.Collections.Generic;
using TransformHelper.Transform;
using System.Text.RegularExpressions;

namespace TransformHelper
{
    public class ProjectHelper
    {
        private readonly ProjectInfo projectInfo;

        public ProjectHelper(ProjectInfo projectInfo)
        {
            this.projectInfo = projectInfo;
        }

        public void AddTransformation(string existingTransformation, string newTransformation)
        {
            XDocument document = XDocument.Parse(File.ReadAllText(projectInfo.ProjectFilePath));

            Console.WriteLine(string.Format("================================{0}================================", projectInfo.ProjectName));

            Console.WriteLine(string.Format("Processing project {0} located in {1}", projectInfo.ProjectName, projectInfo.ProjectFilePath));

            var itemsWithInclude = this.GetItemsWithInclude(document);
            var transformationItems = this.GetTransformationItems(itemsWithInclude, existingTransformation);

            if (transformationItems.Count() == 0)
            {
                Console.WriteLine(string.Format("The project {0} contains no transformation files!", projectInfo.ProjectName, projectInfo.ProjectFilePath));
                return;
            }

            string projectLocation = Path.GetDirectoryName(projectInfo.ProjectFilePath);

            foreach (XElement transformationItem in transformationItems.ToList())
            {
                string transformationFilePath = transformationItem.Attributes("Include").FirstOrDefault().Value;
                string newTransformationFilePath = Regex.Replace(transformationFilePath, string.Format(".{0}.", existingTransformation), string.Format(".{0}.", newTransformation));

                var existingNewTransformation = GetElementByIncludeValue(itemsWithInclude, newTransformationFilePath);
                if (existingNewTransformation != null)
                {
                    Console.WriteLine(string.Format("Item for transformation file {0} already exists", newTransformationFilePath));
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

                Console.WriteLine(string.Format("Adding item for transformation file {0}", newTransformationFilePath));
                transformationItem.AddAfterSelf(newItem);
            }

            Console.WriteLine(string.Format("Saving changes for project {0} located in {1}", projectInfo.ProjectName, projectInfo.ProjectFilePath));
            document.Save(projectInfo.ProjectFilePath);
        }

        public void RemoveTransformation(string existingTransformation)
        {
            XDocument document = XDocument.Parse(File.ReadAllText(projectInfo.ProjectFilePath));

            Console.WriteLine(string.Format("================================{0}================================", projectInfo.ProjectName));

            Console.WriteLine(string.Format("Processing project {0} located in {1}", projectInfo.ProjectName, projectInfo.ProjectFilePath));

            var itemsWithInclude = this.GetItemsWithInclude(document);
            var transformationItems = this.GetTransformationItems(itemsWithInclude, existingTransformation);

            if (transformationItems.Count() == 0)
            {
                Console.WriteLine(string.Format("The project {0} contains no transformation files!", projectInfo.ProjectName, projectInfo.ProjectFilePath));
                return;
            }

            string projectLocation = Path.GetDirectoryName(projectInfo.ProjectFilePath);

            foreach (XElement transformationItem in transformationItems.ToList())
            {
                string transformationFilePath = transformationItem.Attributes("Include").FirstOrDefault().Value;

                string transformationFileFullPath = Path.Combine(projectLocation, transformationFilePath);

                if (File.Exists(transformationFileFullPath))
                {
                    Console.WriteLine(string.Format("Deleting file {0}", transformationFilePath));
                    File.Delete(transformationFileFullPath);
                }

                Console.WriteLine(string.Format("Removing item for transformation file {0}", transformationFilePath));
                transformationItem.Remove();
            }

            Console.WriteLine(string.Format("Saving changes for project {0} located in {1}", projectInfo.ProjectName, projectInfo.ProjectFilePath));
            document.Save(projectInfo.ProjectFilePath);
        }

        public void ApplyTransformation(string existingTransformation)
        {
            XDocument document = XDocument.Parse(File.ReadAllText(projectInfo.ProjectFilePath));

            Console.WriteLine(string.Format("================================{0}================================", projectInfo.ProjectName));

            Console.WriteLine(string.Format("Processing project {0} located in {1}", projectInfo.ProjectName, projectInfo.ProjectFilePath));

            var itemsWithInclude = this.GetItemsWithInclude(document);
            var transformationItems = this.GetTransformationItems(itemsWithInclude, existingTransformation);

            if (transformationItems.Count() == 0)
            {
                Console.WriteLine(string.Format("The project {0} contains no transformation files!", projectInfo.ProjectName, projectInfo.ProjectFilePath));
                return;
            }

            ConfigTransformer transformer = new ConfigTransformer();


            string projectLocation = Path.GetDirectoryName(projectInfo.ProjectFilePath);

            foreach (XElement transformationItem in transformationItems.ToList())
            {
                string transformationFilePath = transformationItem.Attributes("Include").FirstOrDefault().Value;

                string transformationFileFullPath = Path.Combine(projectLocation, transformationFilePath);
                string originalFileFullPath = Regex.Replace(transformationFileFullPath, string.Format(".{0}.config", existingTransformation), string.Format(".config"));

                if (!File.Exists(originalFileFullPath))
                {
                    Console.WriteLine(string.Format("Original file for transformation {0} with transformation file {1} is missing!", originalFileFullPath, transformationFilePath));
                }
                else
                {
                    Console.WriteLine(string.Format("Transforming file {0} using transformation file {1}", originalFileFullPath, transformationFilePath));
                    transformer.ApplyTransformation(originalFileFullPath, transformationFileFullPath);
                }
            }
        }

        public void EnableWarningsAsErrors()
        {
            XDocument document = XDocument.Parse(File.ReadAllText(projectInfo.ProjectFilePath));

            Console.WriteLine(string.Format("================================{0}================================", projectInfo.ProjectName));

            Console.WriteLine(string.Format("Processing project {0} located in {1}", projectInfo.ProjectName, projectInfo.ProjectFilePath));

            var treatWarningsAsErrorsNode = document.Descendants().FirstOrDefault(x => x.Name.LocalName == "TreatWarningsAsErrors");
            if (treatWarningsAsErrorsNode != null 
                && !string.IsNullOrEmpty(treatWarningsAsErrorsNode.Value) 
                && treatWarningsAsErrorsNode.Value.ToLower() == "true")
            {
                return;
            }

            var propertyGroup = document.Descendants().FirstOrDefault(x => x.Name.LocalName == "PropertyGroup");

            Console.WriteLine("Adding <TreatWarningsAsErrors>true<TreatWarningsAsErrors>");
            propertyGroup.Add(new XElement(propertyGroup.Name.Namespace + "TreatWarningsAsErrors", "true"));

            Console.WriteLine(string.Format("Saving changes for project {0} located in {1}", projectInfo.ProjectName, projectInfo.ProjectFilePath));
            document.Save(projectInfo.ProjectFilePath);
        }

        private IEnumerable<XElement> GetItemsWithInclude(XDocument document)
        {
            return document.Descendants()
                           .Where(x =>
                                   x.Attributes("Include") != null
                                   && x.Attributes("Include").Count() > 0);
        }

        private IEnumerable<XElement> GetTransformationItems(IEnumerable<XElement> itemsWithInclude, string transformationName)
        {
            return itemsWithInclude.Where(x =>
                x.Attributes("Include").FirstOrDefault().Value
                .EndsWith(string.Format(".{0}.config", transformationName), StringComparison.InvariantCultureIgnoreCase));
        }

        private XElement GetElementByIncludeValue(IEnumerable<XElement> itemsWithInclude, string includePath)
        {
            return itemsWithInclude.FirstOrDefault(x => x.Attributes("Include").FirstOrDefault().Value.IsEqualTo(includePath));
        }
    }
}
