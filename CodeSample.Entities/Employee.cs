using System;

namespace CodeSample.Entities
{ 
    public class Employee
    {
        public int ID { get; set; }
        public int DepartmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
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

        public virtual Department Department { get; set; }
    }
}
