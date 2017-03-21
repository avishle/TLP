using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TLP.Models
{
    public class User
    {
        [Key]
        [Required]
        public String userName { get; set; }
        [Required]
        public String password { get; set; }
    }
}