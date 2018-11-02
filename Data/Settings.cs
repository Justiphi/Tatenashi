using System;
using System.Collections.Generic;

namespace Justibot
{
    public class Settings
    {
        public static List<ulong> bunker = new List<ulong>();

        public static int maxUserNotes = 10;
        public static int maxStaffNotes = 20;
        public static int maxServerNotes = 30;
        public static int maxNoteLength = 150;
        public static int maxNameLength = 25;
        

        public static ulong[] defaultblacklist =
        {
            110373943822540800,
            264445053596991498
        };
    }
}