// cookies.js
// Function to set a cookie
function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000)); // Corrected days calculation
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/"; // Corrected path
}

// Function to delete a cookie
function deleteCookie(name) {
    document.cookie = name + '=; Max-Age=-99999999; path=/'; // Added path for consistency
}

// Function to get a cookie
function getCookie(name) {
    var nameEQ = name + "="; // Corrected string concatenation
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i]; // Corrected variable name
        while (c.charAt(0) === ' ') c = c.substring(1, c.length); // Trim leading spaces
        if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length, c.length); // Return cookie value
    }
    return null; // Return null if cookie not found
}