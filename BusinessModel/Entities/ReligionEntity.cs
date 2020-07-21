using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Entities
{
    public class ReligionEntity
    {
        public int ReligionId { get; set; }

        public string ReligionName { get; set; }

        override
        public string ToString()
        {
            return this.ReligionName;
        }
    }
}
