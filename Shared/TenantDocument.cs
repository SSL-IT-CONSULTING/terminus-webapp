﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class TenantDocument : TBase
    {

        [Key]
        public Guid id { get; set; }

        [ForeignKey("propertyDirectory")]
        public Guid propertyDirectoryId { get; set; }
        [MaxLength(500)]
        public string fileName { get; set; }
        [MaxLength(1000)]
        public string filePath { get; set; }

        [MaxLength(20)]
        public string extName { get; set; }

        [MaxLength(1000)]
        public string fileDesc { get; set; }


    }




}
