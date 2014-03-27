using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoaderLibrary.Load
{
    public static class Constants
    {
        //public const int ReadBufferSize = 0x4000000;   // 64 MB
        //public const int WriteBufferSize = 0x4000000;   // 64 MB

        //public const int ReadBufferSize = 0x100000;   // 1 MB
        //public const int WriteBufferSize = 0x100000;   // 1 MB

        //public const int ReadBufferSize = 0x400000;   // 4 MB
        //public const int WriteBufferSize = 0x400000;   // 4 MB

        public const int ReadBufferSize = 0x10000;   // 64 K
        public const int WriteBufferSize = 0x10000;   // 64 K
    }
}
