var body = document.body;
var currentTheme = localStorage.getItem("Theme");
body.setAttribute('data-bs-theme', currentTheme);
if (currentTheme == 'dark') {
    document.getElementById('thm').style.display = 'block';
    document.getElementById('thm2').style.display = 'none';
}
else {
    document.getElementById('thm2').style.display = 'block';
    document.getElementById('thm').style.display = 'none';
}
function mode() {

    var body = document.body;
    // var currentTheme = body.getAttribute('data-bs-theme');
    var currentTheme = localStorage.getItem("Theme");

    // Toggle between 'light' and 'dark'
    var newTheme = currentTheme === 'light' ? 'dark' : 'light';
    if (newTheme == 'dark') {
        document.getElementById('thm').style.display = 'block';
        document.getElementById('thm2').style.display = 'none';
    }
    else {
        document.getElementById('thm2').style.display = 'block';
        document.getElementById('thm').style.display = 'none';
    }
    // Update the data-bs-theme attribute
    body.setAttribute('data-bs-theme', newTheme);
    localStorage.setItem("Theme", newTheme);

    console.log(body.getAttribute('data-bs-theme'));
}
function goBack() {
    // Check if there is a previous unique URL
    var previousUrl = getPreviousUniqueUrl();
    if (previousUrl) {
        // If there is a previous URL, navigate to it
        window.location.href = previousUrl;
    } else {
        // If there is no previous URL, go back in history
        history.back();
    }
}

let navLinks = document.querySelectorAll('.nav-link');

// Loop through each 'nav-link' element
navLinks.forEach(function (navLink) {
    // Check if the 'nav-link' element has the class 'active'
    if (navLink.classList.contains('active')) {
        // If it has the class 'active', find its parent element with the class 'nav-item' and add a CSS class to it
        let parentNavItem = navLink.closest('.nav-item');
        if (parentNavItem) {
            parentNavItem.classList.add('active-item');
        }
    }
});
// Function to get the previous unique URL
function getPreviousUniqueUrl() {
    var currentUrl = window.location.href;
    var uniqueUrls = JSON.parse(localStorage.getItem('uniqueUrls')) || [];
    var previousUrl = uniqueUrls.pop();
    while (previousUrl === currentUrl) {
        // Remove current URL if it's the same as the previous one
        previousUrl = uniqueUrls.pop();
    }
    // Update the unique URLs in local storage
    localStorage.setItem('uniqueUrls', JSON.stringify(uniqueUrls));
    return previousUrl;
}
function emptyuniqueurl() {
    localStorage.setItem('uniqueUrls', null);
}
// Store the current URL when the page loads
var currentUrl = window.location.href;
var uniqueUrls = JSON.parse(localStorage.getItem('uniqueUrls')) || [];
if (!uniqueUrls.includes(currentUrl)) {
    uniqueUrls.push(currentUrl);
    localStorage.setItem('uniqueUrls', JSON.stringify(uniqueUrls));
}

function validateForm() {
    // Implement your validation logic here
    var formIsValid = true; // Assume form is valid by default

    // Example: Validate form fields
    var inputs = document.querySelectorAll('#formAddEditSave_UMS inputs');
    inputs.forEach(function (input) {
        if (!input.value) {
            formIsValid = false;
        }
    });
    var textarea = document.querySelectorAll('#formAddEditSave_UMS textarea');
    textarea.forEach(function (input) {
        if (!input.value) {
            formIsValid = false;
        }
    });
    var select = document.querySelectorAll('#formAddEditSave_UMS select');
    select.forEach(function (input) {
        if (!input.value) {
            formIsValid = false;
        }
    });
    // Return validation result
    return formIsValid;
}
var duplicateDivs = document.querySelectorAll('.modal-backdrop.fade.show');

