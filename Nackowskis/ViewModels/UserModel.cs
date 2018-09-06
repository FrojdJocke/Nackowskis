using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nackowskis.ViewModels
{
    public class UserModel
    {
        public UserModel()
        {
            Unique = true;
        }
        [Required(ErrorMessage = "Please register an Username")]
        [MinLength(3, ErrorMessage = "At least 3 characters")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only letters and numbers")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter a password")]
        [MinLength(4, ErrorMessage = "At least 4 characters")]
        [MaxLength(20, ErrorMessage = "Max length 20 characters")]
        public string Password { get; set; }

        public bool Unique { get; set; }
    }
}
