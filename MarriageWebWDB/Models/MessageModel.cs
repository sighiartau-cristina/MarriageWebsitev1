using System;

namespace MarriageWebWDB.Models
{
    public class MessageModel
    {
        public string SenderUsername { get; set; }

        public string ReceiverUsername { get; set; }

        public string MessageText { get; set; }

        public DateTime SendDate { get; set; }

        public DateTime ReadDate { get; set; }

        //public string Status { get; set; }
    }
}
