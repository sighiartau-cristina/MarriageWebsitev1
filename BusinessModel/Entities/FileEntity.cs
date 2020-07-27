using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessModel.Contracts;

namespace BusinessModel.Entities
{
    public class FileEntity
    {
        public int FileId { get; set; }

        public string FileName { get; set; }
    
        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        public FileType FileType { get; set; }

        public int UserProfileId { get; set; }

    }
}
