using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClaveSol.Models
{
    public class List
    {
        public int Id {get; set;}

        [Required]
        public string Name {get; set;}

        [Required]
        [DefaultValueAttribute(false)]
        public bool Deleted {get; set;}

        //NAVIGATION PROPS
        public int? UserId {get; set;}
        public User User {get; set;}

            //N-N
        public int? ListId {get; set;}
        public ICollection<List_Instrument> List_Instruments {get; set;}
    }
}