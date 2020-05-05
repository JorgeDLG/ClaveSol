using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ClaveSol.Models
{
    public class Shop
    {
        public int Id {get; set;}

        [Required]
        public string City {get; set;} //unique?

        //NAV Props
        public int? ShopId {get; set;}
        public ICollection<Shop_Ins> Shop_Inss {get; set;}
    }
}