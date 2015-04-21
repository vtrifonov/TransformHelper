using System;
using System.IO;

namespace TransformHelper.Transform
{
    /// <summary>
    /// Used for applying XML-Document-Transform on configuration files
    /// http://vishaljoshi.blogspot.com/2009/03/web-deployment-webconfig-transformation_23.html
    /// http://sedodream.com/2010/04/26/ConfigTransformationsOutsideOfWebAppBuilds.aspx
    /// to be able to run this, you will need "Microsoft SQL Server 2008 Management Objects" or Microsoft.SqlServer.BatchParser.dll on the server
    /// </summary>
    public class ConfigTransformer
    {

        public TransformationResult ApplyTransformation(string sourceFilePath, string transformFilePath, string destinationFilePath = null)
        {
            if (string.IsNullOrWhiteSpace(destinationFilePath) || sourceFilePath.IsEqualTo(destinationFilePath))
            {
                destinationFilePath = sourceFilePath;
                sourceFilePath = System.IO.Path.GetTempFileName();
                File.Copy(destinationFilePath, sourceFilePath);
            }

            var buildLog = new LogBuildEngine();

            Microsoft.Web.Publishing.Tasks.TransformXml transform = new Microsoft.Web.Publishing.Tasks.TransformXml();
            transform.Source = sourceFilePath;
            transform.Transform = transformFilePath;
            transform.Destination = destinationFilePath;
            transform.StackTrace = false;
            transform.BuildEngine = buildLog;

            if (transform.Execute())
            {
                return new TransformationResult
                {
                    Success = true
                };
            }
            else
            {
                return new TransformationResult
                {
                    Success = false,
                    Errors = string.Format("The transformation of file \"{0}\" using transformation file \"{1}\" to destination file \"{2}\" failed with the following log: {4}{3}",
                     sourceFilePath, transformFilePath, destinationFilePath, buildLog.Text, Environment.NewLine)
                };
            }
        }

        public string GetTransformedFileContent(string sourceFilePath, string transformFilePath)
        {
            string destinationFilePath = System.IO.Path.GetTempFileName();
            try
            {
                if (this.ApplyTransformation(sourceFilePath, transformFilePath, destinationFilePath).Success)
                {
                    return File.ReadAllText(destinationFilePath);
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                if (File.Exists(destinationFilePath))
                {
                    File.Delete(destinationFilePath);
                }
            }
        }
    }
}
