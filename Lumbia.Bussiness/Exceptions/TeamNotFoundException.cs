using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbia.Bussiness.Exceptions
{
    public class TeamNotFoundException : Exception
    {
        public string PropertyName { get; set; }
        public TeamNotFoundException( string propertyName,string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}
