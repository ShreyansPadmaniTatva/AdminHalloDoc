function mode() {
    var body = document.body;
    var currentTheme = body.getAttribute('data-bs-theme');

    var newTheme = currentTheme === 'light' ? 'dark' : 'light';

    body.setAttribute('data-bs-theme', newTheme);

    console.log(body.getAttribute('data-bs-theme'));
}
$(document).ready(function () {
    $(".t-tab").click(function () {
        $(".t-tab").removeClass("active");
        $(this).addClass("active");

    });
});
$(function () {
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

const phoneInputField = document.querySelector("#phone");
const phoneInput = window.intlTelInput(phoneInputField, {
    separateDialCode: true,
    hiddenInput: "full",
    preferredCountries: ["us", "co", "in", "de"],
    utilsScript:
        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});
const phoneInputField1 = document.querySelector("#phone1");
const phoneInput1 = window.intlTelInput(phoneInputField1, {
    separateDialCode: true,
    hiddenInput: "full",
    preferredCountries: ["us", "co", "in", "de"],
    utilsScript:
        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
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
