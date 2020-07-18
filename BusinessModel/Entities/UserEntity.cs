using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Entities
{
    public class UserEntity
    {

        public int UserId { get; set; }

        public string UserEmail { get; set; }

        public string UserUsername { get; set; }

        public string UserPassword { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
