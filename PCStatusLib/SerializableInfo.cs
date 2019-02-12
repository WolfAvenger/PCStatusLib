using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace PCStatusLib
{

    enum Charge
    {
        B, KB, MB, GB, TB
    }

    public static class SerializableInfo
    {

        static double sec = 0;

        public static void SerializedAvailibleMemory()
        {
            string path7 = @"../../Physical Memory.txt";
            string path8 = @"../../Virtual Memory.txt";
            Charge el = Charge.B;
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream s1 = new FileStream(path7, FileMode.Create))
            {
                formatter.Serialize(s1, PCStatus.AvailablePhysicalMemory(3) + " " + (el + 3).ToString());
            }
            using (FileStream s2 = new FileStream(path8, FileMode.Create))
            {
                formatter.Serialize(s2, PCStatus.AvailibleVirtualMemory(3) + " " + (el + 3).ToString());
            }

            Console.WriteLine("SerializedAvailibleMemory() done at {0} sec", Math.Round(sec, 1));
        }

        public static void SerializedActiveProcesses()
        {
            string file = @"../../Active Processes.txt";
            List<string> list = PCStatus.ActiveProcesses();
            FileStream s1 = new FileStream(file, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            using (s1)
            {
                formatter.Serialize(s1, list);
            }
            Console.WriteLine($"SerializedActiveProcesses() done at {Math.Round(sec, 1)} sec");
        }

        public static void SerializedInstalledSoft()
        {
            string file = @"../../Installed Soft.txt";
            FileStream s1 = new FileStream(file, FileMode.Create);
            ProgramSoft[] arr = PCStatus.InstalledSoft().ToArray<ProgramSoft>();
            Array.Sort<ProgramSoft>(arr, (a, b) => a.Softname.CompareTo(b.Softname));
            List<ProgramSoft> list = new List<ProgramSoft>(arr);
            BinaryFormatter formatter = new BinaryFormatter();
            using (s1)
            {
                formatter.Serialize(s1, list);
            }
            Console.WriteLine($"SerializedInstalledSoft() done at {Math.Round(sec, 1)} sec");
        }

        public static void SerializedSystemInfo()
        {
            string file = @"../../System Info.txt";
            FileStream s1 = new FileStream(file, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            using (s1)
            {
                formatter.Serialize(s1, PCStatus.SystemInfo());
            }

            Console.WriteLine($"SerializedSystemInfo() done at {Math.Round(sec, 1)} sec");
        }

        public static void SerializedDisksInfo()
        {
            string file = @"../../Disks Info.txt";
            FileStream s1 = new FileStream(file, FileMode.Create);
            List<DiskInfo> list = PCStatus.DisksInfo();
            BinaryFormatter formatter = new BinaryFormatter();

            using (s1)
            {
                formatter.Serialize(s1, list);
            }
            Console.WriteLine($"SerializedDisksInfo() done at {Math.Round(sec, 1)} sec");
        }

        public static void SerializedVideoConrollersInfo()
        {
            string file = @"../../Video Controllers Info.txt";
            FileStream s1 = new FileStream(file, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            List<VideoControllerInfo> list = PCStatus.VideoControllersInfo();

            using (s1)
            {
                formatter.Serialize(s1, list);
            }

            Console.WriteLine($"SerializedVideoConrollersInfo() done at {Math.Round(sec, 1)} sec");
        }

        public static void SerializedProcessorsInfo()
        {
            string file = @"../../Processors Info.txt";
            FileStream s1 = new FileStream(file, FileMode.Create);
            List<ProcessorInfo> list = PCStatus.ProcessorsInfo();
            BinaryFormatter formatter = new BinaryFormatter();

            using (s1)
            {
                formatter.Serialize(s1, list);
            }

            Console.WriteLine($"Serialized" +
                $"ProcessorsInfo() done at {Math.Round(sec, 1)} sec");
        }

        public static void SerializedProcessorActivity()
        {
            string file = @"../../Processor Activity.txt";
            FileStream s1 = new FileStream(file, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            using (s1)
            {
                formatter.Serialize(s1, PCStatus.CPULoad());
            }
            Console.WriteLine($"SerializedProcessorActivity() done at {Math.Round(sec, 1)} sec");
        }
    }
}
