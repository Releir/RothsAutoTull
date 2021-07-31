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
            (int)Debuffs.Silence
        };
        }
    }
}
