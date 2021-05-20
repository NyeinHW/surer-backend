using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace surer_backend.Model
{
    public class User
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password
        {
            get; set;
        }
        public int ContactNumber { get; set; }
    }
}
