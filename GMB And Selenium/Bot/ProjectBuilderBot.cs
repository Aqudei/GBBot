﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using GMB_And_Selenium.Models;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace GMB_And_Selenium.Bot
{
    class ProjectBuilderBot
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

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
            _logger.Info("Terminating Selenium driver...");
            _driver.Quit();
        }

        private void ProcessLoginPage()
        {
            try
            {
                _driver.Navigate().GoToUrl("https://accounts.google.com/ServiceLogin?continue=https://accounts.google.com/ManageAccount&rip=1&nojavascript=1&hl=en#identifier");
                var element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("Email")));
                element.Click();
                element.SendKeys(_projectData.Email);
                Thread.Sleep(2000);
                element.SendKeys(Keys.Enter);
                element = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("Passwd")));
                element.Click();
                element.SendKeys(_projectData.Password);
                Thread.Sleep(2000);
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
            var element = _driver.FindElementByXPath(xpathSelector);
            //element.Click();
            _driver.ExecuteScript("arguments[0].click();", element);
            Thread.Sleep(2000);

            var builder = new Actions(_driver);
            builder.SendKeys(initial.Trim());
            builder.Perform();

            Thread.Sleep(2000);
            var child = _driver.FindElementByXPath(xpathItem);
            _driver.ExecuteScript("arguments[0].click();", child);
            //child.Click();
            Thread.Sleep(2000);
        }

        private void ProcessBusinessNamePage()
        {
            try
            {
                _wait.Until(ExpectedConditions.TitleContains("Account"));
                _driver.Navigate().GoToUrl("https://business.google.com/create?hl=en");

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
                _wait.Until(ExpectedConditions.TitleContains("Where are you located?"));
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

                //if (!TryInput(InputDistrict))
                //{
                //    Debug.WriteLine("Input Failure : District");
                //}

                //if (!TryInput(InputSuburb))
                //{
                //    Debug.WriteLine("Input Failure : Suburb");
                //}

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

                //if (!TryInput(InputProvince))
                //{
                //    throw new Exception("Input Failure : Province");
                //}

                ClickNext();
                //WaitClickNext();
            }
            catch (Exception ex)
            {
                _logger.Error("Error in <ProcessLocationPages>");
                _logger.Error(ex);
                Debug.WriteLine(ex);
                throw;
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
            var initial = _projectData.Country.Split(" ".ToCharArray())[0];

            SelectItem("(//div[@role='listbox']/div/div)[1]",
                            $"//div[@role='option' and @aria-label='{_projectData.Country}']", initial);
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
            var initial = _projectData.Province.Split(" ".ToCharArray())[0];

            // Select Province
            SelectItem($"//div[@aria-label='Province']/div[1]",
                $"//div[contains(@class,'mda pm')]//div[@aria-label='{_projectData.Province}']",
                initial);
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
        }

        private void TypeCredentials()
        {


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
            try
            {
                ProcessLoginPage();
                ProcessBusinessNamePage();
                ProcessLocationPages();
                ProcessIfMapSelect();
                ConfirmLocationPages();
                ProcessBusinessCategoryPage();
                ProcessContactDetailsPage();
                ProcessFinishAndVerifyPage();
            }
            catch (Exception ex)
            {
                _logger.Error("Task did not complet!");

                Terminate();
                Debug.WriteLine(ex);
                //throw;
            }
        }

        private void ProcessIfMapSelect()
        {
            try
            {
                _wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.TagName("p"), "Drag and zoom the map and position the marker on the exact spot where your business is located."));
                ClickNext();
            }
            catch (Exception ex)
            {
                _logger.Error("Error in <ProcessIfMapSelect>\nPossibly skipped Map Select Page.");
            }
        }

        private void ProcessFinishAndVerifyPage()
        {
            try
            {
                _wait.Until(ExpectedConditions.TitleContains("Finish and verify this business"));
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

        private void ConfirmLocationPages()
        {
            try
            {
                _wait.Until(ExpectedConditions.TitleContains("Is this your business?"));
                var element = _driver.FindElementByXPath("(//div[@role='radio'])[last()]");
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

