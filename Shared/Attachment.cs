using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace terminus.shared.models
{
    public class Attachment
    {
        [MaxLength(36)]
        public string id { get; set; }

        [MaxLength(200)]
        public string displayName { get; set; }
        [MaxLength(500)]
        public string fileName { get; set; }

        [MaxLength(20)]
        public string documentType { get; set; }

        [MaxLength(36)]
        public string refKey { get; set; }

    }
}
