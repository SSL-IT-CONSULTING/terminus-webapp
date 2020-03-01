using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace terminus.shared.models
{
    public class UploadFilesViewModal
    {

        [Key, MaxLength(36)]
        public string id { get; set; }

        [MaxLength(100)]
        public string FileName { get; set; }

        [MaxLength(36)]
        public string propertyDirectoryId { get; set; }

        public int fileSize { get; set; }
        
        [MaxLength(100)]
        public string fileType { get; set; }

        [MaxLength(500)]
        public int filePath { get; set; }
    }
}
