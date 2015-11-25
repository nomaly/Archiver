using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    public static class CApplicationUtils
    {
        public static byte[] ConvertStringToBytes(String str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static Int64 GetSizeOfPackageHeader()
        {
            return SerializeToBytes(new CPackageHeaderInfo()).LongLength;
        }

        public static byte[] SerializeToBytes(Object graph)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, graph);
                return ms.ToArray();
            }
        }
    }
}
