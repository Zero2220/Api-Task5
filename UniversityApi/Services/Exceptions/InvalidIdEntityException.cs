﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exceptions
{
    public class InvalidIdEntityException :Exception
    {
        public InvalidIdEntityException(string msg) : base(msg) { }
        public InvalidIdEntityException()
        {

        }
    }
}
