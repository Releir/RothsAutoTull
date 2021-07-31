using System;
using System.Windows;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using BuffHelper;
using KeyHelper;
using System.Configuration;
using System.Collections.Specialized;

namespace RothsAutoTull
{
    public partial class Form1 : Form
    {

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        const uint WM_KEYDOWN = 0x100;
        const uint WM_KEYUP = 0x101;

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

            for (int ctr = 0; ctr < KeyExtension.keyComboList.Count; ctr++)
            {
                ComboBox cb = (ComboBox)this.Controls.Find(KeyExtension.keyComboList[ctr], true)[0];
                KeyExtension.loadKeys(cb);
                KeyExtension.loadConfigKey(cb);
            }
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

        public volatile Boolean isRunning = true;

        private void startBtn_Click(object sender, EventArgs e)
        {
            
            Process prc = clientBox.SelectedValue as Process;
            Thread tid1 = new Thread(() => autoPots(prc));
            Thread tid2 = new Thread(() => autoRemoveStatus(prc));
            Thread tid3 = new Thread(() => autoBuffs(prc));
            Thread tid4 = new Thread(() => autoEquipShieldWhenBroken(prc));
            tid1.IsBackground = true;
            tid2.IsBackground = true;
            tid3.IsBackground = true;
            tid4.IsBackground = true;
            isRunning = true;
            tid1.Start();
            tid2.Start();
            tid3.Start();
            tid4.Start();
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
                    KeyExtension.pressKey(actualProcess, hpKey);
                }

                if (spPerc < spPercToPot)
                {
                    KeyExtension.pressKey(actualProcess, spKey);
                }
                Thread.Sleep(1);
            }
        }

        private void autoRemoveStatus(Process actualProcess)
        {
            if (panaceaCheck.Checked)
            {
                int panaceaKey = Convert.ToInt32(panaceaBox.SelectedValue);
                while (true)
                {
                    if (!isRunning)
                    {
                        break;
                    }
                    List<int> buffArr = new List<int>();
                    populateBuffs(actualProcess, buffArr);

                    if (buffArr.Intersect(DebuffExtension.getPanaceaStatus()).Any())
                    {
                        PostMessage(actualProcess.MainWindowHandle, WM_KEYDOWN, panaceaKey, 0);
                        PostMessage(actualProcess.MainWindowHandle, WM_KEYUP, panaceaKey, 0);
                    }
                    buffArr.Clear();
                    Thread.Sleep(1);
                }
            }
        }

        private void autoBuffs(Process actualProcess)
        {
            int fireKey = Convert.ToInt32(fireProofKey.SelectedValue);
            int earthKey = Convert.ToInt32(earthProofKey.SelectedValue);
            int coldKey = Convert.ToInt32(coldProofKey.SelectedValue);
            int thunderKey = Convert.ToInt32(thunderProofKey.SelectedValue);
            int aloeveraKey = Convert.ToInt32(aloeKey.SelectedValue);
            int boxOfResentKey = Convert.ToInt32(resentKey.SelectedValue);
            int holyScrollKey = Convert.ToInt32(holyKey.SelectedValue);
            int speedPotKey = Convert.ToInt32(speedKey.SelectedValue);
            int cursedWaterKey = Convert.ToInt32(cursedKey.SelectedValue);
            while (true)
            {
                if (!isRunning)
                {
                    break;
                }
                List<int> buffArr = new List<int>();
                populateBuffs(actualProcess, buffArr);

                if (crusaderBuffEnabled.Checked)
                {
                    int autoGuard = 58;
                    int reflectShield = 59;
                    if (!buffArr.Contains(autoGuard))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F4);
                    }
                    if (!buffArr.Contains(reflectShield))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F5);
                    }
                }

                pressBuff(actualProcess, fireProofEnabled, buffArr, (int)ItemBuffs.Fireproof, fireKey);
                pressBuff(actualProcess, coldProofEnabled, buffArr, (int)ItemBuffs.Coldproof, coldKey);
                pressBuff(actualProcess, earthProofEnabled, buffArr, (int)ItemBuffs.Earthproof, earthKey);
                pressBuff(actualProcess, thunderProofEnabled, buffArr, (int)ItemBuffs.Thunderproof, thunderKey);
                pressBuff(actualProcess, aloeEnabled, buffArr, (int)ItemBuffs.Aloevera, aloeveraKey);
                pressBuff(actualProcess, resentEnabled, buffArr, (int)ItemBuffs.Resentment, boxOfResentKey);
                pressBuff(actualProcess, holyEnabled, buffArr, (int)ItemBuffs.HolyScroll, holyScrollKey);
                pressBuff(actualProcess, cursedEnabled, buffArr, (int)ItemBuffs.CursedWater, cursedWaterKey);
                pressBuff(actualProcess, speedEnabled, buffArr, (int)ItemBuffs.SpeedPots, speedPotKey);

                buffArr.Clear();
                Thread.Sleep(10);
            }
        }

        private void pressBuff(Process proc, CheckBox cb, List<int>buffArr, int buffNum, int keyBind)
        {
            if (cb.Checked)
            {
                if (!buffArr.Contains(buffNum))
                {
                    KeyExtension.pressKey(proc, keyBind);
                }
            }
        }

        private void populateBuffs(Process actualProcess, List<int> buffArr)
        {
            int buffStart = 0x010DD284;
            for (int ctr = 0; ctr <= 200; ctr = ctr + 4)
            {
                int buffAddr = buffStart + ctr;
                int buffVal = ReadInt32(actualProcess.Handle, buffAddr);
                buffArr.Add(buffVal);
            }
        }

        private void autoEquipShieldWhenBroken(Process actualProcess)
        {
            int shield = Convert.ToInt32(shieldKey.SelectedValue);
            int weapon = Convert.ToInt32(weaponKey.SelectedValue);
            int upperHg = Convert.ToInt32(upperHgKey.SelectedValue);
            int armor = Convert.ToInt32(armorKey.SelectedValue);
            int shieldAddr = 0x010D9A04;
            int weaponAddr = 0x010D9684;
            int upperHgAddr = 0x010D9CA4;
            int armorAddr = 0x010D9924;
            while (true)
            {
                if (!isRunning)
                {
                    break;
                }
                equipItemWhenEnabled(actualProcess, shieldAddr, shield, shieldEnabled);
                equipItemWhenEnabled(actualProcess, weaponAddr, weapon, weaponEnabled);
                equipItemWhenEnabled(actualProcess, upperHgAddr, upperHg, upperEnabled);
                equipItemWhenEnabled(actualProcess, armorAddr, armor, armorEnabled);
                Thread.Sleep(30);
            }
        }

        private void equipItemWhenEnabled(Process proc, int equipAddr, int keyBind, CheckBox isEnabled)
        {
            if (isEnabled.Checked)
            {
                int isEquipped = ReadInt32(proc.Handle, equipAddr);
                if (isEquipped == 0)
                {
                    KeyExtension.pressKey(proc, keyBind);
                }
                Thread.Sleep(30);
            }
        }

        private void saveKeyBtn_Click(object sender, EventArgs e)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            for (int ctr = 0; ctr < KeyExtension.keyComboList.Count; ctr++)
            {
                ComboBox cb = (ComboBox)this.Controls.Find(KeyExtension.keyComboList[ctr], true)[0];
                configuration.AppSettings.Settings.Remove(cb.Name);
                configuration.AppSettings.Settings.Add(cb.Name, cb.Text);
            }
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            MessageBox.Show("Keybind saved!");
        }
    }
}
