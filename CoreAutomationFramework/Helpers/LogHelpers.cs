using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository;
using static log4net.Appender.FileAppender;
using static log4net.Appender.RollingFileAppender;

namespace CoreAutomationFramework.Helpers
{
    public class LogHelpers
    {
        private  string currentFeatureName {  get; set; }
        private string logFolderPath { get; set; }
        private string logParentFolderPath {  get; set; }
        public ILog Logger { get; set; }

        public  LogHelpers() 
        {
            logParentFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if(!Directory.Exists(logParentFolderPath))
            {
                Directory.CreateDirectory(logParentFolderPath);
            }

            string path = DateTime.Now.ToString("ddd_dd.MM.YYYY_HHmmss");
            logFolderPath=Path.Combine(logParentFolderPath, path);
            if(!Directory.Exists(logFolderPath)) 
            {
                Directory.CreateDirectory(logFolderPath);
            }
        }

        public  void SetUpConfigurations(string featureName)
        {
            PatternLayout layout = new PatternLayout("%date{dd MMM yyy HH:mm:ss,fff} %level %thread %logger %ndc -%message%newline");
            LevelMatchFilter val = new LevelMatchFilter();
            val.LevelToMatch = Level.All;
            ((FilterSkeleton)val).ActivateOptions();
            RollingFileAppender val2= new RollingFileAppender();
            ((FileAppender)val2).File = Path.Combine(logFolderPath, featureName + ".log");
            ((TextWriterAppender)val2).ImmediateFlush = true;
            ((FileAppender)val2).AppendToFile = true;
            val2.RollingStyle = (RollingMode)2;
            val2.DatePattern = "-yyyy-MM-dd";
            ((FileAppender)val2).LockingModel = (LockingModelBase)new ExclusiveLock();
            ((AppenderSkeleton)val2).Name = featureName + "Appender";
            ((AppenderSkeleton)val2).AddFilter((IFilter)(object)val);
            ((AppenderSkeleton)val2).Layout = (ILayout)(object)layout;
            ((AppenderSkeleton)val2).ActivateOptions();
            string text = featureName + "Repository";
            ILoggerRepository val3 = LoggerManager.CreateRepository(text);
            BasicConfigurator.Configure(val3, (IAppender)(object)val2);

        }
        public ILog GetCurrentLogger(string featureName)
        {
            return LogManager.GetLogger(featureName + "Repository", featureName);
        }


    }
}
