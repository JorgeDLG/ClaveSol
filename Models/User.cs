using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaveSol.Models
{
    [Table("User")]
    public class User
    {
        [Column("UserID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "User ID is required")]
        [Display(Name = "User ID")]
        public int Id { get; set; }
    }
}