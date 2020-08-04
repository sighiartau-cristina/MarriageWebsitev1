using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarriageWebWDB.Hubs
{
    public class UserDetails
    {
        public string ConnectionId { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
    }

}