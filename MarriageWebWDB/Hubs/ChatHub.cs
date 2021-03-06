﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BusinessModel.Contracts;
using BusinessModel.Entities;
using BusinessModel.Handlers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace MarriageWebWDB.Hubs
{
    [HubName("chatHub")]
    public class ChatHub : Hub, IReadOnlySessionState

    {
        static List<UserDetails> ConnectedUsers = new List<UserDetails>(); // list of connected users

        public void Connect()
        {
            int userId = int.Parse(Context.QueryString["userId"]);

            string username = new UserHandler().Get(userId).Entity.UserUsername;

            string connectionId = Context.ConnectionId;

            if (ConnectedUsers.Count(x => x.ConnectionId.Equals(connectionId)) == 0)
            {
                ConnectedUsers.Add(new UserDetails { ConnectionId = Context.ConnectionId, UserID = userId, UserName = username });

                Clients.Caller.onConnected(username);
                Clients.Others.onUserConnected(connectionId, username);
            }
        }

        public void UserStatus(string username)
        {
            var user = ConnectedUsers.FirstOrDefault(x => x.UserName.Equals(username));

            if (user != null)
            {
                Clients.Caller.setOnline(username);
            }
        }

        public bool SendPrivateMessage(string toUserName, string msg)
        {

            string fromconnectionid = Context.ConnectionId;
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId.Equals(fromconnectionid));
            var toUser = ConnectedUsers.FirstOrDefault(x => x.UserName == toUserName);

            var receiver = new UserHandler().GetByUsername(toUserName);
            if (!receiver.CompletedRequest)
            {
                return false;
            }

            if (receiver.Entity == null || fromUser==null)
            {
                return false;
            }

            var message = new MessageEntity
            {
                MessageText = msg,
                SenderId = fromUser.UserID,
                ReceiverId = receiver.Entity.UserId,
                SendDate = DateTime.Now,
                Status = MessageStatus.Sent
            };

            var handler = new MessageHandler().Add(message);

            if (!handler.CompletedRequest)
            {
                return false;
            }

            Clients.Caller.addMessage(fromUser.UserName, msg, handler.Entity.SendDate.ToString(), handler.Entity.MessageId);

            // if the other person is online, send message
            if (toUser != null)
            {
                Clients.Client(toUser.ConnectionId).sendPrivateMessage(fromUser.UserName, msg, handler.Entity.SendDate.ToString(), handler.Entity.MessageId);
            }

            return true;
        }

        public void SetTyping(string toUserName)
        {
  
            string fromconnectionid = Context.ConnectionId;

            var toUser = ConnectedUsers.FirstOrDefault(x => x.UserName.Equals(toUserName));
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId.Equals(fromconnectionid));

            if (toUser != null && fromUser!=null) // if the person we talk to is online
            {
                    Clients.Client(toUser.ConnectionId).setTyping(fromUser.UserName);
            }
        }

        public void ChangeMessageStatus(string toUserName)
        {

            string fromconnectionid = Context.ConnectionId;

            var toUser = ConnectedUsers.FirstOrDefault(x => x.UserName.Equals(toUserName));
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId.Equals(fromconnectionid));

            if (toUser != null && fromUser != null) // if the person we talk to is online
            {
                Clients.Client(toUser.ConnectionId).changeMessageStatus();
            }
        }

        public void ArchiveMessage(string toUserName, int messageId)
        {

            string fromconnectionid = Context.ConnectionId;

            var toUser = ConnectedUsers.FirstOrDefault(x => x.UserName.Equals(toUserName));

            if (toUser != null) // if the person we talk to is online
            {
                Clients.Client(toUser.ConnectionId).archiveMessage(messageId);
            }
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var connectionId = Context.ConnectionId;
            var user = ConnectedUsers.Where(x => x.ConnectionId.Equals(connectionId)).FirstOrDefault();

            if (user != null)
            {
                ConnectedUsers.Remove(user);
                Clients.Others.onUserDisconnected(connectionId, user.UserName);
            }

            return base.OnDisconnected(stopCalled);
        }

    }
}
