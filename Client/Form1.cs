using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PCStatusLib;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SandBoxConsole;
using System.Diagnostics;
using System.Threading;

namespace Client
{
    public partial class Form1 : Form
    {
        Thread t1;
        public Form1()
        {
            InitializeComponent();
        }

        void Load_Active_Processes()
        {
            while (true)
            {
                try
                {
                    string info = String.Empty;
                    BinaryFormatter f = new BinaryFormatter();
                    FileStream s = new FileStream(@"../../Active Processes.txt", FileMode.Open);

                    List<string> list = (List<string>)f.Deserialize(s);
                    foreach (string i in list)
                    {
                        info += i + "\r\n";
                    }

                    active_processes_textBox.SuspendLayout();
                    if (!info.Equals(active_processes_textBox.Text)) active_processes_textBox.Text = info;
                    active_processes_textBox.Invalidate();
                    active_processes_textBox.ResumeLayout();
                    s.Close();
                    Thread.Sleep(1000);
                }
                catch { }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ThreadStart th1 = new ThreadStart(SAP);
            ThreadStart th2 = new ThreadStart(Load_Active_Processes);
            t1 = new Thread(th1);
            Thread t2 = new Thread(th2);
            t1.Start();
            t2.Start();
        }

        void SAP()
        {
            while (true)
            {
                try
                {
                    string file = @"../../Active Processes.txt";
                    List<string> list = PCStatus.ActiveProcesses();
                    FileStream s1 = new FileStream(file, FileMode.Create);
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (s1)
                    {
                        formatter.Serialize(s1, list);
                    }
                    Thread.Sleep(1000);
                }
                catch { }
            }
        }
    }
}
