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
        public DateTime Date {get; set;} 

        [Required]
        [DefaultValueAttribute("Genérico")]
        public string Category {get; set;} 

        [Required]
        public string Title {get; set;} 
        public string Body {get; set;} 

        //NAV Props
        public int? OperatorId {get; set;} //FK
        public Operator Operator {get; set;}
        public int? OrderId {get; set;} //FK
        public Order Order {get; set;}
    }
}