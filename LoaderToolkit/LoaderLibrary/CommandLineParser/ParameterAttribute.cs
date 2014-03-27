using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoaderLibrary.CommandLineParser
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ParameterAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
    }
}
