using AventStack.ExtentReports;
using CoreAutomationFramework.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CoreAutomationFramework.Base
{
    public class ParallelConfig
    {
        public IWebDriver Driver { get; set; }
        public ChromeOptions croptions { get; set; }
        public string RandomString { get; set; }
        public string FinalRandommString { get; set; }
        public Dictionary<string, string> GlobalVariables  { get; set; }
        public LogHelpers LogHelper { get; set; }


        public MediaEntityModelProvider CaptureScreenshotAndReturnModel(string name)
        {
            string asBase64EncodedString = ((ITakesScreenshot)Driver).GetScreenshot().AsBase64EncodedString;
            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(asBase64EncodedString, name).Build();
        }
        public string GetCurrentProjectDirectory()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string uriString = baseDirectory.Substring(0,baseDirectory.LastIndexOf("bin",StringComparison.Ordinal));
            return new Uri(uriString).LocalPath;
        }

    }
}
