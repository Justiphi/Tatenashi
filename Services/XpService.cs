using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Discord.Audio;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Justibot.Services
{

    static class xpdict
    {
        public static ConcurrentDictionary<KeyValuePair<ulong, ulong>, int> Localxp = new ConcurrentDictionary<KeyValuePair<ulong, ulong>, int>();
        public static Timer autotimer;
        public static ConcurrentDictionary<ulong, int> serverxp = new ConcurrentDictionary<ulong, int>();
        public static ConcurrentDictionary<ulong, int> globalxp = new ConcurrentDictionary<ulong, int>();
        public static ConcurrentDictionary<Tuple<ulong, ulong>, int> userxp = new ConcurrentDictionary<Tuple<ulong, ulong>, int>();
        public static ConcurrentDictionary<Tuple<ulong, ulong>, Tuple<int, int>> reward = new ConcurrentDictionary<Tuple<ulong, ulong>, Tuple<int, int>>();
        public static List<ulong> xpblacklist = new List<ulong>();
    }


    public class XpService
    {

        public static void addReward(IRole role, IGuildUser user, int lvl)
        {
            xpdict.reward.AddOrUpdate(new Tuple<ulong, ulong>(user.Guild.Id, role.Id), new Tuple<int, int>(lvl, checkXp(lvl)), (k, v) => new Tuple<int, int>(lvl, checkXp(lvl)));
        }

        public static void removeReward(IRole role, IGuildUser user)
        {
            xpdict.reward.TryRemove(new Tuple<ulong, ulong>(user.Guild.Id, role.Id), out var rubbish);
        }

        public static void timerStart()
        {
            int i = 0;
            if (i == 0)
            {
                AutoResetEvent _autoEvent = new AutoResetEvent(true);
                xpdict.autotimer = new Timer(clearDict, _autoEvent, 60000, 60000);
                i = 1;
            }
        }

        public static void clearDict(Object stateInfo)
        {
            xpdict.Localxp.Clear();
        }

        public static List<Tuple<ulong, int>> rewardlist(IGuildUser user)
        {
            var rewards = new List<Tuple<ulong, int>>();
            var checking = xpdict.reward.Where(y => y.Key.Item1 == user.Guild.Id).ToList();
            foreach (var reward in checking.OrderBy(x => x.Value.Item1))
            {
                rewards.Add(new Tuple<ulong, int>(reward.Key.Item2, reward.Value.Item1));
            }
            return (rewards);
        }

        public static List<Tuple<ulong, int>> rewardslist(IGuildUser user)
        {
            var rewards = new List<Tuple<ulong, int>>();
            var checking = xpdict.reward.Where(y => y.Key.Item1 == user.Guild.Id).ToList();
            foreach (var reward in checking.OrderBy(x => x.Value.Item1))
            {
                rewards.Add(new Tuple<ulong, int>(reward.Key.Item2, reward.Value.Item2));
            }
            return (rewards);
        }

        public static void AddXp(IGuildUser user, IGuild guild)
        {
            if (!xpdict.xpblacklist.Contains(guild.Id))
            {
                var key = new KeyValuePair<ulong, ulong>(guild.Id, user.Id);
                if (!xpdict.Localxp.ContainsKey(key))
                {
                    var exp = Justibot.Loader.LoadPerm(user, "EXP");
                    xpdict.Localxp.AddOrUpdate(key, 1, (k, v) => 1);
                    int addxp = Data.rand.Next(11, 17);

                    int gxp = 0;
                    using (var context = new DataContext())
                    {
                        var perms2 = context.globalusersxp.AsQueryable()
                            .Where(x => x.User == user.Id)
                            .ToList();
                        if (perms2.Count() != 0)
                        {
                            gxp = perms2.First().Xp;
                        }
                        else
                        {
                            gxp = 0;
                        }
                    }
                    xpdict.globalxp.AddOrUpdate(user.Id, gxp + addxp, (k, v) => gxp + addxp);
                    Justibot.Saver.addglobalxp(user, (gxp + addxp));
                    statsService.xpRecieved(guild.Id, addxp);


                    if (exp.Item1 == true)
                    {
                        int servexp = 0;
                        int usexp = 0;

                        using (var context = new DataContext())
                        {
                            var perms2 = context.serverusersxp.AsQueryable()
                                .Where(x => (x.User == user.Id) && (x.Server.xServId == guild.Id))
                                .ToList();
                            if (perms2.Count() != 0)
                            {
                                usexp = perms2.First().Xp;
                            }
                            else
                            {
                                usexp = 0;
                            }
                        }

                        using (var context = new DataContext())
                        {
                            var perms2 = context.serversxp.AsQueryable()
                                .Where(x => x.xServId == guild.Id)
                                .ToList();
                            if (perms2.Count() != 0)
                            {
                                servexp = perms2.First().Xp;
                            }
                            else
                            {
                                servexp = 0;
                            }
                        }

                        xpdict.serverxp.AddOrUpdate(guild.Id, servexp + addxp, (k, v) => servexp + addxp);
                        Tuple<ulong, ulong> val = new Tuple<ulong, ulong>(guild.Id, user.Id);
                        xpdict.userxp.AddOrUpdate(val, usexp + addxp, (k, v) => usexp + addxp);

                        Justibot.Saver.AddingXp(user, (servexp + addxp), (usexp + addxp));

                        levels(user);
                    }
                }
            }
        }

        public static void plusXp(IGuildUser user, int xp)
        {
            xpdict.globalxp.AddOrUpdate(user.Id, xp, (k, v) => v + xp);
            xpdict.globalxp.TryGetValue(user.Id, out int gxp);
            Justibot.Saver.addglobalxp(user, gxp);
        }

        public static async Task<Dictionary<ulong, int>> serverLeads(ICommandContext context)
        {
            var checking = xpdict.serverxp.OrderByDescending(x => x.Value).ToList();
            Dictionary<ulong, int> prefix = new Dictionary<ulong, int>();
            foreach (KeyValuePair<ulong, int> pair in checking)
            {
                var user = (await context.Client.GetGuildAsync(pair.Key));
                if (user != null)
                {
                    prefix.Add(user.Id, pair.Value);
                    if(prefix.Count == 10)
                    {
                        return(prefix);
                    }
                }
            }
            return (prefix);
        }
        public static async Task<Dictionary<ulong, int>> userLeads(ICommandContext context)
        {
            var checking = xpdict.userxp.Where(x => x.Key.Item1 == context.Guild.Id).ToList();
            var checking2 = checking.OrderByDescending(x => x.Value).ToList();
            Dictionary<ulong, int> prefix = new Dictionary<ulong, int>();
            foreach (KeyValuePair<Tuple<ulong, ulong>, int> pair in checking2)
            {
                var user = (await context.Guild.GetUserAsync(pair.Key.Item2));
                if (user != null)
                {
                    prefix.Add(user.Id, pair.Value);
                    if(prefix.Count == 10)
                    {
                        return(prefix);
                    }
                }
            }
            return (prefix);
        }

        public static async Task<Dictionary<ulong, int>> globalLeads(ICommandContext context)
        {
            var checking2 = xpdict.globalxp.ToList();
            var checking = checking2.OrderByDescending(x => x.Value).ToList();
            Dictionary<ulong, int> prefix = new Dictionary<ulong, int>();
            var guildys = (context.Client as DiscordSocketClient).Guilds;
            foreach (KeyValuePair<ulong, int> pair in checking)
            {
                IGuildUser user = null;
                foreach (var g in guildys)
                {
                    user = (await (g as IGuild).GetUserAsync(pair.Key));
                    if (user != null)
                    {
                        break;
                    }
                }
                if (user != null)
                {
                    prefix.Add(user.Id, pair.Value);
                    if(prefix.Count == 10)
                    {
                        return(prefix);
                    }
                }
            }
            return (prefix);
        }

        public static KeyValuePair<string, int> getuserxp(IGuildUser user)
        {
            var checking = xpdict.userxp.OrderByDescending(x => x.Value).ToList();
            var userxps = checking.Where(x => x.Key.Item1.Equals(user.Guild.Id)).ToList(); ;
            string place = $"0/{userxps.Count}";
            int usingxp = 0;
            for (int i = 0; i < userxps.Count; i++)
            {
                if (userxps[i].Key.Item2 == user.Id)
                {
                    place = $"{i + 1}/{userxps.Count}";
                    usingxp = userxps[i].Value;
                }
            }
            KeyValuePair<string, int> result = new KeyValuePair<string, int>(place, usingxp);
            return (result);
        }

        public static KeyValuePair<string, int> getglobalxp(IGuildUser user)
        {
            var checking = xpdict.globalxp.OrderByDescending(x => x.Value).ToList();
            var userxps = checking.Where(x => x.Key.Equals(user.Id)).First().Value;
            string place = $"0/{checking.Count}";
            for (int i = 0; i < checking.Count; i++)
            {
                if (checking[i].Key == user.Id)
                {
                    place = $"{i + 1}/{checking.Count}";
                }
            }
            KeyValuePair<string, int> result = new KeyValuePair<string, int>(place, userxps);
            return (result);
        }

        public static KeyValuePair<string, int> getserverxp(IGuild guild)
        {
            var checking = xpdict.serverxp.OrderByDescending(x => x.Value).ToList();
            var userxps = checking.Where(x => x.Key.Equals(guild.Id)).First().Value;
            string place = $"0/{checking.Count}";
            for (int i = 0; i < checking.Count; i++)
            {
                if (checking[i].Key == guild.Id)
                {
                    place = $"{i + 1}/{checking.Count}";
                }
            }
            KeyValuePair<string, int> result = new KeyValuePair<string, int>(place, userxps);
            return (result);
        }

        public static void levels(IGuildUser user)
        {
            var rewards = rewardslist(user);
            var xp = xpdict.userxp.Where(b => b.Key.Item1 == user.Guild.Id && b.Key.Item2 == user.Id).ToList();
            foreach (var reward in rewards)
            {
                if (reward.Item2 <= xp.First().Value)
                {
                    IRole award = user.Guild.GetRole(reward.Item1);
                    user.AddRoleAsync(award);
                }
            }
        }

        public static void Loadxps(DiscordSocketClient Client)
        {
            using (var context = new DataContext())
            {
                var perms2 = context.serversxp
                    .Include(x => x.usersXp)
                    .ToList();

                foreach (serverxp servsxp in perms2)
                {
                    xpdict.serverxp.AddOrUpdate(servsxp.xServId, servsxp.Xp, (k, v) => servsxp.Xp);
                    var usexp = servsxp.usersXp.ToList();
                    foreach (var guildusers in usexp)
                    {
                        Tuple<ulong, ulong> val = new Tuple<ulong, ulong>(servsxp.xServId, guildusers.User);
                        xpdict.userxp.AddOrUpdate(val, guildusers.Xp, (k, v) => guildusers.Xp);
                    }
                }
                var perm3 = context.globalusersxp
                    .ToList();

                foreach (var user in perm3)
                {
                    xpdict.globalxp.AddOrUpdate(user.User, user.Xp, (k, v) => user.Xp);
                }

                var perm4 = context.rewards
                    .ToList();

                foreach (var ward in perm4)
                {
                    xpdict.reward.AddOrUpdate(new Tuple<ulong, ulong>(ward.rserver, ward.rewardrole), new Tuple<int, int>(ward.rewardlvl, checkXp(ward.rewardlvl)), (k, v) => new Tuple<int, int>(ward.rewardlvl, checkXp(ward.rewardlvl)));
                }

                var perm5 = context.blacklists
                    .ToList();

                foreach (var server in perm5)
                {
                    xpdict.xpblacklist.Add(server.bserver);
                }
                foreach (ulong server in Settings.defaultblacklist)
                {
                    xpdict.xpblacklist.Add(server);
                }
            }
        }

        public static KeyValuePair<int, int> checkLevel(int xp)
        {
            double start = 100;
            int level = 0;
            while (start < xp)
            {
                start = (start * 1.4);
                level += 1;
            }
            return (new KeyValuePair<int, int>(level, Convert.ToInt32(start)));
        }

        public static int checkXp(int lvl)
        {
            double start = 100;
            for (int i = 1; i < lvl; i++)
            {
                start = (start * 1.4);
            }
            return (Convert.ToInt32(start));
        }

        public static void resetserver(IGuild guild)
        {
            foreach (var entry in xpdict.userxp.Where(x => x.Key.Item1.Equals(guild.Id)))
            {
                xpdict.userxp.TryRemove(entry.Key, out int removedValue);
            }
        }

        public static void wipeserver(IGuild guild)
        {
            resetserver(guild);
            xpdict.serverxp.TryRemove(guild.Id, out int removedValue);
        }


    }
}