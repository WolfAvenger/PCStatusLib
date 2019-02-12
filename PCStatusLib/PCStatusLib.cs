using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace PCStatusLib
{
    /// <summary>
    /// Статический класс, содержащий методы для вывода различной информации о состоянии ПК.
    /// </summary>
    public static class PCStatus
    {
		/// <summary>
		/// Метод, возвращающий список названий активных процессов ПК, отсортированный в алфавитном порядке.
		/// </summary>
		/// <returns>Список названий активных процессов ПК</returns>
		public static List<string> ActiveProcesses()
		{
			List<string> processes = new List<string>();
			ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2",
											"Select Name, CommandLine From Win32_Process");

			foreach (ManagementObject instance in searcher.Get())
			{
				processes.Add(instance["Name"].ToString());
			}
            string[] proc = new string[processes.Count]; 
            for (int i=0; i<processes.Count; i++)
            {
                proc[i] = processes[i];
            }
            Array.Sort<string>(proc, (a, b) => a.CompareTo(b));

			return new List<string>(proc);
		}

		/// <summary>
		/// Метод, возвращающий список объектов класса ProgramSoft, содержащих в себе информацию об установленном ПО.
		/// </summary>
		/// <returns>Список объектов класса ProgramSoft.</returns>
		public static List<ProgramSoft> InstalledSoft()
		{
			List<ProgramSoft> soft = new List<ProgramSoft>();
		
			ManagementObjectSearcher searcher_soft =
					new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Product");

			foreach (ManagementObject queryObj in searcher_soft.Get())
			{
                if (queryObj["Caption"] == null || queryObj["InstallDate"]==null) continue;
				soft.Add(new ProgramSoft(queryObj["Caption"].ToString(), 
										 queryObj["InstallDate"].ToString()));
			}

			Array.Sort(soft.ToArray<ProgramSoft>(), (a, b) => a.Softname.CompareTo(b.Softname));

			return soft;
		}

		/// <summary>
		/// Метод, возвращающий объект класса SystemOperationInfo, в котором содержится информация об операционной системе ПК
		/// </summary>
		/// <returns>Объект класса OperatingSystemInfo</returns>
		public static OperatingSystemInfo SystemInfo()
		{
			OperatingSystemInfo systemInfo = null;

			ManagementObjectSearcher searcher =
			new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");

			foreach (ManagementObject queryObj in searcher.Get())
			{

				int buildNumber = int.Parse(queryObj["BuildNumber"].ToString());
				string caption = queryObj["Caption"].ToString();
				uint phys_memory = uint.Parse(queryObj["FreePhysicalMemory"].ToString());
				uint virt_memory = uint.Parse(queryObj["FreeVirtualMemory"].ToString());
				string name = queryObj["Name"].ToString();
				int os = int.Parse(queryObj["OSType"].ToString());
				string user = queryObj["RegisteredUser"].ToString();
				string serial = queryObj["SerialNumber"].ToString();
				int major_ver = int.Parse(queryObj["ServicePackMajorVersion"].ToString());
				int minor_ver = int.Parse(queryObj["ServicePackMinorVersion"].ToString());
				string status = queryObj["Status"].ToString();
				string sys_device = queryObj["SystemDevice"].ToString();
				string sys_dir = queryObj["SystemDirectory"].ToString();
				string sys_drive = queryObj["SystemDrive"].ToString();
				string version = queryObj["Version"].ToString();
				string win_dir = queryObj["WindowsDirectory"].ToString();

				systemInfo = new OperatingSystemInfo(buildNumber, caption, phys_memory,
													 virt_memory, name, os, user, serial,
													 major_ver, minor_ver, status, sys_device,
													 sys_dir, sys_drive, version, win_dir);

			}

			return systemInfo;
		}

        /// <summary>
        /// Метод, возвращающий информацию о дисках ПК.
        /// </summary>
        /// <returns>Список объектов типа DiskInfo</returns>
        public static List<DiskInfo> DisksInfo()
        {
            List<DiskInfo> info = new List<DiskInfo>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Volume");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                DiskInfo info1;
                ulong capacity;
                if (queryObj["Capacity"] != null) capacity = ulong.Parse(queryObj["Capacity"].ToString());
                else continue;
                string caption = queryObj["Caption"].ToString();
                string letter;
                if (queryObj["DriveLetter"] != null) letter = queryObj["DriveLetter"].ToString();
                else continue;
                int drive = int.Parse(queryObj["DriveType"].ToString());
                string file_sys = queryObj["FileSystem"].ToString();
                ulong free = ulong.Parse(queryObj["FreeSpace"].ToString());

                info1 = new DiskInfo(capacity, caption, letter, drive, file_sys, free);
                info.Add(info1);
            }

            return info;
        }

        /// <summary>
        /// Метод, возвращающий всю информацию о видеокартах ПК.
        /// </summary>
        /// <returns>Список объектов класса VideoControllerInfo</returns>
        public static List<VideoControllerInfo> VideoControllersInfo()
        {

            List<VideoControllerInfo> info = new List<VideoControllerInfo>();
            VideoControllerInfo additional;
            ManagementObjectSearcher searcher11 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");

            foreach (ManagementObject queryObj in searcher11.Get())
            {
                ulong Aram = ulong.Parse(queryObj["AdapterRAM"].ToString());
                additional = new VideoControllerInfo(Aram, queryObj["Caption"].ToString(), queryObj["Description"].ToString(), queryObj["VideoProcessor"].ToString());
                info.Add(additional);
            }

            return info;
        }

        /// <summary>
        /// Метод, возвращающий всю информацию о процессорах ПК.
        /// </summary>
        /// <returns>Список объектов класса ProcessorInfo</returns>
        public static List<ProcessorInfo> ProcessorsInfo()
        {
            List<ProcessorInfo> info = new List<ProcessorInfo>();
            ProcessorInfo addp;

            ManagementObjectSearcher searcher8 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");

            foreach (ManagementObject queryObj in searcher8.Get())
            {
                int p1 = int.Parse(queryObj["NumberOfCores"].ToString());
                ulong p2 = ulong.Parse(queryObj["NumberOfCores"].ToString());

                addp = new ProcessorInfo(queryObj["Name"].ToString(), p1, p2);
                info.Add(addp);
            }

            return info;
        }

        /// <summary>
        /// Метод, возвращающее число, показывающее, сколько свободно физической памяти у ПК.
        /// </summary>
        /// <param name="reg">Параметр вывода от 0 до 4 (от байт до терабайт соотв.)</param>
        /// <returns></returns>
        public static double AvailablePhysicalMemory(byte reg)
        {
            ComputerInfo ci = new ComputerInfo();
            double ret;
            switch (reg)
            {
                case 0: //B
                    ret = ci.AvailablePhysicalMemory;
                    break;
                case 1: //KB
                    ret = Math.Round(ci.AvailablePhysicalMemory / 1024.0, 3);
                    break;
                case 2: //MB
                    ret = Math.Round(ci.AvailablePhysicalMemory / 1024.0 / 1024.0,3);
                    break;
                case 3: //Gb
                    ret = Math.Round(ci.AvailablePhysicalMemory / 1024.0 / 1024.0 / 1024.0,3);
                    break;
                case 4:
                    ret = Math.Round(ci.AvailablePhysicalMemory / 1024.0 / 1024.0 / 1024.0 / 1024.0,3);
                    break;
                default:
                    throw new ArgumentException("Parameter does not equal to 0, 1, 2, 3 or 4");
            }
            return ret;
        }

        /// <summary>
        /// Метод, возвращающее число, показывающее, сколько свободно виртуального адресного пространства у ПК.
        /// </summary>
        /// <param name="reg">Параметр вывода от 0 до 4 (от байт до терабайт соотв.)</param>
        /// <returns></returns>
        public static double AvailibleVirtualMemory(byte reg)
        {
            ComputerInfo ci = new ComputerInfo();
            double ret;
            switch (reg)
            {
                case 0: //B
                    ret = ci.AvailableVirtualMemory;
                    break;
                case 1: //KB
                    ret = Math.Round(ci.AvailableVirtualMemory / 1024.0, 3);
                    break;
                case 2: //MB
                    ret = Math.Round(ci.AvailableVirtualMemory / 1024.0 / 1024.0, 3);
                    break;
                case 3: //Gb
                    ret = Math.Round(ci.AvailableVirtualMemory / 1024.0 / 1024.0 / 1024.0, 3);
                    break;
                case 4:
                    ret = Math.Round(ci.AvailableVirtualMemory / 1024.0 / 1024.0 / 1024.0 / 1024.0, 3);
                    break;
                default:
                    throw new ArgumentException("Parameter does not equal to 0, 1, 2, 3 or 4");
            }
            return ret;
        }

        /// <summary>
        /// Метод, возвращающий информацию о загруженности процессора ПК и его ядер.
        /// </summary>
        /// <returns></returns>
        public static string CPULoad()
        {
            string ret = String.Empty;
            PerformanceCounter pc = new PerformanceCounter("Процессор", "% загруженности процессора", "_Total");

            int num = PCStatus.ProcessorsInfo()[0].CoresNumber;
            List<PerformanceCounter> list = new List<PerformanceCounter>();
            list.Add(pc);

            for (int i = 0; i < num; i++)
            {
                list.Add(new PerformanceCounter("Процессор", "% загруженности процессора", i.ToString()));
            }

            ret += $"Процессор загружен на: {pc.NextValue()}\r\n";
            for (int i = 1; i <= num; i++)
            {
                ret += "\r\n";
                ret+=$"Ядро {i - 1} загружено на {list[i].NextValue()}%";
            }
            return ret;
        }
    }

    ///TODO: Make a new method in each class to serialize info


    #region Classes

    /// <summary>
    /// Класс, в котором содержится информация о единице установленного ПО.
    /// </summary>
    [Serializable]
	public class ProgramSoft
	{
		/// <summary>
		/// Имя установленного ПО.
		/// </summary>
		public string Softname { get; private set; }

		/// <summary>
		/// Дата установки ПО.
		/// </summary>
		public string Installing_date { get; private set; }

		/// <summary>
		/// Конструктор создания объекта класса ProgramSoft.
		/// </summary>
		/// <param name="soft">Имя установленного ПО.</param>
		/// <param name="date">Дата установки ПО.</param>
		public ProgramSoft(string soft, string date)
		{
			Softname = soft;
			Installing_date = date.Substring(0,4)+"/"+date.Substring(4,2) + "/" + date.Substring(4, 2);
		}

        public string GetFields()
        {
            return $"Имя: {Softname} ; " + $"Дата установки: {Installing_date}";
        }
	}

	/// <summary>
	/// Класс, предоставляющий информацию об операционной системе ПК.
	/// </summary>
    [Serializable]
	public class OperatingSystemInfo
	{
		public int BuildNumber { get; private set; }
		public string Caption { get; private set; }
		public uint FreePhysicalMemory { get; private set; }
		public uint FreeVirtualMemory { get; private set; }
		public string Name { get; private set; }
		public int OSType { get; private set; }
		public string RegisteredUser { get; private set; }
		public string Serial { get; private set; }
		public int ServiceMajorVersion { get; private set; }
		public int ServiceMinorVersion { get; private set; }
		public string Status { get; private set; }
		public string SystemDevice { get; private set; }
		public string SystemDirectory { get; private set; }
		public string SystemDrive { get; private set; }
		public string Version { get; private set; }
		public string WindowsDirectory { get; private set; }

		public OperatingSystemInfo(int build, string caption, uint phys_memory, uint virt_memory,
								   string name, int OSType, string user, string serial,
								   int major_version, int minor_version, string status,
								   string sys_device, string sys_directory, string sys_drive,
								   string version, string win_directory)
		{
			BuildNumber = build;
			Caption = caption;
			FreePhysicalMemory = phys_memory;
			FreeVirtualMemory = virt_memory;
			Name = name;
			this.OSType = OSType;
			RegisteredUser = user;
			Serial = serial;
			ServiceMajorVersion = major_version;
			ServiceMinorVersion = minor_version;
			Status = status;
			SystemDevice = sys_device;
			SystemDirectory = sys_directory;
			SystemDrive = sys_drive;
			Version = version;
			WindowsDirectory = win_directory;
		}

        public string GetFields()
        {
            return $"Build number: {BuildNumber}\r\n" +
                   $"Caption: {Caption}\r\n" +
                   $"Free Physical Memory: {FreePhysicalMemory}\r\n" +
                   $"Free Virtual Memory: {FreeVirtualMemory}\r\n" +
                   $"Name: {Name}\r\n" +
                   $"OS Type: {OSType}\r\n" +
                   $"Registered user: {RegisteredUser}\r\n" +
                   $"Serial number: {Serial}\r\n" +
                   $"Service Major Version: {ServiceMajorVersion}\r\n" +
                   $"Service Minor Version: {ServiceMinorVersion}\r\n" +
                   $"Status: {Status}\r\n" +
                   $"System Device: {SystemDevice}\r\n" +
                   $"System Directory: {SystemDirectory}\r\n" +
                   $"System Drive: {SystemDrive}\r\n" +
                   $"Version: {Version}\r\n" +
                   $"Windows Directory: {WindowsDirectory}";
        }
	}

    /// <summary>
    /// Класс, предоставляющий доступ к информации о дисках ПК.
    /// </summary>
    [Serializable]
    public class DiskInfo
    {
        /// <summary>
        /// Объем памяти диска в байтах
        /// </summary>
        public ulong Capacity { get; private set; }
        /// <summary>
        /// Корневой каталог диска
        /// </summary>
        public string Caption { get; private set; }
        /// <summary>
        /// Метка тома диска
        /// </summary>
        public string Letter { get; private set; }
        /// <summary>
        /// Номер типа диска
        /// </summary>
        public int DriveType { get; private set; }
        /// <summary>
        /// Тип файловой системы диска
        /// </summary>
        public string FileSystem { get; private set; }
        /// <summary>
        /// Объем достуного места на диске
        /// </summary>
        public ulong FreeSpace { get; private set; }
        public DiskInfo(ulong cap, string capt, string let, int drive, string file_sys, ulong free)
        {
            Capacity = cap;
            Caption = capt;
            Letter = let;
            DriveType = drive;
            FileSystem = file_sys;
            FreeSpace = free;
        }
        /// <summary>
        /// Метод, возвращающий всю информацию об одном диске ПК.
        /// </summary>
        /// <returns>Строка с информацией о диске</returns>
        public string GetFields()
        {
            return $"Capacity: {Capacity} Bytes\r\n" +
                   $"Caption: {Caption}\r\n" +
                   $"Letter: {Letter}\r\n" +
                   $"Drive Type: {DriveType}\r\n" +
                   $"File System: {FileSystem}\r\n" +
                   $"Free Space: {FreeSpace} Bytes";
        }
    }

    /// <summary>
    /// Класс, предоставляющий информацию о видеоадаптерах ПК.
    /// </summary>
    [Serializable]
    public class VideoControllerInfo
    {
        /// <summary>
        /// Полный объем RAM-памяти видеоадаптера.
        /// </summary>
        public ulong RAM { get; private set; }
        /// <summary>
        /// Название видеоадаптера
        /// </summary>
        public string Caption { get; private set; }
        /// <summary>
        /// Системное описание видеоадаптера
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// Название видеопроцессора
        /// </summary>
        public string VideoProcessor { get; private set; }
        public VideoControllerInfo(ulong ram, string cap, string descr, string vpr)
        {
            RAM = ram;
            Caption = cap;
            Description = descr;
            VideoProcessor = vpr;
        }
        /// <summary>
        /// Метод, возвращающий информацию об одном видеоадаптере ПК.
        /// </summary>
        /// <returns></returns>
        public string GetFields()
        {
            return $"RAM: {RAM} Bytes\r\n" +
                   $"Caption: {Caption}\r\n" +
                   $"Description: {Description}\r\n" +
                   $"VideoProcessor: {VideoProcessor}";
        }
    }

    /// <summary>
    /// Класс, предоставлющий информацию о процессоре ПК
    /// </summary>
    [Serializable]
    public class ProcessorInfo
    {
        /// <summary>
        /// Системное имя процессора
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Число ядер процессора
        /// </summary>
        public int CoresNumber { get; private set; }
        /// <summary>
        /// ID процессора
        /// </summary>
        public ulong ProcessorID { get; private set; }
        public ProcessorInfo(string Name, int CoresNumber, ulong ID)
        {
            this.Name = Name;
            this.CoresNumber = CoresNumber;
            ProcessorID = ID;
        }
        /// <summary>
        /// Метод, возвращающий информацию о процессоре
        /// </summary>
        /// <returns></returns>
        public string GetFields()
        {
            return $"Name: {Name}\r\n" +
                   $"Number of cores: {CoresNumber}\r\n" +
                   $"Processor ID: {ProcessorID}";
        }
    }

    #endregion
}
