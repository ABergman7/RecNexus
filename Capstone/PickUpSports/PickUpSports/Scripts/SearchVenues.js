/*getting results and manipulating the DOM */
function gameList(result) {
	$("#myList").empty();
	$("#myList").html(result);

}

/* error on AJAX in case something goes wrong*/
function errorOnAjax() {
	console.log("error");
}

/*main method, performs AJAX with dropdown list */
function getGamesByVenue() {

	console.log("loaded!");

			var id = $("#venueList").val();

			$.ajax({
				type: "GET",
				url: "../../Game/GetGamesResult/",
				data: { "venueId": id },
				success: gameList

			});

}

function getGamesBySport() {

	console.log("loaded!");

	var id = $("#sportList").val();

	$.ajax({
		type: "GET",
		url: "../../Game/SearchBySport/",
		data: { "sportId": id },
		success: gameList

	});

}

$('#datetimes').on('apply.daterangepicker', function (ev, picker) {

    console.log("loaded!");

    var id = $("#datetimes").val();
    $.ajax({
        type: "GET",
        url: "../../Game/TimeFilter/",
        data: { "dateRange": id},
        success: gameList
    });

})
