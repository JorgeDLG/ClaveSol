using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaveSol.Models
{
    public class User
    {
        public int Id {get; set;}

        [StringLength(30,MinimumLength = 2)]
        [Required]
        public string Name {get; set;}

        [StringLength(60,MinimumLength = 2)]
        [Required]
        public string Surname {get; set;}

        [EmailAddress]
        [Required]
        public string Mail {get; set;}

        [Required]
        public bool Premium {get; set;}


        //NAVIGATION PROPS
        public ICollection<Order> Orders {get; set;}
        public ICollection<List> Lists {get; set;}
        public ICollection<Comment> Comments {get; set;}
        public ICollection<Chat> Chats {get; set;}





        /////// Identity/Auth atributes: //////
            //User ID for AspNetUser table
        public string OwnerID {get; set;} 

            //If general users can see it
        public UserStatus Status {get; set;} 
    }
    public enum UserStatus
    {
        Submitted,
        Approved,
        Rejected
    }
}
