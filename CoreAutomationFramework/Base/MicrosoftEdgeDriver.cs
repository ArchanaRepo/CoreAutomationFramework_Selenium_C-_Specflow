using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CoreAutomationFramework.Base
{
    public class MicrosoftEdgeDriver
    {
        private readonly ParallelConfig _parallelConfig;
        public MicrosoftEdgeDriver(ParallelConfig parallelConfig)
        {
            _parallelConfig = parallelConfig;
        }
        public void Launch(string downloadToFolder)
        {
            EdgeOptions edgeOption = new EdgeOptions();
            if(downloadToFolder!=null)
            {
                string text = Path.Combine(_parallelConfig.GetCurrentProjectDirectory() + downloadToFolder);
                if(!Directory.Exists(text)) 
                {
                    Directory.CreateDirectory(text);
                }
            }
            edgeOption.PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Normal;
            _parallelConfig.Driver=new EdgeDriver(edgeOption);
            _parallelConfig.Driver.Manage().Window.Maximize();
        }

        public void Launch(bool inCognitoMode)
        {
            EdgeOptions edgeOption = new EdgeOptions();
            if(inCognitoMode) 
            {
                edgeOption.AddArgument("--inprivate");
            }
            edgeOption.PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Normal;
            _parallelConfig.Driver = new EdgeDriver(edgeOption);
            _parallelConfig.Driver.Manage().Window.Maximize();
        }

        public void Launch(bool incognitoMode,string downloadToFolder)
        {
            EdgeOptions edgeOption = new EdgeOptions();
            if(downloadToFolder != null)
            {
                string text=Path.Combine(_parallelConfig.GetCurrentProjectDirectory()+downloadToFolder);
                if(Directory.Exists(text)) 
                {
                    Directory.CreateDirectory(text );
                }
                edgeOption.AddUserProfilePreference("download.default_directory", text);

            }
            if(incognitoMode) 
            {
                edgeOption.AddArgument("--inprivate");
            }
            edgeOption.PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Normal;
            _parallelConfig.Driver = new EdgeDriver( edgeOption);
            _parallelConfig.Driver.Manage().Window.Maximize();

        }
        public void Launch()
        {
            EdgeOptions edgeOptions=new EdgeOptions();
            edgeOptions.PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Normal;
            _parallelConfig.Driver = new EdgeDriver(edgeOptions);
            _parallelConfig.Driver.Manage().Window.Maximize();
        }
    }
}
