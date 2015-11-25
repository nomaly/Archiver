using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    public static class CArguments
    {
        public static void ThrowIfArgumentNull<T>(T arg, String name) where T : class
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("name");

            if (arg == null)
                throw new ArgumentNullException(name);
        }

        public static void ThrowIfNot(bool condition, String message)
        {
            if (!condition)
                throw new ArgumentException(message);
        }
    }
}
