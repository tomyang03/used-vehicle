/* ----------------------------------------------------------------------------------
 SENG8040 Project - Manual Testing and Selenium Automation Testing
 Used Vehicle Sale Web Application 
 Professor: Firouzeh Sharifi Lotfabad
 Group 2: Dennis Nay, Kang Yang, Ruchika Shekhawat, Youngyun Namkung 
 ----------------------------------------------------------------------------------*/

/* ----------------------------------------------------------------------------------
TODO: 
    * Validate phone number, address, email formats
    * TrimWhiteSpaceAndLetterConversion()
    * Find way to append correct jdpower hyperlink to each vehicle entry
    
MAYBE:
    * Prevent duplicate entries? When all fields are the same as an exisiting entry
    * Find better way to validate vehicle year?
    * Confirm with user if they want to clear vehicle list?
    * Set minimum character requirement for fields?
 ----------------------------------------------------------------------------------*/
const emailRegex = RegExp(/^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/);
const phoneNumberRegex = RegExp(/^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/);

let errorMessage = "";
let enteredVehicle = "";

// Form Validations --------------------------------------------------------------//
function ValidateForm()
{
	// Get All Form Elements -----------------------------------------------------//
	const sellerName = document.getElementById("sellerName").value;
	const userAddress = document.getElementById("address").value;
	const userCity = document.getElementById("city").value;
	const userPhoneNumber = document.getElementById("phoneNumber").value;	
	const userEmail = document.getElementById("email").value;	
	const vehicleMake = document.getElementById("vehicleMake").value;
	const vehicleModel = document.getElementById("vehicleModel").value;
	const vehicleYear = document.getElementById("vehicleYear").value;
	
	errorMessage = "";
	validateRequired(sellerName, "sellerName", "Seller Name");
	validateRequired(userAddress, "address", "Address");
	validateRequired(userCity, "city", "City");
	validateRequired(userPhoneNumber, "phoneNumber", "Phone Number");
	validateRequired(userEmail, "email", "Email");
	validateRequired(vehicleMake, "vehicleMake", "Vehicle Make");
	validateRequired(vehicleModel, "vehicleModel", "Vehicle Model");
	validateRequired(vehicleYear, "vehicleYear", "Vehicle Year");
	
	if (sellerName.length == 1)
	{
		errorMessage += "Enter full name.<br />";
		document.getElementById("sellerName").focus();
	}	
	// TODO: Validate address format
	if (userCity.length == 1)
	{
		errorMessage += "Invalid city.<br />";
		document.getElementById("city").focus();
	}
	// TODO: Validate phone format
	// TODO: Validate email format
	if (vehicleMake.length == 1)
	{
		errorMessage += "Invalid vehicle make.<br />";
		document.getElementById("vehicleMake").focus();
	}	
	if (vehicleModel.length == 1)
	{
		errorMessage += "Invalid vehicle model.<br />";
		document.getElementById("vehicleModel").focus();
	}
	// MAYBE: Find better way to dynamically validate vehicle year?
	if (vehicleYear.length != 0 && vehicleYear < 1990)
	{
		errorMessage += "Vehicle model year too old.<br />";
		document.getElementById("vehicleYear").focus();
	}
	if (vehicleYear > 2020)
	{
		errorMessage += "Vehicle model too new.<br />";
		document.getElementById("vehicleYear").focus();
	}	
	
	if (errorMessage.length != 0)
	{
		document.getElementById("errorOutputSpan").innerHTML = errorMessage;
		return false; // prevents form from submitting if there's errors
    }
    
	if (errorMessage.length == 0)
	{		
		// TODO: append JD Power hyperlink to string
		enteredVehicle += "<b>Seller: " + sellerName + "</b><br />" +
			"Address: " + userAddress + "<br />" +
			"City: " + userCity + "<br />" +
			"Phone: " + userPhoneNumber + "<br />" +
			"Email: " + userEmail + "<br />" +
			"Vehicle: " + vehicleYear + " " + vehicleMake + " " + vehicleModel + 
			"<br /><br />";
    
		// save submitted vehicle info to local storage item "successMessage",
		// which is used by DisplayEnteredVehicle()
		localStorage.setItem("successMessage", enteredVehicle);	
        
		// create var vehicleList to hold local 
		// storage item "vehicleList" (if any)
		let vehicleList = localStorage.getItem("vehicleList");
        
        // if var vehicleList IS empty
		if (vehicleList == null)
		{   
			// add var enteredVehicle to local storage item "vehicleList"
			localStorage.setItem("vehicleList", enteredVehicle);
		}
		else
		{
			// if var vehicleList is NOT empty, first append var enteredVehicle 
			// to var vehicleList, then replace local storage item "vehicleList"
			// with the updated var vehicleList
			vehicleList += enteredVehicle;
			localStorage.setItem("vehicleList", vehicleList);			
		}
	}
}

// Trim White Space and Letter Case Conversions ---------------------------------//
function TrimWhiteSpaceAndLetterConversion()
{
    // TODO: Trim any leading or trailing white spaces for all fields
    // Capitalize the first letter of each input
}

function DisplayEnteredVehicle()
{
	document.getElementById("successOutputSpan").innerHTML = 
		localStorage.getItem("successMessage");		
}

function DisplayVehicleList()
{
	document.getElementById("vehicleList").innerHTML = 
		localStorage.getItem("vehicleList");
}

function ClearLocalStorage()
{
	// MAYBE: Confirm with user if they want to clear vehicle list?
	localStorage.clear();
	location.reload();
}

const validateRequired = (value, id, label) => {
	if (value.length == 0) {
		errorMessage += `${label} required.<br />`;
		document.getElementById(id).classList.add("err-input");
		return false;
	} else {
		document.getElementById(id).classList.remove("err-input");
	}
};