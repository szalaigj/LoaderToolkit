using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderLibrary.Streams
{
    public class DefaultStreamReaderForLoader : CommonStreamReaderForLoader<string>
    {
        public DefaultStreamReaderForLoader(ISelector<string> selector)
            : base(selector)
        {
        }
    }
}
