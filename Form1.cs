using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using BuffHelper;
using KeyHelper;
using System.Configuration;
using RothsAutoTull.Helper;

namespace RothsAutoTull
{
    public partial class Form1 : Form
    {

        public static Process prc;

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

        KeyboardHook hook = new KeyboardHook();
        KeyboardHook speedPotHook = new KeyboardHook();

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

            // register the event that is fired after the key press.
            hook.KeyPressed +=
                new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            // register the control + alt + F12 combination as hot key.
            hook.RegisterHotKey( Helper.ModifierKeys.Alt,
                Keys.D);

            speedPotHook.KeyPressed +=
                new EventHandler<KeyPressedEventArgs>(speedPot_KeyPressed);

            speedPotHook.RegisterHotKey(Helper.ModifierKeys.Alt, Keys.F);
        }

        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (autoCloak.Checked)
            {
                autoCloak.Checked = false;
            } else
            {
                autoCloak.Checked = true;
            }
        }

        void speedPot_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (speedEnabled.Checked)
            {
                speedEnabled.Checked = false;
            }
            else
            {
                speedEnabled.Checked = true;
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
            
            prc = clientBox.SelectedValue as Process;
            Thread tid1 = new Thread(() => autoPots(prc));
            Thread tid2 = new Thread(() => autoRemoveStatus(prc));
            Thread tid3 = new Thread(() => autoBuffs(prc));
            Thread tid4 = new Thread(() => autoEquipShieldWhenBroken(prc));
            Thread tid5 = new Thread(() => autoPots2(prc));
            Thread tid6 = new Thread(() => autoPots3(prc));
            Thread tid7 = new Thread(() => autoPeony(prc));
            Thread tid8 = new Thread(() => autoPots4(prc));
            Thread tid9 = new Thread(() => autoMana(prc));
            tid1.IsBackground = true;
            tid2.IsBackground = true;
            tid3.IsBackground = true;
            tid4.IsBackground = true;
            tid5.IsBackground = true;
            tid6.IsBackground = true;
            tid7.IsBackground = true;
            tid8.IsBackground = true;
            tid9.IsBackground = true;
            isRunning = true;
            tid1.Start();
            tid9.Start();
            tid2.Start();
            tid3.Start();
            tid4.Start();
            tid5.Start();
            tid6.Start();
            tid7.Start();
            tid8.Start();
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
            int hpAddr = 0x010DCE10;  //New Era Addr
            int hpMaxAddr = hpAddr + 4;
            //int spAddr = hpAddr + 8;
            //int spMaxAddr = hpAddr + 12;

            double hpPercToPot = Convert.ToDouble(hpBox.Text);
            //double spPercToPot = Convert.ToDouble(spBox.Text);

            while (true)
            {
                if (!isRunning)
                {
                    break;
                }
                int hpVal = ReadInt32(proc, hpAddr);
                int hpMax = ReadInt32(proc, hpMaxAddr);
                //int spVal = ReadInt32(proc, spAddr);
                //int spMax = ReadInt32(proc, spMaxAddr);

                double hpPerc = ((double)hpVal / (double)hpMax) * 100;
                //double spPerc = ((double)spVal / (double)spMax) * 100;

                int hpKey = Convert.ToInt32(hpKeyBox.SelectedValue);
                //int spKey = Convert.ToInt32(spKeyBox.SelectedValue);

                if (hpPerc < hpPercToPot)
                {
                    KeyExtension.pressKey(actualProcess, hpKey);
                    //Thread.Sleep(RandomNumber());
                }

                //if (spPerc < spPercToPot)
                //{
                //    KeyExtension.pressKey(actualProcess, spKey);
                //}
                Thread.Sleep(1);
                //Thread.Sleep(RandomNumber());
            }
        }

        private void autoMana(Process actualProcess)
        {
            IntPtr proc = actualProcess.Handle;
            int hpAddr = 0x010DCE10;  //New Era Addr
            int spAddr = hpAddr + 8;
            int spMaxAddr = hpAddr + 12;

            double spPercToPot = Convert.ToDouble(spBox.Text);

            while (true)
            {
                int spVal = ReadInt32(proc, spAddr);
                int spMax = ReadInt32(proc, spMaxAddr);

                double spPerc = ((double)spVal / (double)spMax) * 100;

                //sint spKey = Convert.ToInt32(spKeyBox.SelectedValue);

                if (spPerc < spPercToPot)
                {
                    KeyExtension.pressKey(actualProcess, (int)Keys.F6);
                    //Thread.Sleep(RandomNumber());
                }
                Thread.Sleep(1);
                //Thread.Sleep(RandomNumber());
            }
        }

        //Remember to refactor!! You've put this here because gm implemented pumpkin pie!!!
        private void autoPots2(Process actualProcess)
        {
            IntPtr proc = actualProcess.Handle;
            int hpAddr = 0x010DCE10;  //New Era Addr
            int hpMaxAddr = hpAddr + 4;

            double hpPercToPot = Convert.ToDouble(hp2Box.Text);

            if (hp2Enable.Checked)
            {
                while (true)
                {
                    if (!isRunning)
                    {
                        break;
                    }

                    int hpVal = ReadInt32(proc, hpAddr);
                    int hpMax = ReadInt32(proc, hpMaxAddr);

                    double hpPerc = ((double)hpVal / (double)hpMax) * 100;

                    int hpKey = Convert.ToInt32(hp2KeyBox.SelectedValue);

                    if (hpPerc < hpPercToPot)
                    {
                        KeyExtension.pressKey(actualProcess, hpKey);
                    }

                    Thread.Sleep(1);
                }
            }
            
        }

        private void autoPots3(Process actualProcess)
        {
            IntPtr proc = actualProcess.Handle;
            int hpAddr = 0x010DCE10;  //New Era Addr
            int hpMaxAddr = hpAddr + 4;

            double hpPercToPot = Convert.ToDouble(hp3Box.Text);

            if (hp3Enable.Checked)
            {
                while (true)
                {
                    if (!isRunning)
                    {
                        break;
                    }

                    int hpVal = ReadInt32(proc, hpAddr);
                    int hpMax = ReadInt32(proc, hpMaxAddr);

                    double hpPerc = ((double)hpVal / (double)hpMax) * 100;

                    int hpKey = Convert.ToInt32(hp3KeyBox.SelectedValue);

                    if (hpPerc < hpPercToPot)
                    {
                        KeyExtension.pressKey(actualProcess, hpKey);
                    }

                    Thread.Sleep(1);
                }
            }

        }

        private void autoPots4(Process actualProcess)
        {
            IntPtr proc = actualProcess.Handle;
            int hpAddr = 0x010DCE10;  //New Era Addr
            int hpMaxAddr = hpAddr + 4;

            double hpPercToPot = Convert.ToDouble(hp4Box.Text);

            if (hp4Enable.Checked)
            {
                while (true)
                {
                    if (!isRunning)
                    {
                        break;
                    }

                    int hpVal = ReadInt32(proc, hpAddr);
                    int hpMax = ReadInt32(proc, hpMaxAddr);

                    double hpPerc = ((double)hpVal / (double)hpMax) * 100;

                    int hpKey = Convert.ToInt32(hp4KeyBox.SelectedValue);

                    if (hpPerc < hpPercToPot)
                    {
                        KeyExtension.pressKey(actualProcess, hpKey);
                    }

                    Thread.Sleep(1);
                }
            }

        }

        private void autoPeony(Process actualProcess)
        {
            if (peonyCheck.Checked)
            {
                int peonyKey = Convert.ToInt32(peonyBox.SelectedValue);
                while (true)
                {
                    if (!isRunning)
                    {
                        break;
                    }
                    List<int> buffArr = new List<int>();
                    populateBuffs(actualProcess, buffArr);

                    if (buffArr.Intersect(DebuffExtension.getPeonyStatus()).Any())
                    {
                        PostMessage(actualProcess.MainWindowHandle, WM_KEYDOWN, peonyKey, 0);
                        PostMessage(actualProcess.MainWindowHandle, WM_KEYUP, peonyKey, 0);
                    }
                    buffArr.Clear();
                    Thread.Sleep(1);
                }
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
            int strFKey = Convert.ToInt32(strKey.SelectedValue);
            int agiFKey = Convert.ToInt32(agiKey.SelectedValue);
            int vitFKey = Convert.ToInt32(vitKey.SelectedValue);
            int dexFKey = Convert.ToInt32(dexKey.SelectedValue);
            int intFKey = Convert.ToInt32(intKey.SelectedValue);
            int lukFKey = Convert.ToInt32(lukKey.SelectedValue);
            int bDefKey = Convert.ToInt32(defKey.SelectedValue);
            int bMdefKey = Convert.ToInt32(mdefKey.SelectedValue);
            int _daehKey = Convert.ToInt32(daehKey.SelectedValue);
            int _taeKey = Convert.ToInt32(taeKey.SelectedValue);
            int conKey = Convert.ToInt32(concentKey.SelectedValue);
            int awakeningKey = Convert.ToInt32(awakeKey.SelectedValue);
            int anodKey = Convert.ToInt32(anodyneKey.SelectedValue);
            int _botKey = Convert.ToInt32(botKey.SelectedValue);
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
                    int defender = 62;
                    if (!buffArr.Contains(autoGuard))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F4);
                    }
                    if (!buffArr.Contains(reflectShield))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F5);
                    }
                    
                }

                if (archerBuffEnabled.Checked)
                {
                    int spirit = 149;
                    int improveConcen = 3;
                    if (!buffArr.Contains(improveConcen))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F4);
                    }
                    if (!buffArr.Contains(spirit))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F5);
                    }
                }

                if (enableAntiStrip.Checked)
                {
                    int stripWeap = 50;
                    int stripShield = 51;
                    int stripArmor = 52;
                    int stripHeadgear = 53;
                    int cpWeap = 54;
                    int cpShield = 55;
                    int cpArmor = 56;
                    int cpHelm = 57;
                    if (buffArr.Contains(stripWeap) || !buffArr.Contains(cpWeap))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.G);
                    }
                    if (buffArr.Contains(stripShield) || !buffArr.Contains(cpShield))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F);
                    }
                    if (buffArr.Contains(stripArmor) || !buffArr.Contains(cpArmor))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.J);
                    }
                    if (buffArr.Contains(stripHeadgear) || !buffArr.Contains(cpHelm))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.H);
                    }
                }

                if (archerBuffEnabled.Checked)
                {
                    int improveConcen = 3;
                    if (!buffArr.Contains(improveConcen))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F3);
                    }
                }

                if (autoNinja.Checked)
                {
                    int cicada = 206;
                    int ninjaAura = 208;
                    int agi = 12;
                    int blessing = 10;
                    int windConv = 901;
                    if (!buffArr.Contains(cicada))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F3);
                    }
                    if (!buffArr.Contains(ninjaAura))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F2);
                    }
                    if (!buffArr.Contains(blessing))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F5);
                    }
                    if (!buffArr.Contains(windConv))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.D5);
                    }
                }

                if (autoCloak.Checked)
                {
                    int cloaking = 5;
                    if (!buffArr.Contains(cloaking))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F2);
                    }
                }

                if (autoSinx.Checked)
                {
                    int edp = 114;
                    if (!buffArr.Contains(edp))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F3);
                    }
                }

                if (autoLK.Checked)
                {
                    int concentration = 1;
                    int spirit = 149;
                    int parry = 104;
                    int windConv = 901;
                    if (!buffArr.Contains(concentration))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F2);
                    }
                    if (!buffArr.Contains(spirit))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F5);
                    }
                    if (!buffArr.Contains(parry))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F3);
                    }
                    if (!buffArr.Contains(windConv))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.D5);
                    }
                }

                if (enableStarGlad.Checked)
                {
                    int spirit = 149;
                    int tumbling = 143;
                    int blessing = 10;
                    int increaseAgi = 12;
                    int dontForgetMe = 75;
                    if (!buffArr.Contains(spirit))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F7);
                    }
                    if (!buffArr.Contains(tumbling))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.D3);
                    }
                    if (!buffArr.Contains(blessing))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.K);
                    }
                    if (!buffArr.Contains(increaseAgi) && !buffArr.Contains(dontForgetMe))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.L);
                    }

                }

                if (enableWhitesmith.Checked)
                {
                    int greed = 1500;
                    int meltDown = 117;
                    int adrenaline = 23;
                    int weapPerfection = 24;
                    int maximizePower = 26;
                    int maxOverthrust = 188;
                    int crazyUproar = 30;
                    int cartBoost = 118;
                    int fullAdrenaline = 147;
                    int soulLink = 149;
                    int quagmire = 8;

                    if (!buffArr.Contains(soulLink))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.D9);
                    }

                    if (!buffArr.Contains(meltDown))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.D1);
                    }
                    if (!buffArr.Contains(adrenaline))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.D2);
                    }
                    if (!buffArr.Contains(weapPerfection))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.D3);
                    }
                    if (!buffArr.Contains(maximizePower))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.D4);
                    }
                    if (!buffArr.Contains(maxOverthrust))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.D5);
                    }
                    if (!buffArr.Contains(crazyUproar))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.D6);
                    }
                    if (!buffArr.Contains(cartBoost) && !buffArr.Contains(quagmire))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.D7);
                    }
                    if (!buffArr.Contains(fullAdrenaline) && buffArr.Contains(soulLink))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.D8);
                    }
                    if (!buffArr.Contains(greed))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F3);
                    }

                }

                if (sageBuffsEnabled.Checked)
                {
                    int defender = 62;
                    if (!buffArr.Contains(defender))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.D2);
                    }
                }

                if (enableChaseWalk.Checked)
                {
                    int chaseWalk = 149;
                    if (!buffArr.Contains(chaseWalk))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.F5);
                    }
                }

                if (enableRejectSword.Checked)
                {
                    int rejectSword = 120;
                    if (!buffArr.Contains(rejectSword))
                    {
                        KeyExtension.pressKey(actualProcess, (int)Keys.D8);
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
                pressBuff(actualProcess, strEnabled, buffArr, (int)ItemBuffs.StrFood, strFKey);
                pressBuff(actualProcess, agiEnabled, buffArr, (int)ItemBuffs.AgiFood, agiFKey);
                pressBuff(actualProcess, vitEnabled, buffArr, (int)ItemBuffs.VitFood, vitFKey);
                pressBuff(actualProcess, dexEnabled, buffArr, (int)ItemBuffs.DexFood, dexFKey);
                pressBuff(actualProcess, intEnabled, buffArr, (int)ItemBuffs.IntFood, intFKey);
                pressBuff(actualProcess, lukEnabled, buffArr, (int)ItemBuffs.LukFood, lukFKey);
                pressBuff(actualProcess, mdefEnabled, buffArr, (int)ItemBuffs.BigMdefPot, bMdefKey);
                pressBuff(actualProcess, defEnabled, buffArr, (int)ItemBuffs.BigDefPot, bDefKey);
                pressBuff(actualProcess, daehEnabled, buffArr, (int)ItemBuffs.Daehwandan, _daehKey);
                pressBuff(actualProcess, taeEnabled, buffArr, (int)ItemBuffs.Taecheongdan, _taeKey);
                pressBuff(actualProcess, concentEnabled, buffArr, (int)ItemBuffs.ConcentrationPot, conKey);
                pressBuff(actualProcess, awakeEnabled, buffArr, (int)ItemBuffs.Awakening, awakeningKey);
                pressBuff(actualProcess, anodyneEnabled, buffArr, (int)ItemBuffs.Anodyne, anodKey);
                pressBuff(actualProcess, botEnabled, buffArr, (int)ItemBuffs.BoxOfThunder, _botKey);

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
            for (int ctr = 0; ctr <= 240; ctr = ctr + 4)
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

        public static int RandomNumber()
        {
            Random random = new Random();
            return random.Next(1, 8) * 1500;
        }

        private void label39_Click(object sender, EventArgs e)
        {

        }

        private void spKeyBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
