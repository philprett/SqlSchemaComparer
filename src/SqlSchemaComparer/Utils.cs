using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchemaComparer
{
    internal class Utils
    {
        public static long GetRandomLong()
        {
            long ret = 0;
            var random = new Random();
            ret = (long)random.Next() << 32;
            ret = ret | (long)random.Next();
            return ret;
        }

    }
}
