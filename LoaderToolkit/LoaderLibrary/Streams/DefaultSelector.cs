using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderLibrary.Streams
{
    public class DefaultSelector : ISelector<string>
    {
        private const string delimiter = "\t";
        private string prefix = null;

        public string selectObjects(string status)
        {
            string retVal = status;
            if (prefix != null)
            {
                retVal = prefix + delimiter + status;
            }
            return retVal;
        }

        public string Prefix
        {
            get { return prefix; }
            set { prefix = value; }
        }
    }
}
