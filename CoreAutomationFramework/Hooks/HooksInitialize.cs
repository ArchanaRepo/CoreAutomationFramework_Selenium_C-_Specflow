using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.MarkupUtils;
using CoreAutomationFramework.Base;
using NUnit.Framework;
using TechTalk.SpecFlow;

[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(4)]
namespace CoreAutomationFramework.Hooks
{
    [Binding]
    public class HooksInitialize : TestInitializeHookcs
    {
        private readonly ParallelConfig _parallelConfig;
        private readonly FeatureContext _featureContext;
        private readonly ScenarioContext _scenarioContext;
        private static ExtentTest _currentScenarioName;
        private MediaEntityModelProvider _mediaEntity;
        private static ExtentTest _featureName;
        private static ExtentReports _extent;
        public static string browsertype, executionAgent, reportPath;
        public HooksInitialize(ParallelConfig parallelConfig,FeatureContext featureContext,ScenarioContext scenarioContext):base(parallelConfig)
        {
            _parallelConfig = parallelConfig;
            _featureContext = featureContext;
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void TestInitialize()
        {
            browsertype = TestContext.Parameters["Browser"];
            executionAgent = TestContext.Parameters["executionAgent"];
            string relativePath = TestContext.Parameters["AccessibilityReportPath"];
            bool archive = Convert.ToBoolean(TestContext.Parameters["ArchiveReport"]);
            
        }
        [BeforeFeature]
        public static void InitiallizeFeature(FeatureContext featureContext)
        {
            CreateExtentReport(featureContext.FeatureInfo.Title);
            _featureName=_extent.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }
        [BeforeScenario]
        public void Initialize()
        {
            _currentScenarioName=_featureName.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
            OpenBrowser(browsertype, executionAgent);
        }
        public Dictionary<string,string> addChromeOptions() 
        {
            Dictionary<string,string> keyValuePairs = new Dictionary<string,string>();
            keyValuePairs.Add("AddArgument1", "--disabled-dev-shm-usage");
            keyValuePairs.Add("AddArgument2", "--no-sandbox");
            keyValuePairs.Add("AddArgument3", "--incognito");
            keyValuePairs.Add("AddAdditionalCapability", "useAutomationExtension,false");
            keyValuePairs.Add("AddUserProfilePreference", "download.default_directory,customdownloadfolderpath");
            return keyValuePairs;
        }

        [AfterStep]
        public void CaptureScreenshots()
        {
            var stepType=_scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            var tags=_scenarioContext.ScenarioInfo.Tags.ToString();
            _mediaEntity = _parallelConfig.CaptureScreenshotAndReturnModel(_scenarioContext.ScenarioInfo.Title.Trim());
            string captureScreenshot = TestContext.Parameters["CaptureScreenshot"].ToUpper();
            if(_scenarioContext.TestError==null)
            {
                switch(stepType)
                {
                    case "Given":
                        _currentScenarioName.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Pass(_scenarioContext.CurrentScenarioBlock.ToString(), _mediaEntity);
                        break;
                    case "When":
                        _currentScenarioName.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text).Pass(_scenarioContext.CurrentScenarioBlock.ToString(), _mediaEntity);
                        break;
                    case "Then":
                        _currentScenarioName.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text).Pass(_scenarioContext.CurrentScenarioBlock.ToString(), _mediaEntity);
                        break;
                    case "And":
                        _currentScenarioName.CreateNode<And>(_scenarioContext.StepContext.StepInfo.Text).Pass(_scenarioContext.CurrentScenarioBlock.ToString(), _mediaEntity);
                        break;
                }
            }
            else if(_scenarioContext.TestError!=null) 
            {
                switch (stepType)
                {
                    case "Given":
                        _currentScenarioName.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message, _mediaEntity);
                        break;
                    case "When":
                        _currentScenarioName.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message, _mediaEntity);
                        break;
                    case "Then":
                        _currentScenarioName.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message, _mediaEntity);
                        break;
                    case "And":
                        _currentScenarioName.CreateNode<And>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message, _mediaEntity);
                        break;
                }
            }
            else if(_scenarioContext.ScenarioExecutionStatus.ToString()=="StepDefinitionPending")
            {
                switch (stepType)
                {
                    case "Given":
                        _currentScenarioName.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                        break;
                    case "When":
                        _currentScenarioName.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                        break;
                    case "Then":
                        _currentScenarioName.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                        break;
                    case "And":
                        _currentScenarioName.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                        break;
                }
            }
            _extent.Flush();

        }
        [AfterScenario]
        public void TerminateScenarioExecution()
        {
            _parallelConfig.Driver.Quit();
            _parallelConfig.Driver = null;
            var dirpath = _parallelConfig.GetCurrentProjectDirectory();
            var filePath = Path.Combine(dirpath, "TestData", "DataFiles", _scenarioContext.ScenarioInfo.Title + ".json");
            if(Directory.Exists(filePath)) 
            {
                var jsonData=File.ReadAllText(filePath);
                _currentScenarioName.CreateNode<And>("json file used for execution").Info(MarkupHelper.CreateCodeBlock(jsonData,CodeLanguage.Json));
                _currentScenarioName.Pass("");
                _extent.Flush();
            }
            else
            {
                _extent.Flush();
            }
        }
        [AfterTestRun]
        public static void TerminateExecution()
        {
            _extent.Flush();
            _extent.AddSystemInfo("Browser", browsertype);
        }

        public static void CreateExtentReport(string featureName)
        {
            var extentreporter = CreateExtentReportInstance(featureName);
            _extent = new ExtentReports();
            _extent.AttachReporter(extentreporter);

        }
    }
}
