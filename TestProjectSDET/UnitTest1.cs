using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using OpenQA.Selenium.Interactions;

namespace TestProjectSDET
{
    public class Tests
    {
        private IWebDriver _driver;
        [SetUp]
        public void InitSetup()
        {
            //Launch the browser and load the URL
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://www.makemytrip.com");
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

        }
        public void ClickMethod(String icon)
        {
            //reusable method for clicking the icon
            _driver.FindElement(By.XPath($"//span[text()='{icon}']")).Click();
            _driver.FindElement(By.XPath("//a[text()='Search']")).Click();
            Console.WriteLine($"{icon} Page is loaded!...");

        }
        //Code for Flight
        [Test]
        public void TestForFlightsMethod()
        {
            ClickMethod("Flights");
            //Handled the ok button in the popup to display the search result page
            IWebElement element = _driver.FindElement(By.XPath("//button[text()='OKAY, GOT IT!']"));
            IJavaScriptExecutor executor = (IJavaScriptExecutor)_driver;
            executor.ExecuteScript("arguments[0].click();", element);
            Console.WriteLine("Handled the popup");

            //selecting Trip as "Round Trip"
            _driver.FindElement(By.XPath("//div[@class='multiDropDownVal']")).Click();
            _driver.FindElement(By.XPath("//ul[@class='dropDownList']/li[text()='Round Trip']")).Click();

            //Enter & Selecting "Chennai" as FROM place
            _driver.FindElement(By.XPath("//input[@placeholder='Enter City']")).SendKeys("Chennai");
            _driver.FindElement(By.XPath("(//p[@class='makeFlex blackText'])[1]")).Click();

            //Enter & Selecting "Bengaluru" as TO place
            _driver.FindElement(By.Id("toCity")).Click();
            _driver.FindElement(By.XPath("//input[@placeholder='Enter City']")).SendKeys("Bengaluru");
            _driver.FindElement(By.XPath("(//p[@class='makeFlex blackText'])[1]")).Click();

            //From & To Date of Travel
            _driver.FindElement(By.XPath("//div[contains(@aria-label,'18 July 2023')]")).Click();
            _driver.FindElement(By.Id("return")).Click();
            _driver.FindElement(By.XPath("//div[contains(@aria-label,'19 July 2023')]")).Click();

            //Enter Passenger & Class details note: used Business class since Premium economy is not listed for particular locations
            _driver.FindElement(By.Id("travellerAndClass")).Click();
            _driver.FindElement(By.XPath("//p[text()='ADULTS (12y +)']/following::li[text()='2']")).Click();
            _driver.FindElement(By.XPath("//div[@class='childCounter']//p/following::li[text()='1']")).Click();
            _driver.FindElement(By.XPath("//div[@class='pushRight infantCounter']//p/following::li[text()='1']")).Click();
            _driver.FindElement(By.XPath("//p[text()='CHOOSE TRAVEL CLASS']/following::li[text()='Business']")).Click();
            _driver.FindElement(By.XPath("//button[@type='submit']")).Click();

            //Search the results
            _driver.FindElement(By.Id("search-button")).Click();

            //Select the "Airlines" Flight type
            IWebElement flights = _driver.FindElement(By.XPath("//p[text()='Airlines']"));
            Actions airline = new Actions(_driver);
            airline.MoveToElement(flights).Build().Perform();
            //Handled the popup
            IWebElement element1 = _driver.FindElement(By.XPath("//button[text()='OKAY, GOT IT!']"));
            IJavaScriptExecutor executor1 = (IJavaScriptExecutor)_driver;
            executor1.ExecuteScript("arguments[0].click();", element1);
            Console.WriteLine("Handled the popup");

            //Verify the applied filter is displayed  note: used Air India since no flights found for Spiceject for particular location
            _driver.FindElement(By.XPath("//p[contains(text(),'Air India')]/preceding::input[1]")).Click();
            String actualTitle = _driver.FindElement(By.XPath("//p[text()='Flights from ']")).ToString();
            String expectedTitle = "Flights from Chennai to Bengaluru, and back";
            Assert.AreEqual(actualTitle, expectedTitle);
            Console.WriteLine(_driver.FindElement(By.XPath("//div[@class='filtersOuter']//p/following::li[text()='Air India']")).Text);
        
        }
        //Code for Train
        [Test]
        public void TestForTrainMethod()
        {
            ClickMethod("Trains");

            //Enter & Selecting "Chennai"
            _driver.FindElement(By.Id("fromCity")).Click();
            _driver.FindElement(By.XPath("//input[@placeholder='From']")).SendKeys("Chennai");
            _driver.FindElement(By.XPath("(//span[text()='Chennai, Tamil Nadu'])[2]")).Click();

            //Enter & Selecting "Bengaluru"
            _driver.FindElement(By.Id("toCity")).Click();
            _driver.FindElement(By.XPath("//input[@placeholder='To']")).SendKeys("Bengaluru");
            _driver.FindElement(By.XPath("(//span[text()='Bangalore, Karnataka'])[2]")).Click();

            //From & To Date
            _driver.FindElement(By.Id("travelDate")).Click();
            _driver.FindElement(By.XPath("//div[contains(@aria-label,'Jul 18 2023')]")).Click();
            _driver.FindElement(By.Id("travelFor")).Click();
            _driver.FindElement(By.XPath("//li[text()='1st Class AC']")).Click();
            _driver.FindElement(By.XPath("//span[text()='Search']")).Click();

            //Select the train filter
            IWebElement type = _driver.FindElement(By.XPath("//p[text()='Train Types']"));
            Actions trainType = new Actions(_driver);
            trainType.MoveToElement(type).Build().Perform();
            _driver.FindElement(By.XPath("//input[contains(@id,'trainTypeFilter')]")).Click();

            //Verify whether selected train filter displays
            Console.WriteLine(_driver.FindElement(By.XPath("//span[text()='Applied filters']/following::span")).Text);
            
        }

        [TearDown]
        public void FinalTearDown()
        {
            _driver.Quit();

        }
        }
    }