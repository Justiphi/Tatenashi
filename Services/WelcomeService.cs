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

    static class welcomedict
    {
        public static ConcurrentDictionary<ulong, string> welcomes = new ConcurrentDictionary<ulong, string>();
        public static ConcurrentDictionary<ulong, string> leaves = new ConcurrentDictionary<ulong, string>();
    }

    public class WelcomeService
    {
        public static void LoadWelcomes(DiscordSocketClient Client)
        {
            string prefix2;
            foreach (var guild in (Client as DiscordSocketClient).Guilds)
            {
                prefix2 = checkWelcomes(guild);
                welcomedict.welcomes.AddOrUpdate(guild.Id, prefix2, (k,v) => prefix2);
            }
        }

        public static string checkWelcomes(SocketGuild guild)
        {
            string prefix;
            using (var context = new DataContext())
            {
                var perms2 = context.welcomeMessages
                    .Where(b => b.welcomeGuild.Equals(guild.Id))
                    .ToList();
                if (perms2.Count != 0)
                {
                    var perms = perms2.First();

                    if (perms.message == null)
                    {
                        prefix = "Welcome, **[user]** has joined **[server]!!!** \n" +
                    "Have a good time!!!";
                    }
                    else
                    {
                        prefix = perms.message;
                    }

                    return (prefix);
                }
                return ("Welcome, **[user]** has joined **[server]!!!** \n" +
                    "Have a good time!!!");

            }
        }

        public static void addWelcomes(IGuild guild, string message)
        {
            welcomedict.welcomes.AddOrUpdate(guild.Id, message, (k,v) => message);
        }

    }


    public class LeavingService
    {
        public static void Loadleaving(DiscordSocketClient Client)
        {
            string prefix2;
            foreach (var guild in (Client as DiscordSocketClient).Guilds)
            {
                prefix2 = checkLeaves(guild);
                welcomedict.leaves.AddOrUpdate(guild.Id, prefix2, (k,v) => prefix2);
            }
        }

        public static string checkLeaves(SocketGuild guild)
        {
            string prefix;
            using (var context = new DataContext())
            {
                var perms2 = context.leavingMessages
                    .Where(b => b.leavingGuild.Equals(guild.Id))
                    .ToList();
                if (perms2.Count != 0)
                {
                    var perms = perms2.First();

                    if (perms.message == null)
                    {
                        prefix = "**[user]** has left **[server]**, goodbye.";
                    }
                    else
                    {
                        prefix = perms.message;
                    }

                    return (prefix);
                }
                return ("**[user]** has left **[server]**, goodbye.");

            }
        }

        public static void addLeaves(IGuild guild, string message)
        {
            welcomedict.leaves.AddOrUpdate(guild.Id, message, (k,v) => message);
        }

    }
}