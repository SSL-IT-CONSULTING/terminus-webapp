using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace terminus.shared.models
{
    public class Employee:TBase
    {
        [MaxLength(36)]
        public string EmployeeId { get; set; }

        [MaxLength(10), ForeignKey("company")]
        public string CompanyId { get; set; }
        public Company company { get; set; }


        public bool Active { get; set; }
        public DateTime HireDate { get; set; }

        [MaxLength(70)]
        public string LastName { get; set; }

        [MaxLength(70)]
        public string FirstName { get; set; }

        [MaxLength(70)]
        public string MiddleName { get; set; }

        [MaxLength(50)]
        public string Position { get; set; }

        [MaxLength(708)]
        public string Address { get; set; }

        [MaxLength(20)]
        public string ContactNo { get; set; }

        [MaxLength(500)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string SSS { get; set; }
        [MaxLength(50)]
        public string PhilHealth { get; set; }
        [MaxLength(50)]
        public string PagIbig { get; set; }
        [MaxLength(50)]
        public string TIN { get; set; }
        public DateTime? BirthDate { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; }
        public DateTime? EndDate { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }
       
    }
}
