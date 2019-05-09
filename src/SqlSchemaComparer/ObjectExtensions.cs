using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchemaComparer
{
    internal static class ObjectExtensions
    {
        public static void SetRandom(this long longValue)
        {
            longValue = Utils.GetRandomLong();
        }
    }
}
