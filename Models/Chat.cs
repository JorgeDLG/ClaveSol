using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace ClaveSol.Models
{
    public class Chat
    {
        public int Id {get; set;}

        [Required]
        public DateTime StartDate {get; set;} 

        [Required]
        public DateTime EndDate {get; set;} 

        [Required]
        public string FileName {get; set;} 
            //% Userfolder/StartDate-HH:MM(EndDate).json

        [Required]
        [DefaultValueAttribute(false)]
        public bool Resolved {get; set;}

        //NAV Props
        public int? UserId {get; set;} //FK
        public User User {get; set;}
        public int? OperatorId {get; set;} //FK
        public Operator Operator {get; set;}
    }
}