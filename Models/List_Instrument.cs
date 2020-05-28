using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClaveSol.Models
{
    public class List_Instrument
    {
            public int Id {get; set;}
            public int ListId {get; set;}
            public List List {get; set;}
            public int InstrumentId {get; set;}
            public Instrument Instrument {get; set;}
    }
}