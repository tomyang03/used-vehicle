using System;
using System.IO;
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
    [TestFixture]
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
            // Need to add this line or else Environment.CurrentDirectory will point to some funky workspace location
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            currentDirectory = Environment.CurrentDirectory;
            // removed an extra "../", before the assembly folder was netcoreapi3.1 for the Group2UsedVehicleSaleTestProject Project,
            // now the assembly folder is pointing to bin/deubg for Used_Vehicle_Console_GUI_Library Project
            homeURL = $"file:///{currentDirectory}../../../../Used-vehicle-sale/Group2Home.html";
            addVehicleURL = $"file:///{currentDirectory}../../../../Used-vehicle-sale/html/AddUsedVehicle.html";
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
        public void InvalidPhoneNumber()
        {
            /* --------------------------------------------------------------------------
           Test #1: Verify J.D. Power link redirects to the correct vehicle page
               (1) Navigate to AddUsedVehicle page.
               (2) Enter all required form fields with a phone number that is longer 
                   than 10 digits  and click "Submit".
               (3) Click on Submit button
               (4) The form shows The Invalid Phone number as an error tag 
                   for the phoneNumber field
          ----------------------------------------------------------------------------*/

            driver.Navigate().GoToUrl(addVehicleURL);
            driver.FindElement(By.Id("sellerName")).SendKeys("Tony Stark");
            driver.FindElement(By.Id("address")).SendKeys("10880 Malibu Point");
            driver.FindElement(By.Id("city")).SendKeys("Point Dume");
            driver.FindElement(By.Id("phoneNumber")).SendKeys("(212)-970-4133333");
            driver.FindElement(By.Id("email")).SendKeys("iron-man@conestogac.on.ca");
            driver.FindElement(By.Id("vehicleMake")).SendKeys("Tesla-Motors");
            driver.FindElement(By.Id("vehicleModel")).SendKeys("Model-S");
            driver.FindElement(By.Id("vehicleYear")).SendKeys("2020");
            driver.FindElement(By.Id("submitBtn")).Click();

            string phoneNumberError = driver.FindElement(By.Id("phoneNumberError")).Text;
            Assert.AreEqual("Invalid phone number format", phoneNumberError);
        }

        [Test]
        public void InvalidAddress()
        {
            /* --------------------------------------------------------------------------
            Test #2: Verify J.D. Power link redirects to the correct vehicle page
                (1) Navigate to AddUsedVehicle page.
                (2) Enter all required form fields with a address that is only 1 character 
                    than 10 digits  and click "Submit".
                (3) Click on Submit button
                (4) The form shows the error message Invalid format (Minimum 2 characters required)
                    for the address field
           ----------------------------------------------------------------------------*/

            driver.Navigate().GoToUrl(addVehicleURL);
            driver.FindElement(By.Id("sellerName")).SendKeys("Tony Stark");
            driver.FindElement(By.Id("address")).SendKeys("M");
            driver.FindElement(By.Id("city")).SendKeys("Point Dume");
            driver.FindElement(By.Id("phoneNumber")).SendKeys("(212)-970-4133");
            driver.FindElement(By.Id("email")).SendKeys("iron-man@conestogac.on.ca");
            driver.FindElement(By.Id("vehicleMake")).SendKeys("Tesla-Motors");
            driver.FindElement(By.Id("vehicleModel")).SendKeys("Model-S");
            driver.FindElement(By.Id("vehicleYear")).SendKeys("2020");
            driver.FindElement(By.Id("submitBtn")).Click();

            string addressError = driver.FindElement(By.Id("addressError")).Text;
            Assert.AreEqual("Invalid format (Minimum 2 characters required)", addressError);
        }


        [Test]
        public void InvalidEmail()
        {
            /* --------------------------------------------------------------------------
            Test #3: Verify J.D. Power link redirects to the correct vehicle page
                (1) Navigate to AddUsedVehicle page.
                (2) Enter all required form fields with a address that is only 1 character 
                    than 10 digits  and click "Submit".
                (3) Click on Submit button
                (4) The form shows the error message Invalid format (Minimum 2 characters required)
                    for the address field
           ----------------------------------------------------------------------------*/

            driver.Navigate().GoToUrl(addVehicleURL);
            driver.FindElement(By.Id("sellerName")).SendKeys("Tony Stark");
            driver.FindElement(By.Id("address")).SendKeys("10880 Malibu Point");
            driver.FindElement(By.Id("city")).SendKeys("Point Dume");
            driver.FindElement(By.Id("phoneNumber")).SendKeys("(212)-970-4133");
            driver.FindElement(By.Id("email")).SendKeys("iron-man at conestogac.on.ca");
            driver.FindElement(By.Id("vehicleMake")).SendKeys("Tesla-Motors");
            driver.FindElement(By.Id("vehicleModel")).SendKeys("Model-S");
            driver.FindElement(By.Id("vehicleYear")).SendKeys("2020");
            driver.FindElement(By.Id("submitBtn")).Click();

            string addressError = driver.FindElement(By.Id("emailError")).Text;
            Assert.AreEqual("Invalid email format", addressError);
        }


        [Test]
        public void LoadAddVehiclePage_LeaveFieldsBlankAndSubmit_DisplayRequiredErrorsForAllFields()
        {
            /* --------------------------------------------------------------------------
             Test #4: Verify the website requires all form fields be mandatory
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
        public void LoadAddVehiclePage_CompleteForm_VerifyFormDetailsUponSubmission()
        {
            /* --------------------------------------------------------------------------
             Test #5: Verify that the details after submitting the page (actualFormOutputText) is equal to 
                      exptectedFormOutput: The expected form result after clicking the submit button
                 (1) Navigate to AddUsedVehicle page.
                 (2) Enter all required form fields and click "Submit".
                 (3) NUnit verifies that our exptectedFormOutput matches the
                     actual Output (actualFormOutputText) that was generated after the user
                     has entered all the fields correctly.                     
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

            driver.FindElement(By.Id("submitBtn")).Click();
            IWebElement FormOutput = driver.FindElement(By.XPath("//section[@id='enteredVehicle']/li/div"));
            string actualFormOutputText = FormOutput.Text.Replace("\r\n", " ");
            string expectedFormOutput = "Seller Name: Tony stark Address: 10880 malibu point City: Point dume Phone Number: (212)-970-4133 Email: iron-man@conestogac.on.ca Vehicle Make: Tesla-motors Vehicle Model: Model-s Vehicle Year: 2020";
            Assert.AreEqual(expectedFormOutput, actualFormOutputText);
        }


        /* This test fails in the Nunit Console GUI, 
         * I suspect that driver.Title is unable to get the title from the page,
         * could this test be made optional, we need to look into this.
         */
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
    }
}