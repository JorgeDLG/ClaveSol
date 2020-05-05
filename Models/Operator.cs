using System.Collections.Generic;
namespace ClaveSol.Models
{
    public class Operator
    {
        public int Id {get; set;}

        //NAV Props
        public ICollection<Chat> Chats {get; set;}
        public ICollection<Tiket> Tikets {get; set;}
    }
}