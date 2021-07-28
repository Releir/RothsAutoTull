using System;
using System.Windows;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace RothsAutoTull
{
    public partial class Form1 : Form
    {

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        const uint WM_KEYDOWN = 0x100;
        const uint WM_KEYUP = 0x101;
        const uint WM_CHAR = 0x102;
        const int VK_F9 = 0x78;

        const int PROCESS_WM_READ = 0x0010;
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
            [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

        private static byte[] ReadBytes(IntPtr handle, long address, uint bytesToRead)
        {
            IntPtr ptrBytesRead;
            byte[] buffer = new byte[bytesToRead];

            ReadProcessMemory(handle, new IntPtr(address), buffer, bytesToRead, out ptrBytesRead);

            return buffer;
        }

        private static int ReadInt32(IntPtr handle, long address)
        {
            return BitConverter.ToInt32(ReadBytes(handle, address, 4), 0);
        }

        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;  //Not good but it serves the purpose of just getting and not writing
            InitializeComponent();
            LoadProcesses();
            LoadKeys(hpKeyBox);
            LoadKeys(spKeyBox);
            LoadKeys(panaceaBox);
        }

        private void LoadProcesses()
        {
            clientBox.Items.Clear();
            Process[] MyProcess = Process.GetProcesses();
            Dictionary<Process, String> applications = new Dictionary<Process, String>();
            for (int i = 0; i < MyProcess.Length; i++)
            {
                if (MyProcess[i].MainWindowTitle.Length > 0)
                {
                    applications.Add(MyProcess[i], MyProcess[i].MainWindowTitle);
                }
            }
            clientBox.DataSource = new BindingSource(applications, null);
            clientBox.DisplayMember = "Value";
            clientBox.ValueMember = "Key";
        }

        private void LoadKeys(ComboBox box)
        {
            var keyboardDictionary = new Dictionary<Keys, string>{
                   {Keys.F1, "F1"},
                   {Keys.F2, "F2"},
                   {Keys.F3, "F3"},
                   {Keys.F4, "F4"},
                   {Keys.F5, "F5"},
                   {Keys.F6, "F6"},
                   {Keys.F7, "F7"},
                   {Keys.F8, "F8"},
                   {Keys.F9, "F9"},

            };
            box.DataSource = new BindingSource(keyboardDictionary, null);
            box.DisplayMember = "Value";
            box.ValueMember = "Key";
        }

        public volatile Boolean isRunning = true;

        private void startBtn_Click(object sender, EventArgs e)
        {
            Process prc = clientBox.SelectedValue as Process;
            Thread tid1 = new Thread(() => autoPots(prc));
            Thread tid2 = new Thread(() => autoRemoveStatus(prc));
            tid1.IsBackground = true;
            tid2.IsBackground = true;
            isRunning = true;
            tid1.Start();
            tid2.Start();
            startBtn.Enabled = false;
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            isRunning = false;
            startBtn.Enabled = true;
        }

        private void autoPots(Process actualProcess)
        {
            IntPtr proc = actualProcess.Handle;
            int hpAddr = 0x010DCE10;
            int hpMaxAddr = 0x010DCE10 + 4;
            int spAddr = 0x010DCE10 + 8;
            int spMaxAddr = 0x010DCE10 + 12;

            double hpPercToPot = Convert.ToDouble(hpBox.Text);
            double spPercToPot = Convert.ToDouble(spBox.Text);

            while (true)
            {
                if (!isRunning)
                {
                    break;
                }
                int hpVal = ReadInt32(proc, hpAddr);
                int hpMax = ReadInt32(proc, hpMaxAddr);
                int spVal = ReadInt32(proc, spAddr);
                int spMax = ReadInt32(proc, spMaxAddr);

                double hpPerc = ((double)hpVal / (double)hpMax) * 100;
                double spPerc = ((double)spVal / (double)spMax) * 100;

                int hpKey = Convert.ToInt32(hpKeyBox.SelectedValue);
                int spKey = Convert.ToInt32(spKeyBox.SelectedValue);

                if (hpPerc < hpPercToPot)
                {
                    PostMessage(actualProcess.MainWindowHandle, WM_KEYDOWN, hpKey, 0);
                    PostMessage(actualProcess.MainWindowHandle, WM_KEYUP, hpKey, 0);
                }
                if (spPerc < spPercToPot)
                {
                    PostMessage(actualProcess.MainWindowHandle, WM_KEYDOWN, spKey, 0);
                    PostMessage(actualProcess.MainWindowHandle, WM_KEYUP, spKey, 0);
                }
                Thread.Sleep(1);
            }
        }

        private void autoRemoveStatus(Process actualProcess)
        {
            int panaceaKey = Convert.ToInt32(panaceaBox.SelectedValue);
            while (true)
            {
                if (!isRunning)
                {
                    break;
                }
                List<int> buffArr = new List<int>();
                int buffStart = 0x010DD284;
                for (int ctr = 0; ctr <= 120; ctr = ctr + 4)
                {
                    int buffAddr = buffStart + ctr;
                    int buffVal = ReadInt32(actualProcess.Handle, buffAddr);
                    buffArr.Add(buffVal);
                }

                int silence = 885;
                int curse = 884;
                if (buffArr.Contains(silence) || buffArr.Contains(curse))
                {
                    PostMessage(actualProcess.MainWindowHandle, WM_KEYDOWN, panaceaKey, 0);
                    PostMessage(actualProcess.MainWindowHandle, WM_KEYUP, panaceaKey, 0);
                }
                buffArr.Clear();
                Thread.Sleep(1);
            }
        }
    }
}
