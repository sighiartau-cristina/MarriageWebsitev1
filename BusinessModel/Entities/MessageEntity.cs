using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessModel.Contracts;

namespace BusinessModel.Entities
{
    public class MessageEntity
    {
        public int MessageId { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public string MessageText { get; set; }

        public DateTime SendDate { get; set; }

        public DateTime? ReadDate { get; set; }

        public MessageStatus Status { get; set; }
    }
}
