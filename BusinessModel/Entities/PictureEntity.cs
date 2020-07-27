using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Entities
{
    class PictureEntity
    {
        public string PictureId { get; set; }

        public byte[] Picture { get; set; }

        public int UserProfileId { get; set; }

    }
}
