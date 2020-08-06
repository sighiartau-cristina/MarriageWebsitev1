using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Entities
{
    public class PreferenceEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        override
        public string ToString()
        {
            return Name;
        }
    }
}
