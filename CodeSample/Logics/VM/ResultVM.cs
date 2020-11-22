using System;

namespace CodeSample.Logics.VM
{
    public class ResultVM
    {
        public String Message { get; set; }
        public dynamic Data { get; set; }
        public dynamic Errors { get; set; }
        public string Status { get; set; }
        public Boolean IsSuccess { get; set; }
         
    }
}
