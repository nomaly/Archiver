using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    public static class CConst
    {
        private const string s_packageSign = "2676F2E2-A732-439D-AA93-E49A2C68DD3F"; 
        public static Guid PackageSign => new Guid(s_packageSign);
    }
}
