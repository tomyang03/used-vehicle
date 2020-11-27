/* ----------------------------------------------------------------------------------
 SENG8040 Project - Manual Testing and Selenium Automation Testing
 Used Vehicle Sale Web Application 
 Professor: Firouzeh Sharifi Lotfabad
 Group 2: Dennis Nay, Kang Yang, Ruchika Shekhawat, Youngyun Namkung 
 ----------------------------------------------------------------------------------*/

/* ----------------------------------------------------------------------------------
TODO: 
    * TrimWhiteSpaceAndLetterConversion()
    
MAYBE:
    * Prevent duplicate entries? When all fields are the same as an exisiting entry
    * Find better way to validate vehicle year?
    * Confirm with user if they want to clear vehicle list?
 ----------------------------------------------------------------------------------*/
const vehicleList = document.getElementById("vehicleList");
const enteredVehicle = document.getElementById("enteredVehicle");

const VEHICLES_LS = "vehicles"; // Local storage key

const emailRegex = RegExp(/^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/);
const phoneNumberRegex = RegExp(/^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/);
let errorCount;


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
	
	errorCount = 0;
	validateRequired(sellerName, "sellerName", "Seller Name");
	validateRequired(userAddress, "address", "Address");
	validateRequired(userCity, "city", "City");
	validateRequired(userPhoneNumber, "phoneNumber", "Phone Number");
	validateRequired(userEmail, "email", "Email");
	validateRequired(vehicleMake, "vehicleMake", "Vehicle Make");
	validateRequired(vehicleModel, "vehicleModel", "Vehicle Model");
	validateRequired(vehicleYear, "vehicleYear", "Vehicle Year");

	if (userPhoneNumber.length != 0 && !phoneNumberRegex.test(userPhoneNumber)) {
		errorCount++;
		document.getElementById("phoneNumber").classList.add("err-input");
		document.getElementById("phoneNumberError").innerText = "Invalid phone number format";
	}

	if (userEmail.length != 0 && !emailRegex.test(userEmail)) {
		errorCount++;
		document.getElementById("email").classList.add("err-input");
		document.getElementById("emailError").innerText = "Invalid email format";
	}

	/*
	// TODO: Validate address format
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
	*/
	
	console.log(`errorCount: ${errorCount}`)
	if (errorCount === 0)
	{	
		const vehicleObj = {
			sellerName,
			userAddress,
			userCity,
			userPhoneNumber,
			userEmail,
			vehicleMake,
			vehicleModel,
			vehicleYear,
		}
		let vehicles = JSON.parse(localStorage.getItem(VEHICLES_LS));
		if (vehicles === null) {
			vehicles = []
		}
		vehicles.push(vehicleObj);
		localStorage.setItem(VEHICLES_LS, JSON.stringify(vehicles));
	} else {
		return false; 
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
	let vehicles_LS = JSON.parse(localStorage.getItem(VEHICLES_LS));
	const lastVehicle = vehicles_LS[vehicles_LS.length - 1];
	loadVehicle(lastVehicle, enteredVehicle);
}

function DisplayVehicleList()
{
	const displayVehicles = localStorage.getItem(VEHICLES_LS);
	if (displayVehicles !== null) {
		const parsedVehicles = JSON.parse(displayVehicles);
		parsedVehicles.forEach(function(vehicle) {
			loadVehicle(vehicle, vehicleList);
		})
	} else {
		vehicleList.innerText += "No used-vehicles yet.";
	}
}

function ClearLocalStorage()
{
	// MAYBE: Confirm with user if they want to clear vehicle list?
	localStorage.clear();
	location.reload();
}

const validateRequired = (value, id, label) => {
	if (value.length === 0) {
		errorCount++;
		document.getElementById(id).classList.add("err-input");
		document.getElementById(`${id}Error`).innerText = `${label} is required`;
	} else if (value.length < 2) {
		errorCount++;
		document.getElementById(id).classList.add("err-input");
		document.getElementById(`${id}Error`).innerText = "Invalid format (Minimum 2 characters required)";
	} else {
		document.getElementById(id).classList.remove("err-input");
		document.getElementById(`${id}Error`).innerText = "";
	}
};

const loadVehicle = (obj, ele) => {
	const li = document.createElement("li");
	const linkBtn = document.createElement("a");
	const div = document.createElement("div");
	// const vehicleId = vehicles_LS.length + 1;
	linkBtn.innerText = "Move to JD Power";
	linkBtn.href = `http://www.jdpower.com/cars/${obj.vehicleMake}/${obj.vehicleModel}/${obj.vehicleYear}`;
	linkBtn.target = "_blank";

	div.innerHTML = `
		<label>Seller Name:</label> ${obj.sellerName} <br />
		<label>Address:</label> ${obj.userAddress} <br />
		<label>City:</label> ${obj.userCity} <br />
		<label>Phone Number:</label> ${obj.userPhoneNumber} <br />
		<label>Email:</label> ${obj.userEmail} <br />
		<label>Vehicle Make:</label> ${obj.vehicleMake} <br />
		<label>Vehicle Model:</label> ${obj.vehicleModel} <br />
		<label>vehicle Year:</label> ${obj.vehicleYear} <br />
	`;

	li.appendChild(div);
	li.appendChild(linkBtn);
	// li.id = vehicleId;
	ele.appendChild(li);
};
