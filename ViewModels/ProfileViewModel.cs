using System.ComponentModel.DataAnnotations;

namespace UserRoles.ViewModels
{
    public class ProfileViewModel
    {
        [Required]
        [Display(Name = "Name")]
        [StringLength(20, ErrorMessage = "Name cannot exceed 20 characters.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name must contain only letters.")]
        public string FirstName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }   // Read-only

        [Display(Name = "Mobile Number")]
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public string MobileNumber { get; set; }

        // Controls View / Edit mode
        public bool IsEditMode { get; set; }
    }
}
