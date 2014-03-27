using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoaderLibrary.CommandLineParser
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionAttribute : ParameterAttribute
    {
    }
}
