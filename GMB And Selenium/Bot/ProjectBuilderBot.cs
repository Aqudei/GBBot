using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using GMB_And_Selenium.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace GMB_And_Selenium.Bot
{
    class ProjectBuilderBot
    {
        private readonly ProjectData _projectData;
        private readonly ChromeDriver _driver;
        private readonly WebDriverWait _wait;

        public ProjectBuilderBot(ProjectData projectData)
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            _projectData = projectData;
            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);

            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        public void Terminate()
        {
            _driver.Quit();
        }

        private void DoLogin()
        {
            _driver.Navigate().GoToUrl("https://accounts.google.com/ServiceLogin?continue=https://accounts.google.com/ManageAccount&rip=1&nojavascript=1&hl=en#identifier");
            //if (CheckIfAutoSelectedUserAccount())
            //{
            //    GetOut();
            //}

            try
            {
                TypeCredentials();
                WhereAreYouLocatedPage();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }

        private void SelectItem(string xpathSelector, string xpathItem, string initial)
        {
            var element = _driver.FindElementByXPath(xpathSelector);
            //element.Click();
            _driver.ExecuteScript("arguments[0].click();", element);
            Thread.Sleep(1000);

            var builder = new Actions(_driver);
            builder.SendKeys(initial);
            builder.Perform();

            Thread.Sleep(1000);
            var child = _driver.FindElementByXPath(xpathItem);
            _driver.ExecuteScript("arguments[0].click();", child);
            //child.Click();
        }

        private void WhereAreYouLocatedPage()
        {
            _driver.Navigate().GoToUrl("https://business.google.com/create?hl=en");

            if (!TryInput(TypeBusinessName))
                throw new Exception("Unable to type business name");

            // Select Country
            if (!TryInput(InputCountry))
                throw new Exception("Unable to select country");

            Thread.Sleep(2);

            if (!TryInput(InputZipCode))
            {
                Debug.WriteLine("Input Failure : Zip Code");
            }

            //if (!TryInput(InputDistrict))
            //{
            //    Debug.WriteLine("Input Failure : District");
            //}

            //if (!TryInput(InputSuburb))
            //{
            //    Debug.WriteLine("Input Failure : Suburb");
            //}

            if (!TryInput(InputCity))
            {
                Debug.WriteLine("Input Failure : City");
            }

            if (!TryInput(InputState))
            {
                throw new Exception("Unable to select state");
            }

            if (!TryInput(InputStreetAddress))
            {
                Debug.WriteLine("Input Failure : StreetName");
            }

            //if (!TryInput(InputProvince))
            //{
            //    throw new Exception("Input Failure : Province");
            //}

            ClickNext();
            WaitClickNext();

            if (!TryInput(InputBusinessCategory))
            {
                throw new Exception("Unable to input business category.");
            }

            if (!TryInput(InputContactDetails))
            {
                throw new Exception("Unable to input contact details.");
            }
            
        }

        private void InputContactDetails()
        {
            _wait.Until(ExpectedConditions.TitleContains("What contact details do you want to show"));
            var element = _driver.FindElementByXPath("//input[@type='tel' and @aria-label='Contact phone number']");
            element.SendKeys(_projectData.Phone);
            Thread.Sleep(1000);
            ClickNext();
        }

        private void InputBusinessCategory()
        {
            _wait.Until(ExpectedConditions.TitleContains("Choose the category that fits"));
            var element = _driver.FindElementByXPath("//input[@type='text' and @aria-label='Business category']");
            element.SendKeys(_projectData.Category);
            Thread.Sleep(1000);
            ClickNext();
        }

        private void ClickNext()
        {
            var next = _driver.FindElementByXPath("(//div[@role='button']//span[contains(text(),'Next')])[1]");
            next.Click();
            Thread.Sleep(2000);
        }

        private void WaitClickFinish()
        {
            var next = _wait.Until(
                ExpectedConditions.ElementExists(
                    By.XPath("(//div[@role='button']//span[contains(text(),'Finish')])[1]")));
            next.Click();
            Thread.Sleep(2000);
        }

        private void ClickFinish()
        {
            var next = _driver.FindElementByXPath("(//div[@role='button']//span[contains(text(),'Finish')])[1]");
            next.Click();
            Thread.Sleep(2000);
        }

        private void WaitClickNext()
        {
            var next = _wait.Until(ExpectedConditions.ElementExists(By.XPath("(//div[@role='button']//span[contains(text(),'Next')])[1]")));
            next.Click();
            Thread.Sleep(2000);
        }

        private void InputCountry()
        {
            SelectItem("(//div[@aria-label='Country / Region']//div)[1]",
                            $"//div[contains(@class,'mda pm')]//div[@aria-label='{_projectData.Country}']",
                            _projectData.Country.Substring(0, 3));
        }

        private void InputSuburb()
        {
            var suburb =
                _wait.Until(ExpectedConditions.ElementExists(By.XPath("//input[@aria-label='Suburb']")));
            //suburb.Click();
            suburb.SendKeys(_projectData.Suburn ?? "None");
        }

        private void InputProvince()
        {
            // Select Province
            SelectItem($"//div[@aria-label='Province']/div[1]",
                $"//div[contains(@class,'mda pm')]//div[@aria-label='{_projectData.Province}']",
                _projectData.Province.Substring(0, 3));
        }

        private void InputState()
        {
            // Select Province
            SelectItem($"(//div[@aria-label='State']//div)[1]",
                $"//div[@class='mda pm']//div[@aria-label='{_projectData.State}']",
                _projectData.State.Substring(0, 3));
        }

        private void InputZipCode()
        {
            var postal = _driver.FindElementByXPath("//input[@aria-label='ZIP code']");
            //postal.Click();
            postal.SendKeys(_projectData.Zip ?? "None");
        }

        private void InputCity()
        {
            var city = _driver.FindElementByXPath("//input[@aria-label='City']");
            //city.Click();
            city.SendKeys(_projectData.City ?? "None");
        }

        private void InputStreetAddress()
        {
            var streetAddress = _driver.FindElementByXPath("//input[@aria-label='Street address']");
            //streetName.Click();
            streetAddress.SendKeys(_projectData.StreetAddress ?? "None");
        }

        private bool TryInput(Action action)
        {
            try
            {
                action();
                Thread.Sleep(1000);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        private void InputDistrict()
        {
            var district =
                _wait.Until(ExpectedConditions.ElementExists(By.XPath("//input[@aria-label='District']")));
            //district.Click();
            district.SendKeys(_projectData.District ?? "None");
        }

        private void TypeBusinessName()
        {
            var businessName = _driver.FindElementByXPath("//input[@aria-label='Business name']");
            businessName.SendKeys(_projectData.BusinessName);
            Thread.Sleep(2000);
            businessName.SendKeys(Keys.Enter);
            Thread.Sleep(1000);
            _wait.Until(ExpectedConditions.TitleContains("Where are you located?"));
        }

        private void TypeCredentials()
        {
            var element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("Email")));
            element.Click();
            element.SendKeys(_projectData.Email);
            //var next = _wait.Until(ExpectedConditions.ElementExists(By.Id("identifierNext")));
            //next.Click();
            Thread.Sleep(2000);
            element.SendKeys(Keys.Enter);
            element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("Passwd")));
            element.Click();
            element.SendKeys(_projectData.Password);
            Thread.Sleep(2000);
            element.SendKeys(Keys.Enter);
            //next = _wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@id='passwordNext']/content/span")));
            //next.Click();
            _wait.Until(ExpectedConditions.TitleIs("Google Account"));
        }

        private void GetOut()
        {
            var otherAccount = _wait.Until(ExpectedConditions.ElementExists(By.Id("profileIdentifier")));
            otherAccount.Click();
            otherAccount = _wait.Until(ExpectedConditions.ElementExists(By.Id("identifierLink")));
            otherAccount.Click();
        }

        private bool CheckIfAutoSelectedUserAccount()
        {
            try
            {
                var password = _wait.Until(ExpectedConditions.ElementExists(By.Name("password")));
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


        public void Start()
        {
            DoLogin();
        }

    }
}

