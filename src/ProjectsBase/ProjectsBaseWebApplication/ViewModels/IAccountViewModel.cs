using System.ComponentModel.DataAnnotations;

namespace ProjectsBaseWebApplication.ViewModels
{
    public interface IAccountViewModel
    {
        [Required, EmailAddress]
        string Email { get; set; }
        [Required, StringLength(100, MinimumLength = 6, ErrorMessage = "The {0} must be at least {2} characters long.")]
        string Password { get; set; }
    }
}