$(function () {
    $.ajax({
        type: "POST",
        data: "{count:10}",
        dataType: "json",
        url: "/Timeline.asmx/GetTweets",
        contentType: "application/json; charset=utf-8",
        success: onSuccess,
        cache: false,
        error: ajaxFailed
    });
});

function onSuccess(data, status) {
    $("#tweets").empty();
    for (var i = 0; i < data.d.length; i++) {
        $("#tweets").append("<div>" +
                            "   <div><img src=\"" + data.d[i].ProfileImage + "\" alt=\"\"/>" +
                            "       <h1>" + data.d[i].DisplayName + "</h1>" +
                            "   </div>" +
                            "   <div>" + data.d[i].StatusHtml + "</div>" +
                            "</div>");
    }
}

function ajaxFailed(error) {
}