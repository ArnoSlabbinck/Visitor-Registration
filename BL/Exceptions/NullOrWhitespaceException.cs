﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class NullOrWhitespaceException : Exception
    {
        public NullOrWhitespaceException(string message) : base(message)
        {
        }
    }
}
