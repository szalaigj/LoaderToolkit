using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace FileAdapter
{
    static class AppSettings
    {
        public static string AdminConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["FileAdapter.Admin"].ConnectionString; }
        }
    }
}
