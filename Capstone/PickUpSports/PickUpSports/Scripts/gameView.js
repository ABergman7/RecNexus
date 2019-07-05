$(function() {
    $('input[name="datetimes"]').daterangepicker({
        timePicker: true,
        startDate: moment().startOf("hour"),
        endDate: moment().startOf("hour").add(32, "hour"),
        locale: {
            format: "MM/DD//YYYY hh:mm A"
        }
    });
    $("#datetimes").daterangepicker({
        timePicker: true,
        startDate: moment().startOf("hour"),
        endDate: moment().startOf("hour").add(32, "hour"),
        locale: {
            format: "MM/DD/YYYY hh:mm A"
        }
    });
});

function getVenueBusinessHours() {
    var id = document.getElementById("VenueId").value;
    

    $.ajax({
            type: "GET",
            url: "../../Game/BusinessHoursByVenueId/" + id
        })
        .done(function(partialViewResult) {
            $("#hours").html(partialViewResult);
        });
};
