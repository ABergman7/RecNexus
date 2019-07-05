$(function () {

    var id = $("#datetimes").val().split('-');
    var start = id[0];
    var end = id[1];

	$('input[name="datetimes"]').daterangepicker({
		timePicker: true,
        startDate: start,
		endDate: end,
		locale: {
            format: "MM/DD/YYYY hh:mm A"
		}
	});
	$("#datetimes").daterangepicker({
		timePicker: true,
        startDate: start,
        endDate: end,
		locale: {
            format: "MM/DD/YYYY hh:mm A"
		}
	});
});