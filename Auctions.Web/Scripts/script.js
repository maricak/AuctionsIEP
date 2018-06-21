$(function () {
    $(".time").each(function () {
        var that = this;

        if (this.innerText === "--:--:--") {
            return;
        }

        var timer = setInterval(function () {
            if (getDuration(that.innerText) === 0) {
                clearInterval(timer);

                $(that).html("--:--:--");
                $(that).removeClass("text-warning");

                // set sold picture
                $(that).parent().find(".over").removeClass("d-none");

                // if there is no winner set this text
                $(that).parent().find(".first-one").html("No winner");

                // set buttons. Remove Bid and add Sold
                $(that).parent().parent().find(".bid-form").addClass("d-none");
                $(that).parent().parent().find(".btn-sold").removeClass("d-none");

            } else {
                if (duration <= 11) {
                    $(that).addClass("text-warning");
                }
                // count down
                $(that).html(formatTime(getDuration(that.innerHTML) - 1));
            }
        }, 1000);
    });

    $(".time-details").each(function () {
        var that = this;

        if (this.innerText === "--:--:--") {
            return;
        }
        var timer = setInterval(function () {
            var duration = getDuration(that.innerText);

            if (duration === 0) {

                clearInterval(timer);

                $(that).html("--:--:--");
                $(that).removeClass("text-warning");

                // set sold picture
                $(that).parent().parent().parent().parent().find(".over").removeClass("d-none");

                if ($(that).parent().parent().parent().parent().find(".bidder-label").innerHTML === "") {
                    // no winner
                    $(that).parent().parent().parent().parent().find(".first-one").html("No winner");
                } else {
                    //winner
                    $(that).parent().parent().parent().parent().find(".bidder-label").html("Winner: ");
                }

                // set buttons. Remove Bid and add Sold
                $(that).parent().parent().parent().parent().parent().find(".bid-form").addClass("d-none");
                $(that).parent().parent().parent().parent().parent().find(".btn-sold").removeClass("d-none");

            } else {
                if (duration <= 11) {
                    $(that).addClass("text-warning");
                }
                // count down
                $(that).html(formatTime(duration - 1));
            }
        }, 1000);
    });
});

function formatTime(duration) {
    var hours = parseInt(duration / 3600),
        remainder = duration % 3600,
        minutes = parseInt(remainder / 60),
        seconds = remainder % 60;

    var hoursStr = hours + "";
    if (hours < 10) hoursStr = "0" + hoursStr;
    var minutesStr = minutes + "";
    if (minutes < 10) minutesStr = "0" + minutesStr;
    var secondsStr = seconds + "";
    if (seconds < 10) secondsStr = "0" + secondsStr;

    return (hoursStr + ":"
        + minutesStr + ":"
        + secondsStr);
}

function getDuration(time) {
    var arr = time.split(":");

    var seconds = (+arr[0]) * 60 * 60 + (+arr[1]) * 60 + (+arr[2]);

    return seconds;
}