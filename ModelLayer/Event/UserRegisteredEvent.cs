﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Event
{
    public class UserRegisteredEvent
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
