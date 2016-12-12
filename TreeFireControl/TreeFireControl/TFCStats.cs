using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeFireControl
{
    internal class TFCStats
    {
        public ulong totalburncalls = 0;
        public ulong totalburncallsdisaster = 0;
        public ulong totalburncallsnormal = 0;
        public ulong totalburncallsblockednormal = 0;
        public ulong totalburncallsblockeddisaster = 0;

        public void clearstats()
        {
            totalburncalls = 0;
            totalburncallsdisaster = 0;
            totalburncallsnormal = 0;
            totalburncallsblockednormal = 0;
            totalburncallsblockeddisaster = 0;

        }
    }


}
