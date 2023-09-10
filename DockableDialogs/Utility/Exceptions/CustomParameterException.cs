﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockableDialogs.Utility.Exceptions
{
    public class CustomParameterException : ArgumentException
    {
        public CustomParameterException(string customParameter, string nameInstance)
            : base($"{customParameter} isn't existed in the {nameInstance} object") { }
    }
}