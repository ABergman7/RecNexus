var update = function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    } else {
        x.innerHTML = "Geolocation is not supported by this browser.";
    }

   
}

function showPosition(position) {
    document.getElementById('CurrentLatitude').value = position.coords.latitude;
    document.getElementById('CurrentLongitude').value = position.coords.longitude;
    }

window.onload = update;

