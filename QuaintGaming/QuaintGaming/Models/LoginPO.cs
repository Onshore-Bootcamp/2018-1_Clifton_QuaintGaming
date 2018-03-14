using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuaintGaming.Models
{
    public class LoginPO
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(25)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(25)]
        public string Password { get; set; }
    }
}