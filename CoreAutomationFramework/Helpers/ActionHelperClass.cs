using CoreAutomationFramework.Base;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;



namespace CoreAutomationFramework.Helpers
{
    public class ActionHelperClass
    {
        public enum EnumWaitForElement
        {
            VISIBILITY,
            CLICKABLE,
            INVISIBILITY
        }
        public enum EnumForPageScroll
        {
            PAGEUP,
            PAGEDOWN
        }
        public enum EnumForDropdownSelectionType
        {
            TEXT,
            VALUE,
            INDEX
        }
        public enum EnumForAlert
        {
            ACCEPT,
            DISMISS,
            GETTEXT
        }
        public enum EnumForValidateWebTableLoad
        {
            GREATERTHANOREQUAL,
            GREATERTHAN,
            MATCHES
        }
        public enum EnumForVerifyTextInObject
        {
            MATCHES,
            CONTAINS,
            TEXT
        }
        public enum EnumForActionKey
        {
            END,
            HOME,
            ENTER,
            PAGEUP,
            PAGEDOWN,
            SHIFT,
            TAB,
            UP,
            DOWN
        }
        private readonly ParallelConfig _parallelConfig;

        public ActionHelperClass(ParallelConfig parallelConfig)
        {
            _parallelConfig = parallelConfig;
            _parallelConfig.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(120.0);
        }
        public void ScrollToElement(By element)
        {
            IWebElement webElement = _parallelConfig.Driver.FindElement(element);
            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)_parallelConfig.Driver;
            javaScriptExecutor.ExecuteScript("arguments[0].scrollIntoView({block : 'center'});", webElement);

        }

        public void WaitForPageLoad()
        {
            bool flag = false;
            IJavaScriptExecutor javascriptExecutor = (IJavaScriptExecutor)_parallelConfig.Driver;
            do
            {
                flag = javascriptExecutor.ExecuteScript("document.readyState").Equals("Complete");
                if (flag)
                {
                    break;
                }
                Thread.Sleep(2000);
            } while (!flag);

        }

        public bool WaitForElement(By element, EnumWaitForElement _waitType, int timeout = 30)
        {
            bool result = false;
            WebDriverWait webDriverWait = new WebDriverWait(_parallelConfig.Driver, TimeSpan.FromSeconds(timeout));
            try
            {
                switch (_waitType)
                {
                    case EnumWaitForElement.VISIBILITY:
                        //webDriverWait.Until(ExpectedConditions.ElementIsVisible(element));
                        result = true;
                        break;
                    case EnumWaitForElement.CLICKABLE:
                        //webDriverWait.Until(ExpectedConditions.ElementIsClickablle(element));
                        result = true;
                        break;
                    case EnumWaitForElement.INVISIBILITY:
                        //webDriverWait.Until(ExpectedConditions.ElementIsINVASIBILITY(element));
                        result = true;
                        break;
                }
            }
            catch (Exception e)
            {
                result = false;
                throw new Exception("Expected Condition for the element " + element?.ToString() + " is not satisfied");
            }
            return result;
        }

