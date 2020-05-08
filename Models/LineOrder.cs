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
        [Display(Name="Nombre")]
        public string Name {get; set;} //unique  

        [Required]
        [DefaultValueAttribute(0)]
        [Display(Name="Cantidad")]
        public int Quantity {get; set;}

        [Required]
        [DefaultValueAttribute(0)]
        [Display(Name="Precio Unitario")]
        public decimal UnitaryPrice {get; set;}

        [Required]
        [DefaultValueAttribute(0)]
        [Display(Name="Precio Total")]
        public decimal TotalPrice {get; set;}

        //Nav props
        public int? OrderId {get; set;} //FK
        public Order Order {get; set;}
        public int? IntrumentId {get; set;} //FK
        public Instrument Instrument {get; set;}
    }
}