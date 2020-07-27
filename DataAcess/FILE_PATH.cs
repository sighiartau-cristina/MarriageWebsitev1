using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Azure.Management.BatchAI.Fluent.Models;


namespace DataAccess
{

    [Table("FILE_PATH")]
    public partial class FILE_PATH
    {
        public int FilePathId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        public FileType FileType { get; set; }
        public int UserProfileID { get; set; }
        public virtual USER_PROFILE USER_PROFILE { get; set; }
    }
}

