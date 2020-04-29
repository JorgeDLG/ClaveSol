using System;
using System.ComponentModel.DataAnnotations;

namespace ClaveSol.Models
{
    public class Order
    {
        public int Id {get; set;}
        public DateTime Date {get; set;} 
        public int nLines {get; set;} //null
        public decimal Price {get; set;}
        public string State {get; set;}
        public int IdUserFK {get; set;} //FK
    }
}