using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderLibrary.Streams
{
    public interface ISelector<T>
    {
        T selectObjects(string status);

        string Prefix { get; set; }
    }
}
