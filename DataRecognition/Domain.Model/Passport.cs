using System;

namespace Domain.Model
{
    public enum SexType
    {
        Male,
        Female
    }

    public class Passport
    {
        public int Id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public SexType Sex { get; set; }
        public string IssuedBy { get; set; }
        public DateTime IssuedOn { get; set; }
        public string IssuedDepartment { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
    }
}
