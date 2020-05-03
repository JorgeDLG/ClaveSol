using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace ClaveSol.Models
{
    public class Order
    {
        public int Id {get; set;}

        [Required]
        public DateTime Date {get; set;} 
        public int nLines {get; set;} //null

        [Required]
        [DefaultValueAttribute(0)]
        public decimal Price {get; set;}

        [Required]
        [DefaultValueAttribute("Procesando")]
        public string State {get; set;} //% enum?

        //NAV Props
        public int? UserId {get; set;} //FK
        public User User {get; set;}
    }
}