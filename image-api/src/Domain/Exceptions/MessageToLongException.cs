using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class MessageToLongException : Exception
    {
        public MessageToLongException() : base("Message is too long.")
        {

        }
    }
}
