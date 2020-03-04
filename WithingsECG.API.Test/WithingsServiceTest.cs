using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;
using WithingsECG.API.Controllers;

namespace WithingsECG.API.Test
{
    [TestClass]
    public class WithingsServiceTest
    {
        //we cannot use demo mode because the ECG is not filled in. We need a real account with real data.
        private static readonly bool DEMOMODE = false;
        private static readonly string USERNAME = "AREALUSERNAME";
        private static readonly string PASSWORD = "PASSWORD";

        IConfiguration _configuration { get; set; }

        public WithingsServiceTest()
        {
            // the type specified here is just so the secrets library can 
            // find the UserSecretId we added in the csproj file
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<WithingsServiceTest>();

            _configuration = builder.Build();
        }


        [TestMethod]
        public void GetECG()
        {
            WithingsService withingsService = new WithingsService(_configuration);
            using IWebDriver driver = CreateDriver(false);
            var authUrl = DEMOMODE ? $"{withingsService.AuthUrl}&mode=demo" : withingsService.AuthUrl;

            driver.Navigate().GoToUrl(authUrl);
            Login(driver);
            GiveConsent(driver);

            Assert.IsTrue(driver.Url.Contains("code="));
            var code = driver.Url.Split("code=")[1].Split("&")[0];
            Assert.IsNotNull(code);

            driver.Close();

            var token = withingsService.GetToken(code);
            Assert.IsNotNull(token);
            Assert.IsFalse(string.IsNullOrEmpty(token.access_token));

            //no ecg's in demo mode.
            if (DEMOMODE)
                return;

            var ecgs = withingsService.ListECGs(token.access_token);
            Assert.IsTrue(ecgs.body.series.Length > 0);
            var ecg = withingsService.GetECG(token.access_token, ecgs.body.series.First().ecg.signalid);
            Assert.IsTrue(ecg.body.signal.Length > 0);
        }

        private static IWebDriver CreateDriver(bool headless = true)
        {
            var chromeOptions = new ChromeOptions();
            if (headless)
                chromeOptions.AddArguments("headless");
            IWebDriver driver = new ChromeDriver("webdriver/", chromeOptions);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return driver;
        }

        private static void GiveConsent(IWebDriver driver)
        {
            driver.FindElement(By.TagName("form")).FindElement(By.TagName("button")).Click();
        }

        private static void Login(IWebDriver driver)
        {
            //no need to login in demo mode;
            if (DEMOMODE) return;

            driver.FindElement(By.Name("email")).SendKeys(USERNAME);
            driver.FindElement(By.Name("password")).SendKeys(PASSWORD);
            driver.FindElement(By.XPath("//div[contains(@class,'createButton')]")).FindElement(By.TagName("button")).Click();
        }
    }
}
