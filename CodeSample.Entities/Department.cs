using System;

namespace CodeSample.Entities
{ 
    public class Department
    {
        public int ID { get; set; }

        public string Title { get; set; }
        public string Description { get; set; } 

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastActivityOn { get; set; }
        public int? LastActivityBy { get; set; }
    }
}
