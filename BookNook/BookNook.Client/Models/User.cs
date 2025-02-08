using System.ComponentModel.DataAnnotations;

namespace BookNook.Client.Models
{
    public class User
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? MembershipLevelName { get; set; }
        public decimal DiscountRate { get; set; }
        [Required(ErrorMessage = "Old password is required.")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "New password is required.")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare(nameof(NewPassword), ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
