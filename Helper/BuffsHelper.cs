using System.Collections.Generic;

namespace BuffHelper
{
    public static class DebuffExtension
    {
        public static List<int> getPanaceaStatus()
        {
            return new List<int>()
        {
            (int)Debuffs.Curse,
            (int)Debuffs.Silence,
            (int)Debuffs.Poison,
            (int)Debuffs.Confuse,
            (int)Debuffs.Blind,
            (int)Debuffs.Hallucination
        };
        }
        public static List<int> getPeonyStatus()
        {
            return new List<int>()
        {
            (int)Debuffs.Burn,
            (int)Debuffs.Burning,
            (int)Debuffs.Fear
        };
        }

    }
}
