using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8,MinimumLength = 3, ErrorMessage = "password should be between 4 and 8 character")]
        public string Password { get; set; }

    }
}
