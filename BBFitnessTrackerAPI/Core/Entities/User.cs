using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
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

        private string _password = string.Empty;

        //[RegularExpression(@"^[a-zA-Z0-9]{3,15}$", ErrorMessage = "Password must be 3-15 characters long and contain only letters and digits.")]
        public string Password
        {
            get => _password;
            set
            {
                _password = HashPassword(value);
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool VerifyPassword(string password)
        {
            string hashedPassword = HashPassword(password);
            return hashedPassword == _password;
        }

        public int Age { get; set; }
        public int Weight { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public string ZielSpezifikation { get; set; } = string.Empty; //muskelaufbau ,gewichtsverlust, gewichthaltung 

        [Required]
        public double BMI { get; set; }

        public int Height { get; set; }

        public int Kalorienziel { get; set; }
    }
}