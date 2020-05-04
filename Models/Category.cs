using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClaveSol.Models
{
    public class Category
    {
        public int Id {get; set;}
        [Required]
        public string Name {get; set;} //unique
        public int? nElements {get; set;}

        //Nav props
        public ICollection<SubCategory> SubCategory {get; set;}
    }
}