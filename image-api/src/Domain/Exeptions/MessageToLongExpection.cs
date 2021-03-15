using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exeptions
{
    public class MessageToLongExpection : Exception
    {
        public MessageToLongExpection() : base("Message is too long.")
        {

        }
    }
}
