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

    static class prefixdict
    {
        public static ConcurrentDictionary<ulong, char> prefixes = new ConcurrentDictionary<ulong, char>();
    }

    public class PrefixService
    {
        public static void LoadPrefixs(DiscordSocketClient Client)
        {
            char prefix2;
            foreach (var guild in (Client as DiscordSocketClient).Guilds)
            {
                prefix2 = checkPrefix(guild);
                prefixdict.prefixes.AddOrUpdate(guild.Id, prefix2, (k,v) => prefix2);
            }
        }

        

        public static char getPrefix(ulong guild)
        {
            char prefix;
            if (prefixdict.prefixes.ContainsKey(guild))
            {
                prefixdict.prefixes.TryGetValue(guild, out prefix);
            }
            else
            {
                prefix = '+';
            }

            return (prefix);
        }

        public char getPrefix2(ulong guild)
        {
            char prefix;
            if (prefixdict.prefixes.ContainsKey(guild))
            {
                prefixdict.prefixes.TryGetValue(guild, out prefix);
            }
            else
            {
                prefix = '+';
            }

            return (prefix);
        }

        public static void addPrefixes(IGuild guild, char prefix2)
        {
            prefixdict.prefixes.AddOrUpdate(guild.Id, prefix2, (k,v) => prefix2);
        }



        public static char checkPrefix(SocketGuild guild)
        {
            char prefix;
            using (var context = new DataContext())
            {
                var perms2 = context.prefixes
                    .Where(b => b.prefixGuild.Equals(guild.Id))
                    .ToList();
                if (perms2.Count != 0)
                {
                    var perms = perms2.First();

                    if (perms.prefix == '\0')
                    {
                        prefix = '+';
                    }
                    else
                    {
                        prefix = perms.prefix;
                    }

                    return (prefix);
                }
                return ('+');

            }
        }
    }
}