// Remove duplicate elements except the first one
for (var i = 1; i < duplicateDivs.length; i++) {
    duplicateDivs[i].parentNode.removeChild(duplicateDivs[i]);
}
$(document).ready(function () {
    // Remove duplicate elements except the first one
    $('.modal-backdrop.fade.show:gt(0)').remove();
    $(".t-tab").click(function () {
        $(".t-tab").removeClass("active");
        $(this).addClass("active");

    });
});
(function () {
    'use strict'

    var forms = document.querySelectorAll('.needs-validation')

    Array.prototype.slice.call(forms)
        .forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault()
                    event.stopPropagation()
                }

                form.classList.add('was-validated')
            }, false)
        })
})()
$(function () {
    // Add hover-triggered dropdown behavior
    $('.dropdown').hover(function () {
        var $dropdownMenu = $(this).find('.dropdown-menu');
        if ($dropdownMenu.length === 0) {
            console.log("Dropdown menu not found");
            return;
        }

        $dropdownMenu.addClass('show');
        // Adjust position if menu extends beyond viewport
        var dropdownMenuRect = $dropdownMenu[0].getBoundingClientRect();
        var windowWidth = window.innerWidth;
        var windowHeight = window.innerHeight;

        // Check if menu extends beyond the right of the viewport
        if (dropdownMenuRect.right > windowWidth) {
            $dropdownMenu.css('right', 0);
            $dropdownMenu.css('left', 'auto');
        }
        // Check if menu extends beyond the bottom of the viewport
        if (dropdownMenuRect.bottom > windowHeight) {
            $dropdownMenu.css('top', 'auto');
            $dropdownMenu.css('bottom', '100%');
        }
        // Check if menu extends beyond the top of the viewport
        if (dropdownMenuRect.top < 0) {
            $dropdownMenu.css('top', '100%');
            $dropdownMenu.css('bottom', 'auto');
        }
    }, function () {
        $(this).find('.dropdown-menu').removeClass('show');
    });

    // Close dropdown when clicked outside
    $(document).on('click', function (e) {
        if (!$(e.target).closest('.dropdown').length) {
            $('.dropdown-menu').removeClass('show');
        }
    });

    $('#staticBackdrop').modal('show');
    $(".t-tab").click(function () {
        $('#requeststatus').val(this.value);
        $('.exerequeststatus').val(this.value);
        $('#submit').click();

    });
    $(".request-type-btn").click(function () {
        $('#requesttypebtn').val(this.value);
        $('#exerequesttypebtn').val(this.value);
        $('.t-tab.active').click();

    });
    $(".regionDashboard").change(function () {
        $('#requestRegionId').val(this.value);
        $('#exerequestRegionId').val(this.value);
        $('.t-tab.active').click();

    });
    $("#getSearchInput").keyup(function () {
        $('#SearchInput').val(this.value.trim());
        $('#exeSearchInput').val(this.value.trim());
        console.log($('#SearchInput').val());
        $('.t-tab.active').click();

    });
    $("form").on("change", ".file-upload-field", function () {
        $(this).parent(".file-upload-wrapper").attr("data-text", $(this).val().replace(/.*(\/|\\)/, ''));
    });
    $("form").on("change", ".file-upload-field1", function () {
        $(this).parent(".file-upload-wrapper").attr("data-text", $(this).val().replace(/.*(\/|\\)/, ''));
    });
 
    $(".toggle--open").on("click", function () {
        $("#navlist").attr("data-hidden", "false");
        $(this).attr("aria-expanded", "true");
    });

    $(".toggle--close").on("click", function () {
        $("#navlist").attr("data-hidden", "true");
        $(".toggle--open").attr("aria-expanded", "false");
    });

});


var buttons = document.querySelectorAll('.request-type-btn');

// Add click event listeners to each button
buttons.forEach(function (button) {
    button.addEventListener('click', function () {
        // Remove 'active' class from all buttons
        buttons.forEach(function (btn) {
            btn.classList.remove('active');
        });

        // Add 'active' class to the clicked button
        button.classList.add('active');
    });
});

function oops(title) {
    Swal.fire({
        icon: "error",
        title: "Oops...",
        text: title
    });
}
function cancelalt(title) {
    const Toast = Swal.mixin({
        toast: true,
        position: "top-end",
        showConfirmButton: false,
        timer: 3000,
        iconColor: 'black',
        customClass: {
            popup: 'colored-toast-cancel',
        },
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.onmouseenter = Swal.stopTimer;
            toast.onmouseleave = Swal.resumeTimer;
        }
    });
    Toast.fire({
        icon: "danger",
        title: title
    });
}


