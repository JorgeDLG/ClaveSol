using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClaveSol.Models
{
    public class SubCategory
    {
        public int Id {get; set;}

        [Required]
        [Display(Name="Nombre")]
        public string Name {get; set;} //unique  

        //public int? nElements {get; set;} UNECESARY

        //Nav props
        public ICollection<Instrument> Instrument {get; set;}
        public int? CategoryId {get; set;} //FK
        public Category Category {get; set;}
        
    }
}