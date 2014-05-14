using LoaderLibrary.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader.Mappers
{
    public class Sread : Mapper<string>
    {
        public override string TableName
        {
            get { return "sreadLoad"; }
        }

        public override void Map(string obj)
        {

        }
    }
}