function savealt(title) {
    const Toast = Swal.mixin({
        toast: true,
        position: "top-end",
        showConfirmButton: false,
        timer: 3000,
        iconColor: 'black',
        customClass: {
            popup: 'colored-toast',
        },
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.onmouseenter = Swal.stopTimer;
            toast.onmouseleave = Swal.resumeTimer;
        }
    });
    Toast.fire({
        icon: "success",
        title: title
    });
}
document.addEventListener("DOMContentLoaded", function () {
    const phoneInputFieldphone = document.querySelector("#phone");
    const phoneInput = window.intlTelInput(phoneInputFieldphone, {
        separateDialCode: true,
        hiddenInput: "country_code",
        preferredCountries: ["us", "co", "in", "de"],
        utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
    });

    // Trigger the input event to format the phone number initially
    phoneInputFieldphone.dispatchEvent(new Event('input'));

    phoneInputFieldphone.addEventListener("input", function () {
        // Get the formatted phone number
        const formattedNumber = phoneInput.getNumber();
        // Update the input field value with the formatted number
        document.getElementById("phone").value = formattedNumber;
    });
   

const phoneInputField1 = document.querySelector("#phone1");
const phoneInput1 = window.intlTelInput(phoneInputField1, {
    separateDialCode: true,
    hiddenInput: "country_code",
    preferredCountries: ["us", "co", "in", "de"],
    utilsScript:
        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});

phoneInputField1.addEventListener("input", function () {
    const formattedNumber = phoneInput1.getNumber();
    // Update the input field value with the formatted number
    document.getElementById("phone1").value = formattedNumber;

});
    function dispatchInputEvent() {
        phoneInputFieldphone.dispatchEvent(new Event('input'));
        phoneInputField1.dispatchEvent(new Event('input'));
    }

    var delayMilliseconds = 100;

    setTimeout(dispatchInputEvent, delayMilliseconds);
});
function getLocation() {
    const x = document.getElementById("map");

    if (navigator.geolocation) {
        console.log(51);
        navigator.geolocation.watchPosition(showPosition);
        console.log(51545);
    } else {
        x.innerHTML = "Geolocation is not supported by this browser.";
    }
}

function showPosition(position) {

    const x = document.getElementById("map");
    x.innerHTML = "Latitude: " + position.coords.latitude +
        "<br>Longitude: " + position.coords.longitude;
    document.getElementById("latitude").value = position.coords.latitude;
    document.getElementById("longitude").value = position.coords.longitude;
}


const phoneInputField11 = document.querySelector("#phone11");
const phoneInput11 = window.intlTelInput(phoneInputField11, {
    separateDialCode: true,
    hiddenInput: "full",
    preferredCountries: ["us", "co", "in", "de"],
    utilsScript:
        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});

function CheckPassword(inputtxt) {
    var decimal = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,15}$/;
    var submitButton = inputtxt.closest('form').querySelector('input[type="submit"]');
    if (!inputtxt.value.match(decimal)) {
        $("#perror").show();
        submitButton.disabled = true;
        return true;
    }
    else {
        $("#perror").hide();
        submitButton.disabled = false;
        return false;
    }
} 

function passtoggle(x) {
    var parentDiv = x.closest('.password-hide, .password');
    var passwordInput = parentDiv.querySelector('.password-input');
    var showEyes = parentDiv.querySelectorAll('.bi-eye');
    var hideEyes = parentDiv.querySelectorAll('.bi-eye-slash');

    if (passwordInput.type === "password") {
        passwordInput.type = "text";
        showEyes.forEach(i => i.style.display = "none");
        hideEyes.forEach(i => i.style.display = "block");
    } else {
        passwordInput.type = "password";
        hideEyes.forEach(i => i.style.display = "none");
        showEyes.forEach(i => i.style.display = "block");
    }
}