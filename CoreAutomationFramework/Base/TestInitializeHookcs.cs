using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAutomationFramework.Base
{
    public class TestInitializeHookcs
    {
        public static Dictionary<string, string> customchromeoptions;
        private readonly ParallelConfig parallelConfig;
        public  TestInitializeHookcs(ParallelConfig parallelConfig)
        {
            this.parallelConfig = parallelConfig;
        }
        public void OpenBrowser(string browserType,string executionAgent , bool incognitoMode = false,string headlessMode=null,string downloadToFolder=null)
        {
            switch(browserType.ToUpper())
            {
                case "CHROME":
                    GoogleChromeDriver googleChromeDriver = new GoogleChromeDriver(parallelConfig);
                    googleChromeDriver.Launch(executionAgent,incognitoMode,headlessMode,downloadToFolder);
                    break;
                case "MICROSOFTEDGE":
                    {
                        MicrosoftEdgeDriver microsoftEdgeDriver= new MicrosoftEdgeDriver(parallelConfig);
                        if(incognitoMode && downloadToFolder!=null)
                        {
                            microsoftEdgeDriver.Launch(incognitoMode, downloadToFolder);
                        }
                        else if(incognitoMode && downloadToFolder==null)
                        {
                            microsoftEdgeDriver.Launch(incognitoMode);
                        }
                        else if(!incognitoMode && downloadToFolder!=null)
                        {
                            microsoftEdgeDriver.Launch(downloadToFolder);
                        }
                        else if(!incognitoMode && downloadToFolder == null)
                        {
                            microsoftEdgeDriver.Launch();
                        }
                    }
                    break;
                case "FIREFOX":
                    break;
            }
        }
        public static ExtentHtmlReporter CreateExtentReportInstance(String featureName)
        {
            ParallelConfig parallelConfig = new ParallelConfig();
            string text = DateTime.Now.ToString("dd-MM-yyyy-HHmmss");
            string currentProjectDictory = parallelConfig.GetCurrentProjectDirectory();
            string text2=Path.Combine(currentProjectDictory,"TestResults","ExtentReports_"+text,featureName);
            if(!Directory.Exists(text2))
            {
                Directory.CreateDirectory(text2);
            }
            ExtentHtmlReporter extentHtmlReporter= new ExtentHtmlReporter(text2);
            extentHtmlReporter.Config.Theme = Theme.Dark;
            extentHtmlReporter.Config.DocumentTitle = "Test Automation Report";
            extentHtmlReporter.Config.ReportName = "test Automationn Report";
            extentHtmlReporter.Config.EnableTimeline = true;
            return extentHtmlReporter;

        }
    }
}
