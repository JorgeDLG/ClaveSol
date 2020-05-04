using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClaveSol.Models
{
    public class LineOrder
    {
        
        public int Id {get; set;}

        [Required]
        [DefaultValueAttribute("null")]
        public string Name {get; set;} //unique  

        [Required]
        [DefaultValueAttribute(0)]
        public int Quantity {get; set;}

        [Required]
        [DefaultValueAttribute(0)]
        public decimal UnitaryPrice {get; set;}

        [Required]
        [DefaultValueAttribute(0)]
        public decimal TotalPrice {get; set;}

        //Nav props
        public int? IntrumentId {get; set;} //FK
        public Instrument Instrument {get; set;}
    }
}