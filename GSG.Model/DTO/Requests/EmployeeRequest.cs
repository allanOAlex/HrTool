using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Model.DTO.Requests
{
    public class EmployeeRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PictureUrl { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string EmployeeState { get; set; }
    }
}
