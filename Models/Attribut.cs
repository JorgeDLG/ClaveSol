using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClaveSol.Models
{
    public class Attribut
    {
        public int Id {get; set;}

        [Required]
        [Display(Name="Tipo")]
        public string Type {get; set;}

        [Required]
        [Display(Name="Valor")]
        public string Value {get; set;}

        //NAV Props
        public int? AttributId {get; set;}
        public ICollection<Attribut_Ins> Attribut_Inss {get; set;}
    }
}