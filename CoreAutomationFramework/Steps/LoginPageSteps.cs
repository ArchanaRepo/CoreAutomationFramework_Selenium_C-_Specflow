using CoreAutomationFramework.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.NUnit;

namespace CoreAutomationFramework.Steps
{
    [Binding]
    public class LoginPageSteps : BaseStep
    {
        private readonly ParallelConfig _pconfig;
        public LoginPageSteps(ParallelConfig pconfig) : base(pconfig)
        {
            _pconfig = pconfig;
        }
        [Given(@"I am on the Flipkart login page")]
        public void GivenIAmOnTheFlipkartLoginPage()
        {
            Console.WriteLine("I am on the Flipkart login page");
        }

        [When(@"I enter valid username ""(.*)"" and password ""(.*)""")]
        public void WhenIEnterValidUsernameAndPassword(string username, string password)
        {
            Console.WriteLine("I enter valid username \"\"(.*)\"\" and password \"\"(.*)\"");
        }

        [When(@"I enter invalid username ""(.*)"" and password ""(.*)""")]
        public void WhenIEnterInvalidUsernameAndPassword(string username, string password)
        {
            Console.WriteLine("I enter invalid username \"(.*)\" and password \"(.*)\"");
        }

        [When(@"I click the login button")]
        public void WhenIClickTheLoginButton()
        {
            Console.WriteLine("I click the login button");
        }

        [Then(@"I should be logged in successfully")]
        public void ThenIShouldBeLoggedInSuccessfully()
        {
            Console.WriteLine("I should be logged in successfully");
        }

    }
}
