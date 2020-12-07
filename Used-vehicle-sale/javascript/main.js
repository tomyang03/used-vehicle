/* ----------------------------------------------------------------------------------
 SENG8040 Project - Manual Testing and Selenium Automation Testing
 Used Vehicle Sale Web Application 
 Professor: Firouzeh Sharifi Lotfabad
 Group 2: Dennis Nay, Kang Yang, Ruchika Shekhawat, Youngyun Namkung 
 ----------------------------------------------------------------------------------*/

/* ----------------------------------------------------------------------------------
MAYBE:
    * Prevent duplicate entries? When all fields are the same as an exisiting entry
 ----------------------------------------------------------------------------------*/
const vehicleList = document.getElementById("vehicleList");
const enteredVehicle = document.getElementById("enteredVehicle");

const VEHICLES_LS = "vehicles"; // Local storage key

const addressRegex = RegExp(/^[#.0-9a-zA-Z\s,-]+$/);
const emailRegex = RegExp(/^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/);
const phoneNumberRegex = RegExp(/^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/);
const yearRegex = RegExp(/^[0-9]+$/);
let errorCount;

// Form Validations --------------------------------------------------------------//
function ValidateForm()
{
	// Get All Form Elements -----------------------------------------------------//
	let sellerName = document.getElementById("sellerName").value;
	let userAddress = document.getElementById("address").value;
	let userCity = document.getElementById("city").value;
	let userPhoneNumber = document.getElementById("phoneNumber").value;	
	let userEmail = document.getElementById("email").value;	
	let vehicleMake = document.getElementById("vehicleMake").value;
	let vehicleModel = document.getElementById("vehicleModel").value;
	let vehicleYear = document.getElementById("vehicleYear").value;
	
	errorCount = 0;
	// Validate required field and format data (trim and capitalize first letter)
	sellerName = validateRequired(sellerName, "sellerName", "Seller Name");
	userAddress = validateRequired(userAddress, "address", "Address");
	userCity = validateRequired(userCity, "city", "City");
	userPhoneNumber = validateRequired(userPhoneNumber, "phoneNumber", "Phone Number");
	userEmail = validateRequired(userEmail, "email", "Email");
	vehicleMake = validateRequired(vehicleMake, "vehicleMake", "Vehicle Make");
	vehicleModel = validateRequired(vehicleModel, "vehicleModel", "Vehicle Model");
	vehicleYear = validateRequired(vehicleYear, "vehicleYear", "Vehicle Year");
	
	// Additional validation
	validateAddress(userAddress, "address");
	validatePhoneNumber(userPhoneNumber, "phoneNumber");
	validateEmail(userEmail, "email");
	userEmail = userEmail.toLowerCase(); 
	validateYear(vehicleYear, "vehicleYear");

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
			vehicles = [];
		}
		vehicles.push(vehicleObj);
		localStorage.setItem(VEHICLES_LS, JSON.stringify(vehicles));
	} else {
		return false; 
	}
}

function DisplayEnteredVehicle()
{
	let vehicles = JSON.parse(localStorage.getItem(VEHICLES_LS));
	const lastVehicle = vehicles[vehicles.length - 1];
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
	let ok = confirm("Would you like to clean the list?");
	if (ok) {
		localStorage.removeItem(VEHICLES_LS);
		location.reload();
	}
}

const capitalizeFirstLetter = (str) => {
	str = str.trim().toLowerCase();
	return str.charAt(0).toUpperCase() + str.slice(1);
}

const validateRequired = (value, id, label) => {
	value = capitalizeFirstLetter(value);
	if (value === "") {
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
		// value = capitalizeFirstLetter(value);
	}
	return value;
};

const validateAddress = (address, id) => {
	if (address !== "") {
		if (!addressRegex.test(address)) {
			errorCount++;
			document.getElementById(id).classList.add("err-input");
			document.getElementById(`${id}Error`).innerText = "Invalid address format";
		}
	}
};

const validatePhoneNumber = (phoneNumber, id) => {
	if (phoneNumber !== "") {
		if (!phoneNumberRegex.test(phoneNumber)) {
			errorCount++;
			document.getElementById(id).classList.add("err-input");
			document.getElementById(`${id}Error`).innerText = "Invalid phone number format";
		}
	}
};

const validateEmail = (email, id) => {
	if (email !== "") {
		if (!emailRegex.test(email)) {
			errorCount++;
			document.getElementById(id).classList.add("err-input");
			document.getElementById(`${id}Error`).innerText = "Invalid email format";
		}
	}
};

const validateYear = (year, id) => {
	let currentYear = new Date().getFullYear();
	if (year != "") {
		if (!yearRegex.test(year)) {
			errorCount++;
			document.getElementById(id).classList.add("err-input");
			document.getElementById(`${id}Error`).innerText = "Invalid year format";
		} else if (year.length != 4) {
			errorCount++;
			document.getElementById(id).classList.add("err-input");
			document.getElementById(`${id}Error`).innerText = "Invalid year format (yyyy)";
		} else if (year < 1900 || year > currentYear) {
			errorCount++;
			document.getElementById(id).classList.add("err-input");
			document.getElementById(`${id}Error`).innerText = "Vehicle year should be in range 1900 to current year.";
		}
	}
};

const loadVehicle = (obj, ele) => {
	const li = document.createElement("li");
	const linkBtn = document.createElement("a");
	const div = document.createElement("div");
	linkBtn.innerText = "Move to J.D. Power";
	linkBtn.href = `http://www.jdpower.com/cars/${obj.vehicleMake}/${obj.vehicleModel}/${obj.vehicleYear}`;
	linkBtn.target = "_blank";
	linkBtn.classList.add("btn-jd-power");

	div.innerHTML = `
		<label>Seller Name:</label> ${obj.sellerName} <br />
		<label>Address:</label> ${obj.userAddress} <br />
		<label>City:</label> ${obj.userCity} <br />
		<label>Phone Number:</label> ${obj.userPhoneNumber} <br />
		<label>Email:</label> ${obj.userEmail} <br />
		<label>Vehicle Make:</label> ${obj.vehicleMake} <br />
		<label>Vehicle Model:</label> ${obj.vehicleModel} <br />
		<label>Vehicle Year:</label> ${obj.vehicleYear} <br />
	`;

	li.appendChild(div);
	li.appendChild(linkBtn);
	ele.appendChild(li);
};
