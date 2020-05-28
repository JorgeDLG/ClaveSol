using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClaveSol.Models
{
    public class Shop_Ins
    {
            public int Id {get; set;}
            public int ShopId {get; set;}
            public  Shop Shop {get; set;}
            public int InstrumentId {get; set;}
            public Instrument Instrument {get; set;}
    }
}