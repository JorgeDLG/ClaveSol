using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ClaveSol.Data;
using ClaveSol.Models;
namespace ClaveSol.Models
{
    public class nnModels
    {
       public List<Attribut_Ins> AttrIns {get;set;} 
       public List<List_Instrument> ListIns {get;set;} 
       public List<Shop_Ins> ShopIns {get;set;} 
    }
}