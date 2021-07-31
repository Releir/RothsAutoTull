using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Collections.Generic;
using RothsAutoTull;

namespace KeyHelper
{
    public static class KeyExtension
    {
        public static List<String> keyComboList = new List<String>()
            {
                "hpKeyBox",
                "spKeyBox",
                "panaceaBox",
                "fireProofKey",
                "coldProofKey",
                "thunderProofKey",
                "earthProofKey",
                "speedKey",
                "cursedKey",
                "aloeKey",
                "resentKey",
                "holyKey",
                "shieldKey",
                "weaponKey",
                "upperHgKey",
                "armorKey"
            };

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        const uint WM_KEYDOWN = 0x100;
        const uint WM_KEYUP = 0x101;

        public static void pressKey(Process proc, int keybind)
        {
            PostMessage(proc.MainWindowHandle, WM_KEYDOWN, keybind, 0);
            PostMessage(proc.MainWindowHandle, WM_KEYUP, keybind, 0);
        }

        public static void loadConfigKey(ComboBox cb)
        {
            cb.SelectedIndex = cb.FindString(ConfigurationManager.AppSettings.Get(cb.Name));
        }

        public static void loadKeys(ComboBox box)
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
                   {Keys.D1, "1"},
                   {Keys.D2, "2"},
                   {Keys.D3, "3"},
                   {Keys.D4, "4"},
                   {Keys.D5, "5"},
                   {Keys.D6, "6"},
                   {Keys.D7, "7"},
                   {Keys.D8, "8"},
                   {Keys.D9, "9"},
                   {Keys.Q, "Q"},
                   {Keys.W, "W"},
                   {Keys.E, "E"},
                   {Keys.R, "R"},
                   {Keys.T, "T"},
                   {Keys.Y, "Y"},
                   {Keys.U, "U"},
                   {Keys.I, "I"},
                   {Keys.O, "O"},
                   {Keys.A, "A"},
                   {Keys.S, "S"},
                   {Keys.D, "D"},
                   {Keys.F, "F"},
                   {Keys.G, "G"},
                   {Keys.H, "H"},
                   {Keys.J, "J"},
                   {Keys.K, "K"},
                   {Keys.L, "L"},


            };
            box.DataSource = new BindingSource(keyboardDictionary, null);
            box.DisplayMember = "Value";
            box.ValueMember = "Key";
        }
    }
}