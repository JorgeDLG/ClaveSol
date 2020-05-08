using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace ClaveSol.Models
{
    public class Tiket
    {
        
        public int Id {get; set;}

        [Required]
        [Display(Name="Fecha")]
        public DateTime Date {get; set;} 

        [Required]
        [DefaultValueAttribute("Genérico")]
        [Display(Name="Categoría")]
        public string Category {get; set;} 

        [Required]
        [Display(Name="Título")]
        public string Title {get; set;} 

        [Display(Name="Cuerpo")]
        public string Body {get; set;} 

        //NAV Props
        public int? OperatorId {get; set;} //FK
        public Operator Operator {get; set;}
        public int? OrderId {get; set;} //FK
        public Order Order {get; set;}
    }
}