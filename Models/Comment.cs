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
        public DateTime Date {get; set;} 

        [Required]
        public string Title {get; set;}

        [Required]
        public string Body {get; set;}

        [Required]
        [DefaultValueAttribute(0)]
        public decimal Stars {get; set;}

        [Required]
        [DefaultValueAttribute(false)]
        public bool Deleted {get; set;}

        //NAVIGATION PROPS
        public int? UserId {get; set;}
        public User User {get; set;}
        public int? InstrumentId {get; set;}
        public Instrument Instrument {get; set;}
    }
}