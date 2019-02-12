using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Threading;
using System.Diagnostics;
using PCStatusLib;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualBasic.Devices;

namespace SandBoxConsole
{
    enum Charge
    {
        B, KB, MB, GB, TB
    }
	class Program
	{

        static double sec = 0;
        static void ProcessorsOverloadTracing()
        {
            PerformanceCounter pc = new PerformanceCounter("Процессор", "% загруженности процессора", "_Total");

            int num = PCStatus.ProcessorsInfo()[0].CoresNumber;
            List<PerformanceCounter> list = new List<PerformanceCounter>();
            list.Add(pc);

            for (int i = 0; i < num; i++)
            {
                list.Add(new PerformanceCounter("Процессор", "% загруженности процессора", i.ToString()));
            }
            while (true) {
                Console.Clear();
                Console.WriteLine($"Процессор загружен на: {pc.NextValue()}%\r\n");
                Console.WriteLine($"Ядро 0 загружено на {list[0].NextValue()}%");
                Console.WriteLine($"Ядро 1 загружено на {list[1].NextValue()}%");
                Console.WriteLine($"Ядро 2 загружено на {list[2].NextValue()}%");
                Console.WriteLine($"Ядро 3 загружено на {list[3].NextValue()}%");
                Thread.Sleep(1000);
            }

        }
        static void NewMain()
        {
            #region Paths of files of testing
            string path1 = @"../../Active Processes.txt";
            string path2 = @"../../Installed Soft.txt";
            string path3 = @"../../System Info.txt";
            string path4 = @"../../Disks Info.txt";
            string path5 = @"../../Video Controllers Info.txt";
            string path6 = @"../../Processors Info.txt";
            string path7 = @"../../Physical Memory.txt";
            string path8 = @"../../Virtual Memory.txt";
            string path9 = @"../../Processor Activity.txt"; 
            #endregion

            TimerCallback call = new TimerCallback(Tick);
            Timer timer = new Timer(call, sec, 0, 100);
            //ActiveProcesses(path1);
            //InstalledSoft(path2);
            //SystemInfo(path3);
            //DisksInfo(path4);
            //VideoConrollersInfo(path5);
            //ProcessorsInfo(path6);
            //AvailibleMemory();
            //ProcessorActivity(path9);
            Console.WriteLine("FINISHED");
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            Console.Read();
        }

        static void SerializingData()
        {
            ThreadStart s1 = new ThreadStart(SerializedActiveProcesses);
            s1 += SerializedSystemInfo; s1 += SerializedAvailibleMemory; s1 += SerializedProcessorsInfo;
            ThreadStart s2 = new ThreadStart(SerializedDisksInfo);
            s2 += SerializedProcessorActivity; s2 += SerializedVideoConrollersInfo; s2 += SerializedInstalledSoft;

            Thread[] t_s = new Thread[] { new Thread(s1), new Thread(s2) };
            TimerCallback call = new TimerCallback(Tick);
            Timer timer = new Timer(call, sec, 0, 100);
            for (int i = 0; i < t_s.Length; i++)
            {
                t_s[i].Start();
            }
            while (true)
            {
                if (!t_s[1].IsAlive)
                {
                    timer.Change(Timeout.Infinite, Timeout.Infinite);
                    break;
                }

            }
            Console.WriteLine("All Tasks done");
            Console.Read();
        }

        static void ThreadMain()
        {

            ThreadStart s1 = new ThreadStart(ActiveProcesses);
            s1 += SystemInfo; s1 += AvailibleMemory; s1 += ProcessorsInfo;
            ThreadStart s2 = new ThreadStart(DisksInfo);
            s2 += ProcessorActivity; s2 += VideoConrollersInfo; s2 += InstalledSoft;

            Thread[] t_s = new Thread[] { new Thread(s1), new Thread(s2) };
            TimerCallback call = new TimerCallback(Tick);
            Timer timer = new Timer(call, sec, 0, 100);
            for (int i=0; i<t_s.Length; i++)
            {
                t_s[i].Start();
            }
            while (true)
            {
                if (!t_s[1].IsAlive)
                {
                    timer.Change(Timeout.Infinite, Timeout.Infinite);
                    break;
                }
                
            }
            Console.WriteLine("All Tasks done");
            Console.Read();
        }

        static void Main(string[] args)
        {
            //NewMain();
            //ProcessorsOverloadTracing();
            //ThreadMain();
            SerializingData();
            Console.WriteLine(DeserializedProcessorActivity());
        }

