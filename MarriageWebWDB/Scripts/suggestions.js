function acceptMatch(username) {

    if (!confirm("Are you sure?"))
        return;

    $.ajax({
        url: '/Home/Match',
        data: { username: username, accepted:true },
        dataType: 'html',
        success: function (result) {
            location.reload(); 
        }
    });
}

function rejectMatch(username) {

    if (!confirm("Are you sure?"))
        return;

    $.ajax({
        url: '/Home/Match',
        data: { username: username, accepted: false },
        dataType: 'html',
        success: function (result) {
            location.reload(); 
        }
    });
}