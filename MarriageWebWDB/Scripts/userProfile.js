$(document).ready(function () {
    var lengthLikes = document.getElementById("likes-list").getElementsByTagName("li").length;
    var lengthDislikes = document.getElementById("dislikes-list").getElementsByTagName("li").length;

    if (lengthLikes == 0) {
        $('#noLikesLabel').text("No Likes");
        document.getElementById("showLikesDeleteButton").style.display = "none";
    }
    else {
        $('#noLikesLabel').text("");
        document.getElementById("showLikesDeleteButton").style.display = "block";
    }

    if (lengthDislikes == 0) {
        $('#noDislikesLabel').text("No Dislikes");
        document.getElementById("showDislikesDeleteButton").style.display = "none";
    }
    else {
        $('#noDislikesLabel').text("");
        document.getElementById("showDislikesDeleteButton").style.display = "block";
    }
});

function DeletePreference(like) {

    var str;

    if (like == true) str = "likes";
    else str = "dislikes";

    $('input[name="' + str + '"]').each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            $.ajax({
                url: '/Account/DeletePreference',
                data: { id: this.id, like: like },
                dataType: 'html',
                success: function (result) { }
            });
            $(this).parent().remove();
        }
    });
}

function AddPreference(like) {

    var prefName;
    var prefId;
    var str;
    var title;

    if (like == true) {
        prefName = $('#likes').val();
        prefId = $('#likes-id').val();
        str = "likes";
        title = "Likes";
    }
    else {
        prefName = $('#dislikes').val();
        prefId = $('#dislikes-id').val();
        str = "dislikes";
        title = "Dislikes";
    }

    if (prefName == "") return;

    if (prefId == "") prefId = 0;

    $.ajax({
        url: '/Account/AddPreference',
        data: { name: prefName, id: prefId, like: like },
        dataType: 'html',
        success: function (result) {

            var newline = "";
            newline += '<li><input type="checkbox" id="' + result + '" name="' + str + '" <label for="' + result + '">' + prefName + '</label></li>';
            $('#' + str + '-list').append(newline);

            var length = document.getElementById(str + '-list').getElementsByTagName("li").length;

            if (length > 0) {
                $('#no' + title + 'Label').text("");
                document.getElementById("show" + title + "DeleteButton").style.display = "block";
            }
        }
    });
}

$(function () {

    $("#deleteLikesButton").click(function () {

        DeletePreference(true);

        var length = document.getElementById("likes-list").getElementsByTagName("li").length;

        if (length == 0) {
            $('#noLikesLabel').text("No Likes");
            document.getElementById("showLikesDeleteButton").style.display = "none";
        }
    })

    $("#deleteDislikesButton").click(function () {

        DeletePreference(false);

        var length = document.getElementById("dislikes-list").getElementsByTagName("li").length;

        if (length == 0) {
            $('#noDislikesLabel').text("No Dislikes");
            document.getElementById("showDislikesDeleteButton").style.display = "none";
        }
    })

    $("#addLikeButton").click(function () {

        AddPreference(true);
    });

    $("#addDislikeButton").click(function () {

        AddPreference(false);
    });
});

$(function () {
    $("#likes").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "/Account/GetAllLikes",
                dataType: "json",
                data: {
                    term: request.term
                },
                success: function (data) {
                    $("#likes-id").val("");
                    response(data);
                }
            });
        },
        focus: function (event, ui) {
            $("#likes").val(ui.item.Name);
            return false;
        },
        select: function (event, ui) {
            $("#likes").val(ui.item.Name);
            $("#likes-id").val(ui.item.Id);

            return false;
        }
    })
        .autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
                .append("<div>" + item.Name + "</div>")
                .appendTo(ul);
        };
});

$(function () {
    $("#dislikes").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "/Account/GetAllLikes",
                dataType: "json",
                data: {
                    term: request.term
                },
                success: function (data) {
                    $("#dislikes-id").val("");
                    response(data);
                }
            });
        },
        focus: function (event, ui) {
            $("#dislikes").val(ui.item.Name);
            return false;
        },
        select: function (event, ui) {
            $("#dislikes").val(ui.item.Name);
            $("#dislikes-id").val(ui.item.Id);

            return false;
        }
    })

        .autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
                .append("<div>" + item.Name + "</div>")
                .appendTo(ul);
        };
});
