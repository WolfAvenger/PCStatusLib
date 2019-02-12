using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PCStatusLib
{
    static class DeserializableInfo
    {
        static string DeserializedProcessorActivity()
        {
            string file = @"../../Processor Activity.txt";
            FileStream s1 = new FileStream(file, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            string received = String.Empty;

            using (s1)
            {
                received = (string)formatter.Deserialize(s1);
            }

            return received;
        }
    }
}
