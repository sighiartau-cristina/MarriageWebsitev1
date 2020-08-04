



chatHub.client.onUserDisconnected = function (connectionId, userName) // On User Disconnected
{
    var disUserName = $('.usr-mg-info #sender-username').text();
    if (disUserName == userName) {
        $('#sender-status').text("Offline");
    }
    $('#posted_time-' + userName).text("Offline");
    console.log(userName + " disconnected!");
};


