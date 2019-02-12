using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PCStatusLib
{
    static class DeserializableInfo
    {
        /// <summary>
        /// Method, which deserializes information about processor and core's activity
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Method, which deserializes information about processor's factory information
        /// </summary>
        /// <returns></returns>
        static List<ProcessorInfo> DeserializedProcessorsInfo()
        {
            string file = @"../../Processors Info.txt";
            FileStream s1 = new FileStream(file, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            List<ProcessorInfo> received = new List<ProcessorInfo>();

            using (s1)
            {
                received = (List<ProcessorInfo>)formatter.Deserialize(s1);
            }

            return received;
        }

        /// <summary>
        /// Method, which deserializes information about video controllers
        /// </summary>
        /// <returns></returns>
        static List<VideoControllerInfo> DeserializedVideoControllersInfo()
        {
            string file = @"../../Video Controllers Info.txt";
            FileStream s1 = new FileStream(file, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            List<VideoControllerInfo> received = new List<VideoControllerInfo>();

            using (s1)
            {
                received = (List<VideoControllerInfo>)formatter.Deserialize(s1);
            }

            return received;
        }

        /// <summary>
        /// Method, which deserializes information about disks
        /// </summary>
        /// <returns></returns>
        static List<DiskInfo> DeserializedDisksInfo()
        {
            string file = @"../../Disks Info.txt";
            FileStream s1 = new FileStream(file, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            List<DiskInfo> received = new List<DiskInfo>();

            using (s1)
            {
                received = (List<DiskInfo>)formatter.Deserialize(s1);
            }

            return received;
        }
    }
}
