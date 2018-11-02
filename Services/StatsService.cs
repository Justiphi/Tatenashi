using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Discord.Audio;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;

namespace Justibot.Services
{

    static class statsdict
    {
        public static ConcurrentDictionary<ulong, int> guildCommands = new ConcurrentDictionary<ulong, int>();
        public static int globalCommands;
        public static ConcurrentDictionary<ulong, int> guildInteracts = new ConcurrentDictionary<ulong, int>();
        public static int globalInteracts;
        public static ConcurrentDictionary<ulong, int> guildXP = new ConcurrentDictionary<ulong, int>();
        public static ulong globalXP;
        public static ConcurrentDictionary<ulong, ulong> guildmess = new ConcurrentDictionary<ulong, ulong>();
        public static ulong messagesSeen;
    }

    public class statsService
    {
        public static Tuple<int, ulong> getXp(ulong Id)
        {
            statsdict.guildXP.TryGetValue(Id, out int guildsXp);
            Tuple<int, ulong> Value = new Tuple<int, ulong>(guildsXp, statsdict.globalXP);
            return(Value);
        }

        public static Tuple<int, int> getInteracts(ulong Id)
        {
            statsdict.guildInteracts.TryGetValue(Id, out int guildsXp);
            Tuple<int, int> Value = new Tuple<int, int>(guildsXp, statsdict.globalInteracts);
            return(Value);
        }

        public static Tuple<int, int> getCommands(ulong Id)
        {
            statsdict.guildCommands.TryGetValue(Id, out int guildsXp);
            Tuple<int, int> Value = new Tuple<int, int>(guildsXp, statsdict.globalCommands);
            return(Value);
        }

        public static Tuple<ulong, ulong> getMessages(ulong Id)
        {
            statsdict.guildmess.TryGetValue(Id, out ulong guildsXp);
            Tuple<ulong, ulong> Value = new Tuple<ulong, ulong>(guildsXp, statsdict.messagesSeen);
            return(Value);
        }

        public static void messageRecieved(ulong guildi)
        {
            statsdict.messagesSeen = statsdict.messagesSeen+1;
            statsdict.guildmess.AddOrUpdate(guildi, 1, (k, v) => v=(v+1));
        }

        public static void interactRecieved(ulong guildi)
        {
            statsdict.globalInteracts = statsdict.globalInteracts+1;
            statsdict.guildInteracts.AddOrUpdate(guildi, 1, (k, v) => v=(v+1));
        }

        public static void commandRecieved(ulong guildi)
        {
            statsdict.globalCommands = statsdict.globalCommands+1;
            statsdict.guildCommands.AddOrUpdate(guildi, 1, (k, v) => v=(v+1));
        }

        public static void xpRecieved(ulong guildi, int xpadd)
        {
            ulong xps = Convert.ToUInt64(xpadd);
            statsdict.globalXP = statsdict.globalXP+xps;
            statsdict.guildXP.AddOrUpdate(guildi, xpadd, (k, v) => v=(v + xpadd));
        }
    }
}