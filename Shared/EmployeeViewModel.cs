using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text;

namespace terminus.shared.models
{
    public class EmployeeViewModel
    {

        [MaxLength(36)]
        public string EmployeeId { get; set; }

        public bool Active { get; set; }
        public DateTime HireDate { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string SSS { get; set; }
        public string PhilHealth { get; set; }
        public string PagIbig { get; set; }
        public string TIN { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public DateTime? EndDate { get; set; }
        public string Remarks { get; set; }

        public string attachmentRefKey { get; set; }
        public List<AttachmentViewModel> attachments { get; set; }

    }
}
