function showPlayers(results) {
	console.log(results);
    $('#myList').empty();
    $('#myList').html(results);

}

function main() {

	var id = document.getElementById("GameId").value;

    console.log("loaded!");

    var ajaxCall = function() {
	    $.ajax({
		    type: "GET",
		    url: "/Game/PlayerList",
		    data: { "gameId": id },
		    success: showPlayers
	    });
    };

    window.onload = ajaxCall;

    var interval = 1000 * 5;
    window.setInterval(ajaxCall, interval);

}

/*Main method */
$(document).ready(main);