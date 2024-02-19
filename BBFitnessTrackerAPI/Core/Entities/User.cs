using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{3,15}$", ErrorMessage = "Password must be 3-15 characters long and contain only letters and digits.")]
        public string Password { get; set; } = string.Empty;
        public int Age { get; set; }
        public int Weight { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public string ZielSpezifikation { get; set; } = string.Empty; //muskelaufbau ,gewichtsverlust usw. ...

        [Required]
        public double BMI { get; set; }

        public int Kalorienziel { get; set; }
    }
}
   
