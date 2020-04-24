using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaveSol.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "User ID is required")]
        [Display(Name = "User ID")]
        public int Id { get; set; }

        [StringLength(30,MinimumLength = 2)]
        [Required(ErrorMessage = "Name is required")]
        public string Name {get; set;}

        [StringLength(60,MinimumLength = 2)]
        [Required(ErrorMessage = "Surname is required")]
        public string Surname {get; set;}

        [EmailAddress]
        [Required]
        public string Mail {get; set;}

        [Required]
        public bool Premium {get; set;}

        //user ID from AspNetUser table (Identity DB)
        public string OwnerID {get; set;}
    }
}