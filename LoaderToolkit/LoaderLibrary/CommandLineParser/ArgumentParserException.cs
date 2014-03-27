using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoaderLibrary.CommandLineParser
{
    public class ArgumentParserException : Exception
    {
        public ArgumentParserException()
            : base()
        {
        }

        public ArgumentParserException(string message)
            : base(message)
        {
        }

        public ArgumentParserException(string message, Exception innerException)
            :base(message, innerException)
        {
        }
    }
}
