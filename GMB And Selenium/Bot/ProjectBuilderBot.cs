using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using GMB_And_Selenium.Exceptions;
using GMB_And_Selenium.Models;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace GMB_And_Selenium.Bot
{
    class ProjectBuilderBot
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly ProjectData _projectData;
        private readonly PhoneNumberProvider _phoneNumberProvider;
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _longWait;
        private readonly WebDriverWait _shortWait;

        public ProjectBuilderBot(ProjectData projectData,
            PhoneNumberProvider phoneNumberProvider)
        {
            if (Properties.Settings.Default.BROWSER.ToUpper() == "FIREFOX")
            {
                _driver = new FirefoxDriver();
                _driver.Manage().Window.Maximize();
            }
            else
            {
                var options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                _driver = new ChromeDriver(options);
            }


            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);

            _longWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _shortWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(4));

            _projectData = projectData;
            _phoneNumberProvider = phoneNumberProvider;
        }

        public void Terminate()
        {
            _logger.Info("Terminating Selenium driver...");
            _driver.Quit();
        }

        private void ProcessLoginPage()
        {
            try
            {
                _driver.Navigate().GoToUrl("https://accounts.google.com/ServiceLogin?continue=https://accounts.google.com/ManageAccount&rip=1&nojavascript=1&hl=en#identifier");
                var element = _longWait.Until(ExpectedConditions.ElementToBeClickable(By.Id("Email")));
                element.Click();
                element.SendKeys(_projectData.Email);
                element.SendKeys(Keys.Enter);
                element = _longWait.Until(ExpectedConditions.ElementToBeClickable(By.Id("Passwd")));
                element.Click();
                element.SendKeys(_projectData.Password);
                element.SendKeys(Keys.Enter);
            }
            catch (Exception e)
            {
                _logger.Error("Error in <ProcessLoginPage>");
                _logger.Error(e);
                Debug.WriteLine(e);
                throw;
            }
        }

        private void SelectItem(string xpathSelector, string xpathItem, string initial)
        {
            var element = _driver.FindElement(By.XPath(xpathSelector));
            element.Click();
            //_driver.ExecuteScript("arguments[0].click();", element);
            Thread.Sleep(2000);

            var builder = new Actions(_driver);
            builder.SendKeys(initial.Trim());
            builder.Perform();

            Thread.Sleep(2000);
            var child = _driver.FindElement(By.XPath(xpathItem));
            //_driver.ExecuteScript("arguments[0].click();", child);
            child.Click();
            Thread.Sleep(2000);
        }

        private void ProcessBusinessNamePage()
        {
            try
            {
                Thread.Sleep(3000);
                _driver.Navigate().GoToUrl("https://business.google.com/create?hl=en&nojavascript=1");

                if (!TryInput(TypeBusinessName))
                    throw new Exception("Unable to type business name");

            }
            catch (Exception ex)
            {
                _logger.Error("Error in <ProcessBusinessNamePage>");
                _logger.Error(ex);
                Debug.WriteLine(ex);
                throw;
            }
        }

        private void ProcessContactDetailsPage()
        {
            try
            {
                if (!TryInput(InputContactDetails))
                {
                    throw new Exception("Unable to input contact details.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error in <ProcessContactDetailsPage>");
                _logger.Error(ex);
                Debug.WriteLine(ex);
                throw;
            }
        }

        private void ProcessBusinessCategoryPage()
        {
            try
            {
                if (!TryInput(InputBusinessCategory))
                {
                    throw new Exception("Unable to input business category.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error in <ProcessBusinessCategoryPage>");
                _logger.Error(ex);
                Debug.WriteLine(ex);
                throw;
            }
        }

        private void ProcessLocationPages()
        {
            try
            {
                _shortWait.Until(ExpectedConditions.ElementExists(By.XPath(Properties.Settings.Default.DELIVER_GOODS)));
                // Select Country
                _logger.Info("Trying InputCountry");
                if (!TryInput(InputCountry))
                {
                    throw new Exception("Input Failure: Country");
                }

                Thread.Sleep(2);

                _logger.Info("Trying InputZipCode");
                if (!TryInput(InputZipCode))
                {
                    throw new Exception("Input Failure: Zip Code");
                }

                _logger.Info("Trying InputCity");
                if (!TryInput(InputCity))
                {
                    Debug.WriteLine("Input Failure : City");
                }

                _logger.Info("Trying InputState");
                if (!TryInput(InputState))
                {
                    throw new Exception("Input Failure: State");
                }

                _logger.Info("Trying InputStreetAddress");
                if (!TryInput(InputStreetAddress))
                {
                    throw new Exception("Input Failure: Street Address");
                }

                ClickNext();
            }
            catch (Exception ex)
            {
                _logger.Error("Error in <ProcessLocationPages>");
                _logger.Error(ex);
                Debug.WriteLine(ex);
                throw;
            }
        }

        private async void InputContactDetails()
        {
            var phone = await _phoneNumberProvider.GetNumberAsync();
            if (!phone.StartsWith("+"))
                phone = "+" + phone;

            var element = _shortWait.Until(ExpectedConditions.ElementExists(By.XPath("//input[@type='tel']")));
            element.SendKeys(phone);
            Thread.Sleep(1000);
            ClickNext();
        }

        private void InputBusinessCategory()
        {
            _longWait.Until(ExpectedConditions.TitleContains("Choose the category that fits"));
            var element = _driver.FindElement(By.XPath("//input[@type='text' and @aria-label='Business category']"));
            element.SendKeys(_projectData.Category);
            Thread.Sleep(1000);
            ClickNext();
        }

        private void ClickNext()
        {
            var next = _driver.FindElement(By.XPath("(//div[@role='button']//span[contains(text(),'Next')])[1]"));
            next.Click();
            Thread.Sleep(2000);
        }

        private void WaitClickFinish()
        {
            var next = _longWait.Until(
                ExpectedConditions.ElementExists(
                    By.XPath("(//div[@role='button']//span[contains(text(),'Finish')])[1]")));
            next.Click();
            Thread.Sleep(2000);
        }

        private void ClickFinish()
        {
            var next = _driver.FindElement(By.XPath("(//div[@role='button']//span[contains(text(),'Finish')])[1]"));
            next.Click();
            Thread.Sleep(2000);
        }

        private void WaitClickNext()
        {
            var next = _longWait.Until(ExpectedConditions.ElementExists(By.XPath("(//div[@role='button']//span[contains(text(),'Next')])[1]")));
            next.Click();
            Thread.Sleep(2000);
        }

        private void InputCountry()
        {
            var initial = _projectData.Country.Split(" ".ToCharArray())[0];

            SelectItem($"//*[@role='option' and @aria-label='{_projectData.Country}']/..",
                $"//div[@role='option' and @aria-label='{_projectData.Country}']", initial);
        }

        private void InputStreetAddress()
        {
            var streetAddress = _driver.FindElement(By.XPath("//input[@aria-label='Street address']"));
            //streetName.Click();
            streetAddress.SendKeys(_projectData.StreetAddress ?? "None");
        }

        private void InputSuburb()
        {
            var suburb =
                _longWait.Until(ExpectedConditions.ElementExists(By.XPath("//input[@aria-label='Suburb']")));
            //suburb.Click();
            suburb.SendKeys(_projectData.Suburn ?? "None");
        }



        private void InputCity()
        {
            var city = _driver.FindElement(By.XPath("//input[@aria-label='City']"));
            //city.Click();
            city.SendKeys(_projectData.City ?? "None");
        }

        private void InputState()
        {
            var initial = _projectData.State.Split(" ".ToCharArray())[0];
            // Select Province
            SelectItem(string.Format(Properties.Settings.Default.STATE_PARENT, _projectData.State),
                string.Format(Properties.Settings.Default.STATE_CHILD, _projectData.State), initial);
        }

        private void InputZipCode()
        {
            var postal = _driver.FindElement(By.XPath("//input[@aria-label='ZIP code']"));
            //postal.Click();
            postal.SendKeys(_projectData.Zip ?? "None");
        }





        private void InputProvince()
        {
            var initial = _projectData.Province.Split(" ".ToCharArray())[0];

            // Select Province
            SelectItem($"//div[@aria-label='Province']/div[1]",
                $"//div[contains(@class,'mda pm')]//div[@aria-label='{_projectData.Province}']",
                initial);
        }

        private bool TryInput(Action action)
        {
            try
            {
                action();
                Thread.Sleep(1000);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Error in <TryInput>");
                _logger.Error(ex);

                return false;
            }

        }

        private void InputDistrict()
        {
            var district =
                _longWait.Until(ExpectedConditions.ElementExists(By.XPath("//input[@aria-label='District']")));
            //district.Click();
            district.SendKeys(_projectData.District ?? "None");
        }

        private void TypeBusinessName()
        {
            var businessName = _driver.FindElement(By.XPath("//input[@aria-label='Business name']"));
            businessName.SendKeys(_projectData.BusinessName);
            Thread.Sleep(2000);
            businessName.SendKeys(Keys.Enter);
        }

        private void TypeCredentials()
        {


        }

        private void GetOut()
        {
            var otherAccount = _longWait.Until(ExpectedConditions.ElementExists(By.Id("profileIdentifier")));
            otherAccount.Click();
            otherAccount = _longWait.Until(ExpectedConditions.ElementExists(By.Id("identifierLink")));
            otherAccount.Click();
        }

        private bool CheckIfAutoSelectedUserAccount()
        {
            try
            {
                var password = _longWait.Until(ExpectedConditions.ElementExists(By.Name("password")));
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        public Task StartAsync()
        {
            return Task.Run(() => Start());
        }

        private void ProcessIfMapSelect()
        {
            try
            {
                _shortWait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.TagName("p"), "Drag and zoom the map and position the marker on the exact spot where your business is located."));
                ClickNext();
            }
            catch (Exception)
            {
                _logger.Warn("Error in <ProcessIfMapSelect>\nPossibly skipped Map Select Page.");
            }
        }

        private void ProcessFinishAndVerifyPage()
        {
            try
            {
                _longWait.Until(ExpectedConditions.TitleContains("Finish and verify this business"));
                ClickFinish();
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to find Finish Button in <ProcessFinishAndVerifyPage>");
                _logger.Error(ex);
                Debug.WriteLine(ex);
                throw;
            }
        }

        public void Start()
        {
            ProcessLoginPage();

            while (true)
            {
                try
                {
                    ProcessBusinessNamePage();
                    ProcessLocationPages();
                    ProcessIfMapSelect();
                    ConfirmLocationPages();
                    ProcessBusinessCategoryPage();
                    ProcessContactDetailsPage();
                    ProcessFinishAndVerifyPage();
                }
                catch (PhoneVerifyNotTriggeredException)
                { }
                catch (Exception ex)
                {
                    _logger.Error("Task did not complet!");

                    Terminate();
                    Debug.WriteLine(ex);
                    //throw;
                    break;
                }
            }
        }

        private void ConfirmLocationPages()
        {
            try
            {
                _shortWait.Until(ExpectedConditions.TitleContains("Is this your business?"));
                var element = _driver.FindElement(By.XPath("(//div[@role='radio'])[last()]"));
                element.Click();
                ClickNext();
            }
            catch (Exception ex)
            {
                _logger.Error("Error from <ConfirmLocationPages>");
                _logger.Error(ex);
            }
            finally
            {
                //WaitClickNext();
            }
        }
    }
}

