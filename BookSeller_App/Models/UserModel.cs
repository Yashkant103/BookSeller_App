using System.ComponentModel.DataAnnotations;

namespace BookSeller_App.Models
{
    public class UserModel
    {
        public int UserID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        [Required]
        public string UserPassword { get; set; }
        [Required]
        public int RoleID { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    public class Role_DropDown
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }

}