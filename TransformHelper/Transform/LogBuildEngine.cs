using Microsoft.Build.Framework;
using System;
using System.Collections;
using System.Text;

namespace TransformHelper.Transform
{
    public class LogBuildEngine : IBuildEngine
    {
        private StringBuilder Log = new StringBuilder();
        public string Text
        {
            get
            {
                return Log.ToString();
            }
        }

        public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs)
        {
            throw new NotImplementedException();
        }

        public int ColumnNumberOfTaskNode
        {
            get { return 0; }
        }

        public bool ContinueOnError
        {
            get { return false; }
        }

        public int LineNumberOfTaskNode
        {
            get { return 0; }
        }

        public void LogCustomEvent(CustomBuildEventArgs e)
        {
            Log.AppendLine(e.Message);
        }

        public void LogErrorEvent(BuildErrorEventArgs e)
        {
            Log.AppendLine(e.Message);
        }

        public void LogMessageEvent(BuildMessageEventArgs e)
        {
            Log.AppendLine(e.Message);
        }

        public void LogWarningEvent(BuildWarningEventArgs e)
        {
            Log.AppendLine(e.Message);
        }

        public string ProjectFileOfTaskNode
        {
            get { return "Transform File Fake Project"; }
        }
    }
}
