using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BatchLoader
{
    static class AppSettings
    {
        public static string AdminConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["szalaigj"].ConnectionString; }
        }
    }
}
