﻿function fun(msgId) {
            //console.log(msgId)
             $.ajax({
                 url: '/Home/ArchiveMessage/',
                 contentType: 'application/html ; charset:utf-8',
                 type: 'GET',
                 data: { messageId: msgId, username: toUserName },
                 dataType: 'html',
                 success: function (response) { getAll(); }
             });
        }

        function getAll() {
             var model = $('#dataModel');
             $.ajax({
                 url: '/Home/GetChatHistory/',
                 contentType: 'application/html ; charset:utf-8',
                 type: 'GET',
                 data: { username: toUserName },
                 dataType: 'html',
                 success: function (result) {
                     model.empty().append(result);
                     var mydiv = $("#dataModel");
                     mydiv.scrollTop(mydiv.prop("scrollHeight"));
                 }
             });
        }

        $(function () {
            var chatHub = $.connection.chatHub; // SignalR Class Name = ChatHub
            $.connection.hub.qs = { "userId": userId };

            $.connection.hub.start().done(function ()
            {
                chatHub.server.connect();
            })

            chatHub.client.onConnected = function (username) // On connected method
            {
                console.log(username + " connected!");
                var toUserName = toUserName;
                chatHub.server.userStatus(toUserName);
                getAll();
            };

            chatHub.client.sendPrivateMessage = function (toUserName, msg, sendDate) // After sending message to do
            {
                var result = "";
                result += '<div class="chat-container">' +
                    '<img class="left" style="height:50px; width:50px;" src="../../File/UserFile/?id=' + toUserName + '" onerror="this.onerror=null;this.src=' + "'" + '../../Images/untitled.jpg' + "'" + ';" />' +
                    '<p align="left" style="color:black">' + msg + '</p>' +
                    '<span class="time-right" style="color:black">' + sendDate + '</span></div>';
                $('#dataModel').append(result);
                var mydiv = $("#dataModel");
                mydiv.scrollTop(mydiv.prop("scrollHeight"));
            };

            chatHub.client.addMessage = function (myUserName, msg, sendDate, messageId) // add message
            {
                var result = "";
                result += '<div class="chat-container darker">' +
                    '<img class="right" style="height:50px; width:50px;" src="../../File/UserFile/?id=' + myUserName + '" onerror="this.onerror=null;this.src=' + "'" + '../../Images/untitled.jpg' + "'" + ';" />' +
                    '<p align="right">' + msg + '</p>' +
                    '<span class="time-left" style="color:white">' + sendDate + '  • </span>' +
                    '<span class="time-left" style="color:white"><a href="../ArchiveMessage?messageId=' + messageId + '&username=' + toUserName + '"> Archive</span></div>';
                $('#dataModel').append(result);
                $("#lastStatus").text('Sent');
                var mydiv = $("#dataModel");
                mydiv.scrollTop(mydiv.prop("scrollHeight"));
            };

            var typingListener;
            chatHub.client.setTyping = function (fromUserName) // setting typing text
            {
                    $("#sender-status").text(fromUserName + ' is typing...');
                            clearTimeout(typingListener);
                            typingListener = setTimeout(function () {$("#sender-status").text('');}, 5000);
            };

            chatHub.client.changeMessageStatus = function ()
            {
                $("#lastStatus").text('Seen');
            };

            $('#messageInput').keypress(function (e) // keypress on message input
            {
                            if (e.which == 13) //enter
                            {
                    $("#sendMessageButton").click();
                                return false;
                            }
                            else {
                                chatHub.server.setTyping(toUserName);
                            }
            });

            $('#messageInput').click(function (e)
            {
                chatHub.server.changeMessageStatus(toUserName);
            });

            $("#sendMessageButton").click(function ()
            {
                            var msg = $('#messageInput').val();
                            if (msg == "") return;
                            chatHub.server.sendPrivateMessage(toUserName, msg);
                $('#messageInput').val("");
            });


            chatHub.client.onUserDisconnected = function (connectionId, userName) // On User Disconnected
            {
                if (toUsername == userName) {
                    $('#status').text("Offline");
                }
                console.log(userName + " disconnected!");
            };

            chatHub.client.onUserConnected = function (connectionId, userName) // On User Connected
            {
                if (toUserName == userName) {
                    $('#status').text("Online");
                }
                console.log(userName + " connected!");
            };

            chatHub.client.setOnline = function (userName) // On User Connected
            {
                $('#status').text("Online");
            };

        });