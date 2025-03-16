using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model
{
    public class UserRegistrationDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }  // Plain text password to be hashed
        public string Email { get; set; }
    }
}
