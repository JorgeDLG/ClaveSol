using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClaveSol.Models
{
    public class Attribut_Ins
    {
            public int Id {get; set;}
            public int AttributId {get; set;}
            public  Attribut Attribut {get; set;}
            public int InstrumentId {get; set;}
            public Instrument Instrument {get; set;}
    }
}