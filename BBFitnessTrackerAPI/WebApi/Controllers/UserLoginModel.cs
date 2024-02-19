using System.ComponentModel.DataAnnotations;

namespace Users.Web.ApiControllers
{
    public class UserLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}