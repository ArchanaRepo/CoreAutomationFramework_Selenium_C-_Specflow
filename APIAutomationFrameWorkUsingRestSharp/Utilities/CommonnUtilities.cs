using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAutomationFrameWorkUsingRestSharp.Utilities
{
    public class CommonnUtilities
    {
        public static string GetCurrentProjectDirectory()
        {
            string  getPath=AppDomain.CurrentDomain.BaseDirectory;
            string actualPath=getPath.Substring(0,getPath.LastIndexOf("bin",StringComparison.Ordinal));
            string ProjectPath = new Uri(actualPath).LocalPath;
            return ProjectPath;
        }
        //public void UpdateJsonProperty(InputDataModel )
    }
}
