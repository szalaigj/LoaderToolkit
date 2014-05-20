using LoaderLibrary.CommandLineParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Verbs
{
    public abstract class BatchLoaderVerb : Verb
    {
        public abstract string Mode
        {
            get;
            set;
        }

        public Type GetMapperType()
        {
            return Type.GetType("BatchLoader.Mappers." + Mode);
        }

        public Type GetMergerType()
        {
            return Type.GetType("BatchLoader.Mergers." + Mode);
        }
    }
}
