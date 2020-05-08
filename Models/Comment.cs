using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClaveSol.Models
{
    public class Comment
    {
        
        public int Id {get; set;}

        [Required]
        [Display(Name="Fecha")]
        public DateTime Date {get; set;} 

        [Required]
        [Display(Name="Título")]
        public string Title {get; set;}

        [Required]
        [Display(Name="Cuerpo")]
        public string Body {get; set;}

        [Required]
        [DefaultValueAttribute(0)]
        [Display(Name="Estrellas")]
        public decimal Stars {get; set;}

        [Required]
        [DefaultValueAttribute(false)]
        [Display(Name="Eliminado")]
        public bool Deleted {get; set;}

        //NAVIGATION PROPS
        public int? UserId {get; set;}
        public User User {get; set;}
        public int? InstrumentId {get; set;}
        public Instrument Instrument {get; set;}
    }
}