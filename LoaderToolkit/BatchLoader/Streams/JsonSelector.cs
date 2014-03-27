using LoaderLibrary.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Streams
{
    class JsonSelector : ISelector<Dictionary<string, object>>
    {
        /// <summary>
        /// Select objects to dictionary. It can be used for tweeter loading.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public Dictionary<string, object> selectObjects(string status)
        {
            try
            {
                return (Dictionary<string, object>)fastJSON.JSON.Instance.Parse(status);
            }
            catch (Exception e)
            {
                // This is a possible parsing error from a partial message
                Console.Error.WriteLine("JSON parser error: {0}", e.Message);
                return null;
            }
        }

        /// <summary>
        /// This method is unused in this class
        /// </summary>
        public string Prefix
        {
            get { return null; }
            set { /* do nothing */; }
        }
    }
}
