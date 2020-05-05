using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClaveSol.Models
{
    public class Attribute
    {
        public int Id {get; set;}

        [Required]
        public string Type {get; set;}

        [Required]
        public string Value {get; set;}

        //NAV Props
        public int? AttributeId {get; set;}
        public ICollection<Attribute_Ins> Attribute_Inss {get; set;}
    }
}