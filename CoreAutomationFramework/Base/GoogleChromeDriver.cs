using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAutomationFramework.Base
{
    public class GoogleChromeDriver
    {
        private readonly ParallelConfig parallelConfig;
        public GoogleChromeDriver(ParallelConfig parallelConfig)
        {
            this.parallelConfig = parallelConfig;
        }
        public void Launch(string executionAgent , bool incognitoMode=false , string headlessMode="No",string downloadToFolder=null)
        {
            ChromeOptions chromeOptions=new ChromeOptions();
            chromeOptions.AddAdditionalOption("useAutomationExtension", false);
            chromeOptions.AddArgument("--disable-dev-shm-usage");
            chromeOptions.AddArgument("--disable-popup-blocking");
            chromeOptions.AddArgument("--no-sandbox");
            try
            {
                if(incognitoMode)
                {
                    chromeOptions.AddArguments("--incognito");
                }
            }catch (Exception ex) { Console.WriteLine(ex.Message); }

            if(downloadToFolder != null) 
            {
                string text = Path.Combine(parallelConfig.GetCurrentProjectDirectory() + downloadToFolder);
                if(!Directory.Exists(text)) 
                {
                    Directory.CreateDirectory(text);
                }
                chromeOptions.AddUserProfilePreference("download.default_directory", text);
            }
            chromeOptions.AddUserProfilePreference("disabled-popup-blocking", true);
            if(headlessMode!=null)
            {
                if (headlessMode.ToUpper() == "YES")
                {
                    chromeOptions.AddArgument("--headless");
                }

            }

            if (executionAgent != null)
            {
                switch (executionAgent.ToUpper())
                {
                    case "LOCAL":
                        parallelConfig.Driver = new ChromeDriver(chromeOptions);
                        parallelConfig.Driver.Manage().Window.Maximize();
                        break;
                    case "REMOTESAUCELAB":
                        break;
                    case "REMOTEBROWSERSTACK":
                        break;
                    case "CHROMEBROWSER":
                        {
                            Process.Start("..\\KillChromeProcess.Bat");
                            string userName = Environment.UserName;
                            chromeOptions.AddArgument("-noerrordialogs");
                            chromeOptions.AddArgument("user-data-dir=C:\\Users\\" + userName + "\\AppData\\Local\\Google\\Chrome\\User Data");
                            parallelConfig.Driver = new ChromeDriver(chromeOptions);
                            parallelConfig.Driver.Manage().Window.Maximize();
                            break;
                        }
                    default:
                        parallelConfig.Driver = new ChromeDriver(chromeOptions);
                        break;


                }
            }
            else
            {
                parallelConfig.Driver = new ChromeDriver(chromeOptions);

            }

            
            

        }

        public  ChromeOptions setChromeOptions(Dictionary<string, string> optionsCollection)
        {
            string text = "";
            dynamic val = "";
            ChromeOptions chromeOptions = new ChromeOptions();
            if(optionsCollection != null) 
            {
                foreach(string key in optionsCollection.Keys)
                {
                    if (optionsCollection[key].Contains(",") )
                    {
                        string[] array = optionsCollection[key].Split(',');
                        text = optionsCollection[key][0].ToString();
                        if(text.ToLower()== "addadditionalcapability")
                        {
                            if (optionsCollection[key][1].ToString().Contains("false"))
                            {
                                val = true;
                            }
                            else if(optionsCollection[key][1].ToString().Contains("true"))
                            {
                                val = true;
                            }
                            chromeOptions.AddAdditionalChromeOption(key, val);
                        }
                        else
                        {
                            val = optionsCollection[key][1].ToString();
                            chromeOptions.AddUserProfilePreference(text,val);
                        }
                    }
                    else
                    {
                        chromeOptions.AddArguments(optionsCollection[key]);
                    }
                }
            }
            return chromeOptions;

        }
    }
}
