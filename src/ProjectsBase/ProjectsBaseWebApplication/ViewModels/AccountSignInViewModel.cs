using System.ComponentModel.DataAnnotations;

namespace ProjectsBaseWebApplication.ViewModels
{
    public class AccountSignInViewModel : IAccountViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}