        #region TestTasks
        static void AvailibleMemory()
        {
            string path7 = @"../../Physical Memory.txt";
            string path8 = @"../../Virtual Memory.txt";
            Charge el = Charge.B;
            StreamWriter writer = new StreamWriter(new FileStream(path7, FileMode.Create));
            StreamWriter writer1 = new StreamWriter(new FileStream(path8, FileMode.Create));
            writer.WriteLine(PCStatus.AvailablePhysicalMemory(3) + " " + (el + 3).ToString());
            writer1.WriteLine(PCStatus.AvailibleVirtualMemory(3) + " " + (el + 3).ToString());
            writer.Close(); writer1.Close();
            Console.WriteLine("AvailibleMemory() done at {0} sec", Math.Round(sec,1));
        }

        static void ActiveProcesses(/*string file*/)
        {
            int count = 0;
            string file = @"../../Active Processes.txt";
            List<string> list = PCStatus.ActiveProcesses();
            StreamWriter writer = new StreamWriter(new FileStream(file, FileMode.Create));
            foreach (string elem in list)
            {
                writer.WriteLine(elem);
                count++;
            }
            writer.Write($"КОличество активных процессов: {count}");
            writer.Close();
            Console.WriteLine($"ActiveProcesses() done at {Math.Round(sec,1)} sec");
        }

        static void InstalledSoft(/*string file*/)
        {
            string file = @"../../Installed Soft.txt";
            int count = 0;
            StreamWriter writer = new StreamWriter(new FileStream(file, FileMode.Create));
            ProgramSoft[] arr = PCStatus.InstalledSoft().ToArray<ProgramSoft>();
            Array.Sort<ProgramSoft>(arr, (a, b) => a.Softname.CompareTo(b.Softname));
            foreach (var elem in arr)
            {
                writer.WriteLine(elem.GetFields());
                count++;
            }
            writer.Write($"Количество установленного ПО: {count}");
            writer.Close();
            Console.WriteLine($"InstalledSoft() done at {Math.Round(sec, 1)} sec");
        }

        static void SystemInfo(/*string file*/)
        {
            string file = @"../../System Info.txt";
            StreamWriter writer = new StreamWriter(new FileStream(file, FileMode.Create));
            writer.WriteLine(PCStatus.SystemInfo().GetFields());
            writer.Close();
            Console.WriteLine($"SystemInfo() done at {Math.Round(sec, 1)} sec");
        }

        static void DisksInfo(/*string file*/)
        {
            string file = @"../../Disks Info.txt";
            StreamWriter writer = new StreamWriter(new FileStream(file, FileMode.Create));
            foreach (var elem in PCStatus.DisksInfo())
            {
                writer.WriteLine(elem.GetFields());
                writer.WriteLine("----------------");
            }
            writer.Close();
            Console.WriteLine($"DisksInfo() done at {Math.Round(sec, 1)} sec");
        }

        static void VideoConrollersInfo(/*string file*/)
        {
            string file = @"../../Video Controllers Info.txt";
            StreamWriter writer = new StreamWriter(new FileStream(file, FileMode.Create));
            foreach (var elem in PCStatus.VideoControllersInfo())
            {
                writer.WriteLine(elem.GetFields());
                writer.WriteLine("---------------");
            }
            writer.Close();
            Console.WriteLine($"VideoConrollersInfo() done at {Math.Round(sec, 1)} sec");
        }

        static void ProcessorsInfo(/*string file*/)
        {
            string file = @"../../Processors Info.txt";
            StreamWriter writer = new StreamWriter(new FileStream(file, FileMode.Create));
            foreach (var elem in PCStatus.ProcessorsInfo())
            {
                writer.WriteLine(elem.GetFields());
                writer.WriteLine("---------------");
            }
            writer.Close();
            Console.WriteLine($"ProcessorsInfo() done at {Math.Round(sec, 1)} sec");
        }

        static void ProcessorActivity(/*string file*/)
        {
            string file = @"../../Processor Activity.txt";
            StreamWriter writer = new StreamWriter(new FileStream(file, FileMode.Create));

            writer.WriteLine(PCStatus.CPULoad());
            writer.Write("---------------------");
            writer.Close();
            Console.WriteLine($"ProcessorActivity() done at {Math.Round(sec, 1)} sec");
        }

        public static void Tick(object time)
        {
            sec+=0.1;
        }

        #endregion

        #region Serialized TestTasks
        static void SerializedAvailibleMemory()
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

        static void SerializedActiveProcesses()
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

        static void SerializedInstalledSoft()
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

        static void SerializedSystemInfo()
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

        static void SerializedDisksInfo()
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

        static void SerializedVideoConrollersInfo()
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

        static void SerializedProcessorsInfo()
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

        static void SerializedProcessorActivity()
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

        #endregion

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
