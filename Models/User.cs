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
        public String username { get; set; }
        [Required]
        public String password { get; set; }
        public Int64 puzzle_a { get; set; }
        public Int64 puzzle_t { get; set; }
        public Int64 puzzle_n { get; set; }
        public Int64 puzzle_key { get; set; }
    }
}