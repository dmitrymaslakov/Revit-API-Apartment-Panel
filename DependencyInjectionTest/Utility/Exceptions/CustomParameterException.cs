﻿using System;

namespace DependencyInjectionTest.Utility.Exceptions
{
    public class CustomParameterException : ArgumentException
    {
        public CustomParameterException(string customParameter, string nameInstance)
            : base($"{customParameter} isn't existed in the {nameInstance} object") { }
    }
}
