using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace ClaveSol.Models
{
    public class Order
    {
        public int Id {get; set;}

        [Required]
        [Display(Name="Fecha")]
        public DateTime Date {get; set;} 

        [Display(Name="Nº de Líneas")]
        public int nLines {get; set;} //null

        [Required]
        [DefaultValueAttribute(0)]
        [Display(Name="Precio")]
        public decimal Price {get; set;}

        [Required]
        [DefaultValueAttribute("Procesando")]
        [Display(Name="Estado")]
        public string State {get; set;} //% enum?

        //NAV Props
        public int? UserId {get; set;} //FK
        public User User {get; set;}

        public ICollection<LineOrder> LineOrders {get; set;}
        public ICollection<Tiket> Tikets {get; set;}
    }
}