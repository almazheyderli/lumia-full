﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbia.Bussiness.Exceptions
{
    public class FileNameNotFoundException : Exception
    {
        public string PropertyName { get; set; }
        public FileNameNotFoundException( string propertyName,string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}
