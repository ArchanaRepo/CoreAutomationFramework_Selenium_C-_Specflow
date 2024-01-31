using CoreAutomationFramework.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace CoreAutomationFramework.Pages
{
    [Binding]
    public class LoginPage : BasePage
    {
        private readonly ParallelConfig _pconfig;
        public LoginPage(ParallelConfig pconfig):base(pconfig)
        {
            _pconfig = pconfig;
        }
    }
}
