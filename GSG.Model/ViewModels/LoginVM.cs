using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Model.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
