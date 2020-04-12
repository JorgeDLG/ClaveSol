using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaveSol.Models
{
    public class User
    {
        public int Id {get; set;}

        [StringLength(30,MinimumLength = 2)]
        [Required]
        public string Name {get; set;}

        [StringLength(60,MinimumLength = 2)]
        [Required]
        public string Surname {get; set;}

        [EmailAddress]
        [Required]
        public string Mail {get; set;}

        [Required]
        public bool Premium {get; set;}
        
    }
}
