using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClaveSol.Models
{
    public class Instrument
    {
        public int Id {get; set;}

        [Required]
        public string Name {get; set;} //unique  
        [Required]
        public string Brand {get; set;} //unique  

        [Required]
        [DefaultValueAttribute(0)]
        public decimal Price {get; set;}
        
        [Required]
        [DefaultValueAttribute("Disponible")]
        public string State {get; set;} //% enum?
        public string[] Materials {get; set;}
        public string[] Colors {get; set;}
        public string Description {get; set;} 
        public string MediaDir {get; set;} //directory for vids and pics 

        //NAV Props
        public int? SubCategoryId {get; set;} //FK
        public SubCategory SubCategory {get; set;}
        public int? LineOrderId {get; set;} //FK
        public LineOrder LineOrder {get; set;}
    }
}