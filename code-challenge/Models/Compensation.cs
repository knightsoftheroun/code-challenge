using System;

namespace challenge.Models
{
    public class Compensation
    {
        public String CompensationId { get; set; }
        public Employee Employee { get; set; }
        public String Salary { get; set; }
        public String EffectiveDate { get; set; }
    }
}