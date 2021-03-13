using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Base
{
    public abstract class ErrorMessageBase
    {
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}
