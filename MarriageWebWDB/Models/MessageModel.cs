using System;
using BusinessModel.Contracts;

namespace MarriageWebWDB.Models
{
    public class MessageModel
    {
        public string SenderUsername { get; set; }

        public string ReceiverUsername { get; set; }

        public string MessageText { get; set; }

        public DateTime SendDate { get; set; }

        public string ReadDate { get; set; }

        public MessageStatus Status { get; set; }
    }
}