        public void Click(By element)
        {
            try
            {
                WaitForElement(element, EnumWaitForElement.VISIBILITY);
                WaitForElement(element, EnumWaitForElement.CLICKABLE);
                ScrollToElement(element);
                IWebElement webElement = _parallelConfig.Driver.FindElement(element);
                if (webElement.Displayed && webElement.Enabled)
                {
                    webElement.Click();
                    return;
                }
                throw new Exception("Expected Condition for the element " + element?.ToString() + " is not satisfied");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Set(By element, string ValueToBeEntered)
        {
            try
            {
                WaitForElement(element, EnumWaitForElement.VISIBILITY);
                WaitForElement(element, EnumWaitForElement.CLICKABLE);
                ScrollToElement(element);
                IWebElement webElement = _parallelConfig.Driver.FindElement(element);
                if (webElement.Displayed && webElement.Enabled)
                {
                    webElement.Click();
                    webElement.Clear();
                    webElement.SendKeys(ValueToBeEntered);
                    return;
                }
                throw new Exception("Expected Condition for the element " + element?.ToString() + " is not satisfied");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }

        public void AutoSuggestions(By element, string valueToBeEnteredAndSelected)
        {
           
            try
            {
                WaitForElement(element, EnumWaitForElement.VISIBILITY);
                WaitForElement(element, EnumWaitForElement.CLICKABLE);
                ScrollToElement(element);
                IWebElement webElement = _parallelConfig.Driver.FindElement(element);
                if (webElement.Displayed && webElement.Enabled)
                {
                    string[] array = valueToBeEnteredAndSelected.Split(',');
                    string[] array2 = array;
                    foreach(string text in array2)
                    {
                        char[] array3=text.ToCharArray();
                        char[] array4= array3;
                        foreach(char c in array4)
                        {
                            webElement.SendKeys(c.ToString());
                            Thread.Sleep(1000);
                        }
                        webElement.SendKeys(Keys.Enter);
                    }
                    webElement.SendKeys(Keys.Tab);
                    return;
                }
                throw new Exception(element?.ToString() + " is not enablled or displayed in UI");

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public void MoveScrollVertically(EnumForPageScroll direction)
        {
            Actions action = new Actions(_parallelConfig.Driver);
            if(direction.Equals(EnumForPageScroll.PAGEUP))
            {
                action.SendKeys(Keys.PageUp).Build().Perform();
            }
            else if(direction.Equals(EnumForPageScroll.PAGEDOWN))
            {
                action.SendKeys(Keys.PageDown).Build().Perform();
            }
        }

        public void Actions_Click(By element)
        {
            Actions action = new Actions(_parallelConfig.Driver);
            try
            {
                WaitForElement(element, EnumWaitForElement.VISIBILITY);
                WaitForElement(element, EnumWaitForElement.CLICKABLE);
                ScrollToElement(element);
                IWebElement webElement = _parallelConfig.Driver.FindElement(element);
                if (webElement.Displayed && webElement.Enabled)
                {
                    action.MoveToElement(webElement).Click().Build().Perform();

                    return;
                }

                    throw new Exception(element?.ToString() + " is not enablled or displayed in UI");
            }
            catch(Exception e) 
            {
                Console.WriteLine(e.Message);
            }
        }

        public string GetCellText(string element,int row,int col)
        {
            string result = null;
            By element2 = By.XPath(element);
            if (WaitForElement(element2, EnumWaitForElement.VISIBILITY))
            {
                By by = By.XPath(element + "/tr[" + row + "]/td[" + col + "]");
                IWebElement webElement = _parallelConfig.Driver.FindElement(by);
                result = webElement.Text;
            }
            return result;
        }

        public void SelectFromDropDown(By element,string valueToBeSelected,EnumForDropdownSelectionType selectinType)
        {
            try
            {
                if(WaitForElement(element,EnumWaitForElement.VISIBILITY))
                {
                    SelectElement selectElement = new SelectElement(_parallelConfig.Driver.FindElement(element));
                    switch(selectinType)
                    {
                        case EnumForDropdownSelectionType.TEXT:
                            selectElement.SelectByText(valueToBeSelected);
                            break;
                        case EnumForDropdownSelectionType.VALUE:
                            selectElement.SelectByValue(valueToBeSelected);
                            break;
                        case EnumForDropdownSelectionType.INDEX:
                            {
                                int index = Convert.ToInt32(valueToBeSelected);
                                selectElement.SelectByIndex(index);
                                break;
                                
                            }
                        default:
                            selectElement.SelectByText(valueToBeSelected);
                            break;
                    }
                    
                }
              
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        public void Alert(EnumForAlert operation , string expectedText=null)
        {
            switch(operation) 
            {
                case EnumForAlert.ACCEPT:
                    _parallelConfig.Driver.SwitchTo().Alert().Accept();
                    break;
                case EnumForAlert.DISMISS:
                    _parallelConfig.Driver.SwitchTo().Alert().Dismiss();
                    break;
                case EnumForAlert.GETTEXT:
                    {
                        string text = _parallelConfig.Driver.SwitchTo().Alert().Text;
                        Assert.That(text,Is.EqualTo(expectedText));
                        break;
                    }
                default:
                    _parallelConfig.Driver.SwitchTo().Alert().Accept();
                    break;
            
            
            }
        }

        public Dictionary<string , string> ReadDataFromVerticalSpecFlowTableToDictionary(Table table)
        {
           if(table==null||table.Rows.Count==0) return null;
           Dictionary<string,string> dictionary=new Dictionary<string,string>();
           for(int i=0;i<table.Rows.Count;i++)
            {
                dictionary.Add(table.Rows[0].ToString(), table.Rows[1].ToString());

            }
           return dictionary;
       
        }

        public List<string> ReadDataFromSpecFlowTableToList(Table table)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                list.Add(table.Rows[i].ToString());

            }
            return list;
        }

        public bool IsElementFound(By _element)
        {
            try
            {
                IWebElement webElement = _parallelConfig.Driver.FindElement(_element);
                if(webElement.Displayed)
                {
                    return true;
                }
                return false;
            }
            catch(Exception) 
            {

                throw new Exception(_element?.ToString() + " element not found");

            }
        }

        public void SpinnerCheck(By _SpinnerIdentifer)
        {
            while(IsElementFound(_SpinnerIdentifer) && !WaitForElement(_SpinnerIdentifer,EnumWaitForElement.INVISIBILITY))
            {

            }
        }

        public bool Validate_WebTable_Load(By tableElement , EnumForValidateWebTableLoad type , int expectedCount)
        {
            bool result = false;
            int num = 1;
            try
            {
                while(num!=50)
                {
                    IWebElement webElement = _parallelConfig.Driver.FindElement(tableElement);
                    IList<IWebElement> list = webElement.FindElements(By.TagName("tr"));
                    switch(type)
                    {
                        case EnumForValidateWebTableLoad.GREATERTHANOREQUAL:
                            if(list.Count>= expectedCount)
                            {
                                result = true; break;
                            }
                            Thread.Sleep(2000);
                            num++;
                            break;
                        case EnumForValidateWebTableLoad.GREATERTHAN:
                            if(list.Count> expectedCount)
                            {
                                result = true; break;
                            }
                            Thread.Sleep(2000);
                            num++;
                            break;
                        case EnumForValidateWebTableLoad.MATCHES:
                            if(list.Count== expectedCount)
                            {
                                result = true;
                                break;
                            }
                            Thread.Sleep(2000);
                            num++;
                            break;
                        default:
                            result = false;
                            break;
                    }
                }
            }catch (Exception ex) 
            {
                result=false;
            }
            return result;
        }

        public dynamic VerifyTextInObject(By element,string expectedValue,EnumForVerifyTextInObject type)
        {
            object result = "";
            string text = null;
            try
            {
                if(!WaitForElement(element,EnumWaitForElement.VISIBILITY))
                {
                    throw new Exception("Expected Text " + expectedValue + " is missing with " + text);
                }
                IWebElement webElement = _parallelConfig.Driver.FindElement(element);
                text = webElement.Text;
                switch(type)
                {
                    case EnumForVerifyTextInObject.MATCHES:
                        result = ((!(expectedValue == text) ? (object)false : (object)true));
                        break;
                    case EnumForVerifyTextInObject.CONTAINS:
                        result=((!(text.Contains(expectedValue))?(object)false:(object)true));
                        break;
                    case EnumForVerifyTextInObject.TEXT:
                        result = text;
                        break;
                    default: result = false; break;
                }
            }catch(Exception ex)
            {
                result = false;
            }
            return result;
              
        }

        public string GetTableRowData(string cellValue , By tableElement=null)
        {
            string result = "";
            try
            {
                IList<IWebElement> list;
                if(tableElement==null)
                {
                    list = _parallelConfig.Driver.FindElements(By.CssSelector("tbody > tr"));
                }
                else
                {
                    IWebElement webElement = _parallelConfig.Driver.FindElement(tableElement);
                    list = webElement.FindElements(By.TagName("tr"));

                }
                foreach(IWebElement element in list) 
                {
                    if(element.Text.Contains(cellValue))
                    {
                        result=element.Text;

                    }
                }

            }catch(Exception ex) { Console.WriteLine(ex.Message); }

            return result;
        }

        public string GetElementText(By element)
        {
            try
            {
                if(WaitForElement(element,EnumWaitForElement.VISIBILITY))
                {
                    IWebElement webElement = _parallelConfig.Driver.FindElement(element);
                    return webElement.Text;
                }
                throw new Exception("Element is not visible on web page");

            }catch(Exception ex) { Console.WriteLine(ex.Message) ;
                throw;
            }

        }

        public string GetAttributeValue(By elemennt , string sttributeName)
        {
            try
            {
                if(WaitForElement(elemennt,EnumWaitForElement.VISIBILITY))
                {
                    IWebElement webElement = _parallelConfig.Driver.FindElement(elemennt);
                    return webElement.GetAttribute(sttributeName);
                }
                throw new Exception("Element is not visible on web page");

            }
            catch(Exception ex) { Console.WriteLine(ex.Message);throw; }
        }

        public  void Actions_KeyBoard(EnumForActionKey keyName)
        {
            Actions action=new Actions(_parallelConfig.Driver);
            try
            {
                switch(keyName)
                {
                    case EnumForActionKey.END:
                        action.SendKeys(Keys.End);
                        break;
                    case EnumForActionKey.HOME:
                        action.SendKeys(Keys.Home);
                        break;
                    case EnumForActionKey.ENTER:
                        action.SendKeys(Keys.Enter);
                        break;
                    case EnumForActionKey.PAGEUP:
                        action.SendKeys(Keys.PageUp);
                        break;
                    case EnumForActionKey.PAGEDOWN:
                        action.SendKeys(Keys.PageDown);
                        break;
                    case EnumForActionKey.SHIFT:
                        action.SendKeys(Keys.Shift);
                        break;
                    case EnumForActionKey.TAB:
                        action.SendKeys(Keys.Tab);
                        break;
                    case EnumForActionKey.UP:
                        action.SendKeys(Keys.Up);
                        break;
                }

            }catch(Exception ex) { throw new Exception(keyName.ToString() + " key click is unsuccessful, Exception => " + ex.Message); }

        }
        public void UploadAFile(By uploadLocator,string fileName)
        {
            DirectoryInfo parent = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent;
            string text = parent.FullName + "\\TestData\\" + fileName;
            IWebElement webElement = _parallelConfig.Driver.FindElement(uploadLocator);
            webElement.SendKeys(text);
            Thread.Sleep(2000);
        }
        public List<string> GetTextOfAllElements(By elementLocator)
        {
            ReadOnlyCollection<IWebElement> readOnlyCollection = null;
            List<string> list = null;
            try
            {
                WebDriverWait webDriverWait =  new WebDriverWait(_parallelConfig.Driver,TimeSpan.FromSeconds(30));
                //IReadOnlyCollection<IWebElement> readOnlyCollection = webDriverWait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(elementLocator));
                list=new List<string>();
                if(readOnlyCollection.Count > 0 && readOnlyCollection!=null)
                {
                    foreach(IWebElement element in readOnlyCollection)
                    {
                        if(element.Text!=""|| element.Text.Equals(null))
                        {
                            list.Add(element.Text);
                        }
                    }
                }

            }catch(Exception ex) { throw new Exception("The text list of web Elemennt cannot be generated"); }
            return list;
        }

        public  void FilterKendoGridColumn(string columnName , string filterOperator , string filterValue , string gridElementAttribute)
        {
            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)_parallelConfig.Driver;
            string gridReference = GetGridReference(gridElementAttribute);
            StringBuilder stringBuilder= new StringBuilder();
            stringBuilder.Append(gridReference);
            stringBuilder.Append("grid.dataSource.filter({ logic:\"and\",filters:[");
            int result = 0;
            if(!int.TryParse(filterValue, out result))
            {
                stringBuilder.Append("{ field: \"" + columnName + "\",operator:\"" + filterOperator + "\",value: \"" + filterValue + "\"},");

            }
            else
            {
                stringBuilder.Append("{ field: \"" + columnName + "\",operator:\"" + filterOperator + "\",value: \"" + filterValue + "},");

            }
            stringBuilder.Append("] });");
            gridReference = stringBuilder.ToString().Replace(",]", "]");
            javaScriptExecutor.ExecuteScript(gridReference);

        }

        public string GetGridReference(string gridElementAttribute)
        {
            return $"var grid = $('[options={gridElementAttribute}]').data('kendoGrid');";
        }


    }




    
}
