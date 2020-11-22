namespace CodeSample.Entities
{
    using System;
    using System.Collections.Generic;
    
    public class CountryState
    {
        public int ID { get; set; }
        public int CountryID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
