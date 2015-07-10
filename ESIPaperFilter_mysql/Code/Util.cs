using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESIPaperFilter.Code
{
    public class Util
    {
        public static readonly string connectionString = System.Configuration.ConfigurationSettings.AppSettings["ESIPaper"].ToString().Trim();
    }
}
