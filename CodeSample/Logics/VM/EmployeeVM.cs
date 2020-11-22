using System;
using System.ComponentModel.DataAnnotations;

namespace CodeSample.Logics.VM
{
    public class EmployeeVM
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$", ErrorMessage = "Invalid emailId.")]
        [Required(ErrorMessage = "Email id is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile no is required.")]
        public string Mobile { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }

        [Required(ErrorMessage = "Country no is required.")]
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string Logo { get; set; }

        
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastActivityOn { get; set; }
        public int? LastActivityBy { get; set; }


        public string DepartmentTitle { get; set; }
    }
}