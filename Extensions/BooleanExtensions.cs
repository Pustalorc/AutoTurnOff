using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pustalorc.Plugins.ReduceLag
{
    public static class BooleanExtensions
    {
        public static unsafe byte ToByte(this bool source)
            => *((byte*)(&source));
    }
}
