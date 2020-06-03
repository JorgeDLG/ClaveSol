using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClaveSol.Models
{
    public class Instrument
    {
        public int Id {get; set;}

        [Required]
        [Display(Name="Nombre")]
        public string Name {get; set;} //unique  

        [Required]
        [Display(Name="Marca")]
        public string Brand {get; set;} //unique  

        [Required]
        [DefaultValueAttribute(0)]
        [Display(Name="Precio")]
        public decimal Price {get; set;}
        
        [Required]
        [DefaultValueAttribute("Disponible")]
        [Display(Name="Estado")]
        public string State {get; set;} //% enum?

        [Display(Name="Descripción")]
        public string Description {get; set;} 

        [Display(Name="Directorio Multimedia")]
        public string MediaDir {get; set;} //directory for vids and pics 

        //NAV Props
        public int? SubCategoryId {get; set;} //FK
        public SubCategory SubCategory {get; set;}
        // public int? LineOrderId {get; set;} //FK
        // public LineOrder LineOrder {get; set;}
        public ICollection<LineOrder> LineOrders {get; set;}
        public ICollection<Comment> Comments {get; set;}

        //% N-N relations (Materials and Colors too) 

        public int? InstrumentId {get; set;}
        public ICollection<List_Instrument> List_Instruments {get; set;}

        //---------------------------
        public int? shInsId {get; set;}
        public ICollection<Shop_Ins> Shop_Inss {get; set;}

        //---------------------------
        public int? attrInsId {get; set;}
        public ICollection<Attribut_Ins> Attribut_Inss {get; set;}

    }
}