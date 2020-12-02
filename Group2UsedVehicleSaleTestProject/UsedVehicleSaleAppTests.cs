using System;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
/* -------------------------------------------------------------------------------------
 * SENG8040 Project: Manual Testing and Selenium Automation Testing
 * Used Vehicle Sale App System Testing
 * Professor: Firouzeh Sharifi Lotfabad
 * Group 2: Dennis Nay, Kang Yang, Ruchika Shekhawat, Youngyun Namkung
---------------------------------------------------------------------------------------*/
namespace Group2UsedVehicleSaleTestProject
{
    public class UsedVehicleSaleAppTests
    {
        private IWebDriver driver;              // IWebDriver interface
        private string currentDirectory;        // path to current directory
        private string homeURL;                 // Group2Home.html URL
        private string addVehicleURL;           // AddUsedVehicle.html URL

        [SetUp]
        public void Setup()
        {
            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            currentDirectory = Environment.CurrentDirectory;
            homeURL = $"file:///{currentDirectory}../../../../../Used-vehicle-sale/Group2Home.html";
            addVehicleURL = $"file:///{currentDirectory}../../../../../Used-vehicle-sale/html/AddUsedVehicle.html";
        }

        [Test]
        public void LoadHomePage_Wait10Seconds_CorrectTitleAppears()
        {
            /* --------------------------------------------------------------------------
             Smoke Test: Verify the website opens in Firefox
                 (1) Navigate to home page.
                 (2) Check if the correct title appears for up to 10 seconds.
                     If it does, the website successfully opened in Firefox.
                     If it does not, this test fails.
            ----------------------------------------------------------------------------*/

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Navigate().GoToUrl(homeURL);
            wait.Until(p => p.Title.Contains("Group 2 - Used Vehicle Sale App"));
        }

        [Test]
        public void LoadAddVehiclePage_LeaveFieldsBlankAndSubmit_DisplayRequiredErrorsForAllFields()
        {
            /* --------------------------------------------------------------------------
             Test #1: Verify the website requires all form fields be mandatory
                 (1) Navigate to AddUsedVehicle page.
                 (2) Leave all fields blank and click "Submit".
                 (3) Check if all fields are validated and their correct error
                     messages are displayed.
            ----------------------------------------------------------------------------*/

            driver.Navigate().GoToUrl(addVehicleURL);
            driver.FindElement(By.Id("sellerName")).SendKeys("");
            driver.FindElement(By.Id("address")).SendKeys("");
            driver.FindElement(By.Id("city")).SendKeys("");
            driver.FindElement(By.Id("phoneNumber")).SendKeys("");
            driver.FindElement(By.Id("email")).SendKeys("");
            driver.FindElement(By.Id("vehicleMake")).SendKeys("");
            driver.FindElement(By.Id("vehicleModel")).SendKeys("");
            driver.FindElement(By.Id("vehicleYear")).SendKeys("");
            driver.FindElement(By.Id("submitBtn")).Click();

            string sellerNameError = driver.FindElement(By.Id("sellerNameError")).Text;
            string addressError = driver.FindElement(By.Id("addressError")).Text;
            string cityError = driver.FindElement(By.Id("cityError")).Text;
            string phoneNumberError = driver.FindElement(By.Id("phoneNumberError")).Text;
            string emailError = driver.FindElement(By.Id("emailError")).Text;
            string vehicleMakeError = driver.FindElement(By.Id("vehicleMakeError")).Text;
            string vehicleModelError = driver.FindElement(By.Id("vehicleModelError")).Text;
            string vehicleYearError = driver.FindElement(By.Id("vehicleYearError")).Text;

            Assert.AreEqual("Seller Name is required", sellerNameError);
            Assert.AreEqual("Address is required", addressError);
            Assert.AreEqual("City is required", cityError);
            Assert.AreEqual("Phone Number is required", phoneNumberError);
            Assert.AreEqual("Email is required", emailError);
            Assert.AreEqual("Vehicle Make is required", vehicleMakeError);
            Assert.AreEqual("Vehicle Model is required", vehicleModelError);
            Assert.AreEqual("Vehicle Year is required", vehicleYearError);
        }

        [Test]
        public void LoadAddVehiclePage_CompleteForm_GenerateCorrectJDPowerLinkForVehicle()
        {
            /* --------------------------------------------------------------------------
             Test #2: Verify J.D. Power link redirects to the correct vehicle page
                 (1) Navigate to AddUsedVehicle page.
                 (2) Enter all required form fields and click "Submit".
                 (3) Click the genereated J.D. Power link and check if it redirects
                     to the correct J.D. Power page for that specific vehicle.
            ----------------------------------------------------------------------------*/

            driver.Navigate().GoToUrl(addVehicleURL);
            driver.FindElement(By.Id("sellerName")).SendKeys("Tony Stark");
            driver.FindElement(By.Id("address")).SendKeys("10880 Malibu Point");
            driver.FindElement(By.Id("city")).SendKeys("Point Dume");
            driver.FindElement(By.Id("phoneNumber")).SendKeys("(212)-970-4133");
            driver.FindElement(By.Id("email")).SendKeys("iron-man@conestogac.on.ca");
            driver.FindElement(By.Id("vehicleMake")).SendKeys("Tesla-Motors");
            driver.FindElement(By.Id("vehicleModel")).SendKeys("Model-S");
            driver.FindElement(By.Id("vehicleYear")).SendKeys("2020");

            string vehicleMake = Regex.Replace(driver.FindElement
                (By.Id("vehicleMake")).GetAttribute("value"), @"[-]+", " ");
            string vehicleModel = Regex.Replace(driver.FindElement
                (By.Id("vehicleModel")).GetAttribute("value"), @"[-]+", " ");
            string vehicleYear = Regex.Replace(driver.FindElement
                (By.Id("vehicleYear")).GetAttribute("value"), @"[-]+", " ");

            driver.FindElement(By.Id("submitBtn")).Click();
            driver.FindElement(By.ClassName("btn-jd-power")).Click();
            driver.SwitchTo().Window(driver.WindowHandles.Last());

            Assert.AreEqual($"{vehicleYear} {vehicleMake} {vehicleModel} " +
                $"Ratings, Pricing, Reviews and Awards | J.D. Power", driver.Title);
        }

        [Test]
        public void LoadPage_Action_Result3()
        {
            /* --------------------------------------------------------------------------
             Test #: 
                 (1) 
                 (2) 
                 (3) 
            ----------------------------------------------------------------------------*/

        }

        [Test]
        public void LoadPage_Action_Result4()
        {
            /* --------------------------------------------------------------------------
             Test #: 
                 (1) 
                 (2) 
                 (3) 
            ----------------------------------------------------------------------------*/

        }

        [Test]
        public void LoadPage_Action_Result5()
        {
            /* --------------------------------------------------------------------------
             Test #: 
                 (1) 
                 (2) 
                 (3) 
            ----------------------------------------------------------------------------*/

        }

        [Test]
        public void LoadPage_Action_Result6()
        {
            /* --------------------------------------------------------------------------
             Test #: 
                 (1) 
                 (2) 
                 (3) 
            ----------------------------------------------------------------------------*/

        }

        [Test]
        public void LoadPage_Action_Result7()
        {
            /* --------------------------------------------------------------------------
             Test #: 
                 (1) 
                 (2) 
                 (3) 
            ----------------------------------------------------------------------------*/

        }
    }
}