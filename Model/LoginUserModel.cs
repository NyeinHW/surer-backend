using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace surer_backend.Model
{
    public class LoginUserModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password{   get; set;}
    }
}
