using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace terminus.shared.models
{
    public class DocumentIdTable
    {
        [Key, MaxLength(200)]
        public string IdKey { get; set; }

        [MaxLength(200)]
        public string Format { get; set; }

        public int NextId { get; set; }

        [ForeignKey("company")]
        [MaxLength(10)]
        public string CompanyId { get; set; }

        public Company company { get; set; }
    }
}
