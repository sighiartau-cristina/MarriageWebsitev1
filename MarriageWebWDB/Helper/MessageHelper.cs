using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using BusinessModel.Entities;
using MarriageWebWDB.Models;

namespace MarriageWebWDB.Helper
{
    public class MessageHelper
    {
        public ICollection<MessageModel> GetMessageEntities(ICollection<MessageEntity> messages, string from, string to)
        {
            var list = messages.Select(m => ConvertToModel(m, from, to)).ToList();
            return list;
        }

        public MessageModel ConvertToModel(MessageEntity message, string from, string to)
        {
            if (message == null || string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                return null;
            }

            string readDate = message.ReadDate == null ? ("Not opened") : ("Read: " + message.ReadDate.ToString());

            var result = new MessageModel
            {
                MessageText = message.MessageText,
                ReceiverUsername = to,
                SenderUsername = from,
                ReadDate = readDate,
                SendDate = message.SendDate,
                Status = message.Status,
                MessageId = message.MessageId
            };

            if((int)HttpContext.Current.Session["userId"] == message.SenderId)
            {
                result.SenderUsername = from;
            }
            else
            {
                result.SenderUsername = to;
            }

            return result;
        }

        public MessageEntity ToDataEntity(MessageModel message)
        {
            if(message == null)
            {
                return null;
            }

            return new MessageEntity
            {
                MessageId = message.MessageId,
                Status = message.Status
            };
        }

    }
}