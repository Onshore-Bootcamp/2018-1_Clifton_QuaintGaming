using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuaintGaming.Models
{
    public class UserPO
    {
        public UserPO()
        {
            RoleID = 3;
        }

        public int UserID { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 25 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "Password must be between 5 and 25 characters")]
        public string Password { get; set; }

        [StringLength(20, ErrorMessage = "First name must not exceed 20 characters")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [StringLength(20, ErrorMessage = "Last name must not exceed 20 characters")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        //Regular Expression puts specific parameters on what will be accepted by the user for an email.
        [Required(ErrorMessage = "An email address is required")]
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}", ErrorMessage = "Email provided was not a valid email address")]
        [StringLength(50)]
        public string Email { get; set; }

        public int RoleID { get; set; }

        public string Title { get; set; }
    }
}