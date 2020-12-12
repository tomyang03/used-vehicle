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
namespace Group2UsedVehicleSale_GUI_TestProject
{
    [TestFixture]
    public class UsedVehicleSaleApp_GUI_Tests
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
            // Need this line or else Environment.CurrentDirectory points to some funky workspace location
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            currentDirectory = Environment.CurrentDirectory;
            
            // removed an extra "../", before the assembly folder was netcoreapi3.1 for 
            // the Group2UsedVehicleSaleTestProject Project, now the assembly folder is 
            // pointing to bin/deubg for Group2UsedVehicleSale_GUI_TestProject Project

            // homeURL: The dynamic file location for the homepage            
            homeURL = $"file:///{currentDirectory}../../../../Used-vehicle-sale/Group2Home.html";
            
            // addVehicleURL: The dynamic file location for the website with the form to add vehicles
            addVehicleURL = $"file:///{currentDirectory}../../../../Used-vehicle-sale/html/AddUsedVehicle.html";
        }

        [Test]
        public void LoadHomePage_Wait10Seconds_CorrectTitleAppears()
        {
            /* --------------------------------------------------------------------------
             UVS001: Application launches on Firefox and correct title appears
                 (1) Navigate to home page.
                 (2) Check if the correct title appears for up to 10 seconds.
                     If it does, the website successfully opened in Firefox.
                     If it does not, this test fails.
            ----------------------------------------------------------------------------*/

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            
            // Let driver navigate to the homepage
            driver.Navigate().GoToUrl(homeURL);
            
            // Validation: Check if webpage title says "Group 2 - Used Vehicle Sale App",
            // which proves the test passed within the timespan of 10 seconds
            wait.Until(p => p.Title.Contains("Group 2 - Used Vehicle Sale App"));
        }

        [Test]
        public void LoadAddVehiclePage_LeaveFieldsBlankAndSubmit_DisplayRequiredErrorsForAllFields()
        {
            /* --------------------------------------------------------------------------
             UVS002: Application requires all form fields be mandatory
                 (1) Navigate to AddUsedVehicle page.
                 (2) Leave all fields blank and click "Submit".
                 (3) Check if all fields are validated and their correct error
                     messages are displayed.
            ----------------------------------------------------------------------------*/

            // Let driver navigate to AddUsedVehicle page
            driver.Navigate().GoToUrl(addVehicleURL);

            // Enter empty field values (i.e. leave all form fields blank) then submit
            driver.FindElement(By.Id("sellerName")).SendKeys("");
            driver.FindElement(By.Id("address")).SendKeys("");
            driver.FindElement(By.Id("city")).SendKeys("");
            driver.FindElement(By.Id("phoneNumber")).SendKeys("");
            driver.FindElement(By.Id("email")).SendKeys("");
            driver.FindElement(By.Id("vehicleMake")).SendKeys("");
            driver.FindElement(By.Id("vehicleModel")).SendKeys("");
            driver.FindElement(By.Id("vehicleYear")).SendKeys("");
            driver.FindElement(By.Id("submitBtn")).Click();

            // Get the text from error div tags generated after the form was submitted
            string sellerNameError = driver.FindElement(By.Id("sellerNameError")).Text;
            string addressError = driver.FindElement(By.Id("addressError")).Text;
            string cityError = driver.FindElement(By.Id("cityError")).Text;
            string phoneNumberError = driver.FindElement(By.Id("phoneNumberError")).Text;
            string emailError = driver.FindElement(By.Id("emailError")).Text;
            string vehicleMakeError = driver.FindElement(By.Id("vehicleMakeError")).Text;
            string vehicleModelError = driver.FindElement(By.Id("vehicleModelError")).Text;
            string vehicleYearError = driver.FindElement(By.Id("vehicleYearError")).Text;

            // Validation: Check if error div tags hold the corresponding error messages,
            // such as "Seller Name is required", etc. which proves the form submission 
            // was not successful because all the fields were left blank 
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
        public void LoadAddVehiclePage_EnterInvalidAddress_DisplayInvalidAddressError()
        {
            /* --------------------------------------------------------------------------
             UVS003: Application validates correct address format
                (1) Navigate to AddUsedVehicle page.
                (2) Enter all required form fields with an address that is only 
                    one character long and click "Submit".
                (3) Check if the form displays an invalid address error message.
           ----------------------------------------------------------------------------*/

            // Let driver navigate to AddUsedVehicle page
            driver.Navigate().GoToUrl(addVehicleURL);
            
            // Fill out the the form with an invalid address then submit
            driver.FindElement(By.Id("sellerName")).SendKeys("Tony Stark");
            driver.FindElement(By.Id("address")).SendKeys("M");
            driver.FindElement(By.Id("city")).SendKeys("Point Dume");
            driver.FindElement(By.Id("phoneNumber")).SendKeys("(212)-970-4133");
            driver.FindElement(By.Id("email")).SendKeys("iron-man@conestogac.on.ca");
            driver.FindElement(By.Id("vehicleMake")).SendKeys("Tesla-Motors");
            driver.FindElement(By.Id("vehicleModel")).SendKeys("Model-S");
            driver.FindElement(By.Id("vehicleYear")).SendKeys("2020");
            driver.FindElement(By.Id("submitBtn")).Click();

            // Get text from addressError div tag generated after submitting the form
            string addressError = driver.FindElement(By.Id("addressError")).Text;

            // Validation: check if addressError holds the error message, 
            // "Invalid phone number format", which proves an invalid address was entered
            Assert.AreEqual("Invalid format (Minimum 2 characters required)", addressError);
        }

        [Test]
        public void LoadAddVehiclePage_EnterInvalidPhoneNumber_DisplayInvalidPhoneNumberError()
        {
            /* --------------------------------------------------------------------------
             UVS004: Application validates correct phone number format
               (1) Navigate to AddUsedVehicle page.
               (2) Enter all required form fields with a phone number shorter 
                   than 10 digits and click "Submit".
               (3) Check if the form displays an invalid phone number error message.
          ----------------------------------------------------------------------------*/

            // Let driver navigate to AddUsedVehicle page
            driver.Navigate().GoToUrl(addVehicleURL);

            // Fill out the form with an invalid phone number then submit
            driver.FindElement(By.Id("sellerName")).SendKeys("Tony Stark");
            driver.FindElement(By.Id("address")).SendKeys("10880 Malibu Point");
            driver.FindElement(By.Id("city")).SendKeys("Point Dume");
            driver.FindElement(By.Id("phoneNumber")).SendKeys("519-123-456");
            driver.FindElement(By.Id("email")).SendKeys("iron-man@conestogac.on.ca");
            driver.FindElement(By.Id("vehicleMake")).SendKeys("Tesla-Motors");
            driver.FindElement(By.Id("vehicleModel")).SendKeys("Model-S");
            driver.FindElement(By.Id("vehicleYear")).SendKeys("2020");
            driver.FindElement(By.Id("submitBtn")).Click();

            // Get text from phoneNumberError div tag generated after submitting the form 
            string phoneNumberError = driver.FindElement(By.Id("phoneNumberError")).Text;
           
            // Validation: check if phoneNumberError holds the error message, 
            // "Invalid phone number format", which proves an invalid phone number was entered
            Assert.AreEqual("Invalid phone number format", phoneNumberError);
        }

        [Test]
        public void LoadAddVehiclePage_EnterInvalidEmail_DisplayInvalidEmailError()
        {
            /* --------------------------------------------------------------------------
             UVS005: Application validates correct email format
                (1) Navigate to AddUsedVehicle page.
                (2) Enter all required form fields with an email address that does not 
                    have the "@" symbol and click "Submit".
                (3) Check if the form displays an invalid email error message.
           ----------------------------------------------------------------------------*/

            // Let driver navigate to AddUsedVehicle page
            driver.Navigate().GoToUrl(addVehicleURL);

            // Fill out the form with an invalid email then submit
            driver.FindElement(By.Id("sellerName")).SendKeys("Tony Stark");
            driver.FindElement(By.Id("address")).SendKeys("10880 Malibu Point");
            driver.FindElement(By.Id("city")).SendKeys("Point Dume");
            driver.FindElement(By.Id("phoneNumber")).SendKeys("(212)-970-4133");
            driver.FindElement(By.Id("email")).SendKeys("iron-manatconestogac.on.ca");
            driver.FindElement(By.Id("vehicleMake")).SendKeys("Tesla-Motors");
            driver.FindElement(By.Id("vehicleModel")).SendKeys("Model-S");
            driver.FindElement(By.Id("vehicleYear")).SendKeys("2020");
            driver.FindElement(By.Id("submitBtn")).Click();

            // Get text from emailError div tag generated after submitting the form
            string emailError = driver.FindElement(By.Id("emailError")).Text;

            // Validation: check if emailError holds the error message, 
            // "Invalid email format", which proves an invalid email was entered
            Assert.AreEqual("Invalid email format", emailError);
        }

        [Test]
        public void LoadAddVehiclePage_TypeFieldsSpaceAndSubmit_DisplayRequiredErrorsForAllFields()
        {
            /* --------------------------------------------------------------------------
             UVS006: Application prevents white spaces from being accepted as form inputs
                 (1) Navigate to AddUsedVehicle page.
                 (2) Type all fields spaces and click "Submit".
                 (3) Check if all fields are validated and their correct error
                     messages are displayed.
            ----------------------------------------------------------------------------*/

            // Let driver navigate to AddUsedVehicle page
            driver.Navigate().GoToUrl(addVehicleURL);

            // Enter empty field values (i.e. leave all form fields space) then submit
            driver.FindElement(By.Id("sellerName")).SendKeys("    ");
            driver.FindElement(By.Id("address")).SendKeys("  ");
            driver.FindElement(By.Id("city")).SendKeys("   ");
            driver.FindElement(By.Id("phoneNumber")).SendKeys("   ");
            driver.FindElement(By.Id("email")).SendKeys("   ");
            driver.FindElement(By.Id("vehicleMake")).SendKeys("     ");
            driver.FindElement(By.Id("vehicleModel")).SendKeys("    ");
            driver.FindElement(By.Id("vehicleYear")).SendKeys("    ");
            driver.FindElement(By.Id("submitBtn")).Click();

            // Get the text from error div tags generated after the form was submitted
            string sellerNameError = driver.FindElement(By.Id("sellerNameError")).Text;
            string addressError = driver.FindElement(By.Id("addressError")).Text;
            string cityError = driver.FindElement(By.Id("cityError")).Text;
            string phoneNumberError = driver.FindElement(By.Id("phoneNumberError")).Text;
            string emailError = driver.FindElement(By.Id("emailError")).Text;
            string vehicleMakeError = driver.FindElement(By.Id("vehicleMakeError")).Text;
            string vehicleModelError = driver.FindElement(By.Id("vehicleModelError")).Text;
            string vehicleYearError = driver.FindElement(By.Id("vehicleYearError")).Text;

            // Validation: Check if error div tags hold the corresponding error messages,
            // such as "Seller Name is required", etc. which proves the form submission 
            // was not successful because all the fields were left blank 
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
             UVS007: Application displays correct contact and 
                     vehicle information after the form is submitted
                 
                 (1) Navigate to AddUsedVehicle page.
                 (2) Enter all required form fields and click "Submit".
                 (3) Check if the correct contact and vehicle information is
                     displayed on the next page.
            ----------------------------------------------------------------------------*/

            // Let driver navigate to AddUsedVehicle page
            driver.Navigate().GoToUrl(addVehicleURL);

            // Fill out the form with valid field values then submit
            driver.FindElement(By.Id("sellerName")).SendKeys("Tony Stark");
            driver.FindElement(By.Id("address")).SendKeys("10880 Malibu Point");
            driver.FindElement(By.Id("city")).SendKeys("Point Dume");
            driver.FindElement(By.Id("phoneNumber")).SendKeys("(212)-970-4133");
            driver.FindElement(By.Id("email")).SendKeys("iron-man@conestogac.on.ca");
            driver.FindElement(By.Id("vehicleMake")).SendKeys("Tesla-Motors");
            driver.FindElement(By.Id("vehicleModel")).SendKeys("Model-S");
            driver.FindElement(By.Id("vehicleYear")).SendKeys("2020");
            driver.FindElement(By.Id("submitBtn")).Click();

            // After submitting the form, get text from div tag that displays the 
            // previously entered contact and vehicle information
            IWebElement FormOutput = driver.FindElement
                (By.XPath("//section[@id='enteredVehicle']/li/div"));

            // Validation: check if expectedFormOutput (contains submission details)
            // matches the actualFormOutputText generated afer submitting the form,  
            // which proves the form was submitted successfully and the correct
            // information is displayed on the next page
            string expectedFormOutput = 
                "Seller Name: Tony stark Address: 10880 malibu point City: Point " +
                    "dume Phone Number: (212)-970-4133 Email: iron-man@conestogac.on.ca " +
                        "Vehicle Make: Tesla-motors Vehicle Model: Model-s Vehicle Year: 2020";
            string actualFormOutputText = FormOutput.Text.Replace("\r\n", " ");            
            Assert.AreEqual(expectedFormOutput, actualFormOutputText);
        }

        [Test]
        public void LoadAddVehiclePage_CompleteForm_DisplayEntryInCurrentVehicleListing()
        {
            /* --------------------------------------------------------------------------
             UVS008: Application correctly displays a list of previously 
                     saved vehicles and their details

                (1) Navigate to AddUsedVehicle page.
                (2) Enter all required form fields and click "Submit".
                (3) Navigate to CurrentVehicleListing page.
                (4) Check if the previously submitted vehicle info is saved to the list.
           ----------------------------------------------------------------------------*/

            // Let driver navigate to AddUsedVehicle page
            driver.Navigate().GoToUrl(addVehicleURL);

            // Fill out the form with valid field values
            driver.FindElement(By.Id("sellerName")).SendKeys("Tony Stark");
            driver.FindElement(By.Id("address")).SendKeys("10880 Malibu Point");
            driver.FindElement(By.Id("city")).SendKeys("Point Dume");
            driver.FindElement(By.Id("phoneNumber")).SendKeys("(212)-970-4133");
            driver.FindElement(By.Id("email")).SendKeys("iron-man@conestogac.on.ca");
            driver.FindElement(By.Id("vehicleMake")).SendKeys("Tesla-Motors");
            driver.FindElement(By.Id("vehicleModel")).SendKeys("Model-S");
            driver.FindElement(By.Id("vehicleYear")).SendKeys("2020");
            
            // Click submit then navigate to the current vehicle listing
            driver.FindElement(By.Id("submitBtn")).Click();
            driver.FindElement(By.Id("homePageBtn")).Click();
            driver.FindElement(By.Id("viewBtn")).Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(p => p.Title.Contains("Current Vehicle Listing"));

            // Validation: check if the current vehicle listing is empty. If it's not,
            // then the application successfully saved the vehicle information.
            string vehicleList = driver.FindElement(By.Id("vehicleList")).Text;
            Assert.IsNotEmpty(vehicleList);
        }

        [Test]
        public void LoadAddVehiclePage_CompleteForm_GenerateCorrectJDPowerLinkForVehicle()
        {
            /* --------------------------------------------------------------------------
             UVS009: Application generates correct J.D. Power link for each specific 
                     vehicle added and redirects to the appropriate J.D. Power page
                 
                 (1) Navigate to AddUsedVehicle page.
                 (2) Enter all required form fields and click "Submit".
                 (3) Click the genereated J.D. Power link.
                 (4) Check for up to 10 seconds if it redirects to the
                     correct J.D. Power page for that specific vehicle.
            ----------------------------------------------------------------------------*/

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Let driver navigate to AddUsedVehicle page
            driver.Navigate().GoToUrl(addVehicleURL);

            // Fill out the form with valid field values
            driver.FindElement(By.Id("sellerName")).SendKeys("Tony Stark");
            driver.FindElement(By.Id("address")).SendKeys("10880 Malibu Point");
            driver.FindElement(By.Id("city")).SendKeys("Point Dume");
            driver.FindElement(By.Id("phoneNumber")).SendKeys("(212)-970-4133");
            driver.FindElement(By.Id("email")).SendKeys("iron-man@conestogac.on.ca");
            driver.FindElement(By.Id("vehicleMake")).SendKeys("Tesla-Motors");
            driver.FindElement(By.Id("vehicleModel")).SendKeys("Model-S");
            driver.FindElement(By.Id("vehicleYear")).SendKeys("2020");

            // Get the input text for vehicle make/model/year (replace any "-" with " ")
            string vehicleMake = Regex.Replace(driver.FindElement
                (By.Id("vehicleMake")).GetAttribute("value"), @"[-]+", " ");
            string vehicleModel = Regex.Replace(driver.FindElement
                (By.Id("vehicleModel")).GetAttribute("value"), @"[-]+", " ");
            string vehicleYear = Regex.Replace(driver.FindElement
                (By.Id("vehicleYear")).GetAttribute("value"), @"[-]+", " ");

            // Submit the form and click the J.D. Power link generated
            driver.FindElement(By.Id("submitBtn")).Click();
            driver.FindElement(By.ClassName("btn-jd-power")).Click();
            driver.SwitchTo().Window(driver.WindowHandles.Last());

            // Validation: check for up to 10 sec if title of J.D. Power page matches
            // the vehicle make/model/year previously entered, which proves the link
            // redirects to the correct J.D. Power page for the specific vehicle
            wait.Until(p => p.Title.Contains($"{vehicleYear} {vehicleMake} {vehicleModel} " +
                $"Ratings, Pricing, Reviews and Awards | J.D. Power"));

            // If the test times out after 10 seconds or the title doesn't match, 
            // then the test fails.
        }
    }
}