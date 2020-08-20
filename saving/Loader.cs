using System;
using System.Linq;
using Discord;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Discord.Commands;
using Discord.WebSocket;

namespace Justibot
{
    public class Loader
    {

        public static Tuple<bool, ulong, string> LoadPerm(IGuildUser user, string permTL)
        {
            string mode = "nill";
            bool permAct = false;
            ulong channel = user.Guild.DefaultChannelId;

            using (var context = new DataContext())
            {
                //Capture perms for current context
                var perms = context.ServerPerms.AsEnumerable()
                .Where(b => b.PServId.Equals(user.GuildId) && b.SPerm.Equals(permTL))
                .ToList();


                if (perms.Count() == 0)
                {
                    permAct = false;
                    channel = user.Guild.DefaultChannelId;
                    mode = "nill";
                }
                else
                {
                    permAct = perms.First().PermActive;
                    channel = perms.First().PermArg;
                    mode = perms.First().Pmode;
                }
            }
            var values = new Tuple<bool, ulong, string>(permAct, channel, mode);
            return (values);
        }

        public static Tuple<bool, ulong, string> LoadGiveaway(IGuildUser user)
        {
            string prizes = "nill";
            bool giveAct = false;
            ulong channel = user.Guild.DefaultChannelId;

            using (var context = new DataContext())
            {
                var giveaway = context.Givaways.AsEnumerable()
                .Where(b => b.GservID.Equals(user.GuildId))
                .ToList();

                List<string> test = context.Givaways.AsEnumerable().Select(x => x.prize).ToList();


                if (giveaway.Count() == 0)
                {
                    giveAct = false;
                    channel = user.Guild.DefaultChannelId;
                    prizes = "nill";
                }
                else
                {
                    giveAct = giveaway.First().GActive;
                    channel = giveaway.First().Gchannel;
                    prizes = giveaway.First().prize;
                }
            }
            var values = new Tuple<bool, ulong, string>(giveAct, channel, prizes);
            return (values);
        }

        public static ulong GetGHost(IGuildUser user)
        {
            ulong hostID = 1;
            using (var context = new DataContext())
            {
                var giveaway = context.Givaways.AsQueryable()
                    .Where(b => b.GservID.Equals(user.GuildId))
                    .Include(x => x.Entries)
                    .ToList();


                if (giveaway.Count() == 0)
                {
                    hostID = 1;
                }
                else
                {
                    var host = giveaway.First().Entries.Where(x => x.ishost).Select(x => x.entrentID);
                    hostID = host.FirstOrDefault();
                }
            }
            return (hostID);
        }

        public static ulong GetTHost(IGuildUser user)
        {
            ulong hostID = 1;
            using (var context = new DataContext())
            {
                var giveaway = context.Tournaments.AsEnumerable()
                    .Where(b => b.TservID.Equals(user.GuildId))
                    .ToList();


                if (giveaway.Count() == 0)
                {
                    hostID = 1;
                }
                else
                {
                    var host = giveaway.First();
                    hostID = host.Thost;
                }
            }
            return (hostID);
        }

        public static int entrycount(IGuildUser user)
        {
            int entcount = 0;
            using (var context = new DataContext())
            {
                var giveaway = context.Givaways.AsQueryable()
                    .Where(b => b.GservID.Equals(user.GuildId))
                    .Include(x => x.Entries)
                    .ToList();


                if (giveaway.Count() == 0)
                {
                    entcount = 0;
                }
                else
                {
                    var giving = giveaway.First();
                    entcount = giving.Entries.Count();
                }
            }

            return (entcount);
        }

        public static bool checkentry(IGuildUser user)
        {
            using (var context = new DataContext())
            {
                var giveaway = context.Givaways.AsQueryable()
                    .Where(b => b.GservID.Equals(user.GuildId))
                    .Include(x => x.Entries)
                    .First();

                var host = giveaway.Entries.Where(x => x.entrentID.Equals(user.Id)).ToList();

                if (host.Count() == (0))
                {
                    return (false);
                }
                else
                {
                    return (true);
                }
            }
        }

        public static ulong getWinner(IGuildUser user, int win)
        {
            using (var context = new DataContext())
            {
                var giveaway = context.Givaways.AsQueryable()
                    .Where(b => b.GservID.Equals(user.GuildId))
                    .Include(x => x.Entries)
                    .First();

                var gList = giveaway.Entries.Select(x => x.entrentID).ToArray();
                ulong host = gList[win];
                return (host);
            }
        }

        public static bool isStaff(IGuildUser user)
        {
            var userRoles = user.RoleIds.ToList();
            var Guild = user.Guild;
            bool userStaff = false;
            using (var context = new DataContext())
            {
                var staffoles = context.staffRoles.AsEnumerable()
                    .Where(b => b.RServId.Equals(user.GuildId))
                    .ToList();

                if (staffoles != null)
                {
                    foreach (Justibot.staffRole entry in staffoles)
                    {
                        foreach (ulong userRole in userRoles)
                        {
                            if (userRole == entry.PermArg)
                            {
                                userStaff = true;
                            }
                        }
                    }
                }

                return (userStaff);
            }
        }

        public static List<string> loadTracks(IGuildUser user)
        {
            using (var context = new DataContext())
            {
                var tracks = context.musics.AsEnumerable()
                    .Where(b => b.Tserver.Equals(user.GuildId))
                    .Select(x => x.Tname)
                    .ToList();

                return (tracks);
            }
        }

        public static int queueCount(IGuildUser user)
        {
            using (var context = new DataContext())
            {
                var tracks = context.musics.AsEnumerable()
                    .Where(b => b.Tserver.Equals(user.GuildId))
                    .Select(x => x.Tname)
                    .ToList();

                if (tracks.Count() == 0)
                {
                    return (0);
                }
                else
                {
                    return (tracks.Count());
                }
            }
        }
        public static bool hasServer(ulong guild)
        {
            using (var context = new DataContext())
            {
                var tracks = context.musics.AsEnumerable()
                    .Where(b => b.Tserver.Equals(guild))
                    .ToList();

                if (tracks.Count() == 0)
                {
                    return (false);
                }
                else
                {
                    return (true);
                }
            }
        }

        public static bool checkAdmin(ulong adminID)
        {
            using (var db = new DataContext())
            {
                var adminList = db.admins.AsEnumerable()
                    .Select(x => x.staffMember)
                    .ToList();

                if (adminList.Contains(adminID))
                {
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
        }

        public static KeyValuePair<DateTime, string> checkUpdate()
        {
            using(var db = new DataContext())
            {
                var versionCheker = db.versionChecks
                    .ToList();
                
                string version;
                DateTime date;
                if(versionCheker.Count() == 0)
                {
                    date = DateTime.Now;
                    version = "nill";
                }
                else
                {
                    date = versionCheker.First().time;
                    version = versionCheker.First().version;
                }
                
                return(new KeyValuePair<DateTime, string>(date, version));
            }
        }

        public static int usercount(ICommandContext Context)
        {
            int usercount = 0;
            foreach (var guild in (Context.Client as DiscordSocketClient).Guilds)
            {
                usercount += guild.MemberCount;
            }
            return (usercount);
        }

        public static List<string> loadpublicnote(IGuildUser user)
        {
            using (var context = new DataContext())
            {
                var content = context.notes.AsEnumerable()
                    .Where(b => (b.User.Equals(user.Id) && b.Type.Equals(1)))
                    .Select(x => $"({x.NoteID}) {Format.Bold($"{x.Name}:")} {x.Content}")
                    .ToList();
                
                if(content.Count() == 0)
                {
                    content.Add("No public notes found by this user.");
                }
                return (content);
            }
        }
        public static List<string> loadprivatenote(IGuildUser user)
        {
            using (var context = new DataContext())
            {
                var content = context.notes.AsEnumerable()
                    .Where(b => (b.User.Equals(user.Id) && b.Type.Equals(2)))
                    .Select(x => $"({x.NoteID}) {Format.Bold($"{x.Name}:")} {x.Content}")
                    .ToList();
                
                if(content.Count() == 0)
                {
                    content.Add("No public notes found by this user.");
                }
                return (content);
            }
        }

        public static List<string> loadstaffnote(IGuildUser user)
        {
            using (var context = new DataContext())
            {
                var content = context.notes.AsEnumerable()
                    .Where(b => (b.Type.Equals(4)) && b.nServId.Equals(user.GuildId))
                    .Select(x => $"({x.NoteID}) {Format.Bold($"{x.Name}:")} {x.Content}")
                    .ToList();

                if(content.Count() == 0)
                {
                    content.Add("No staff notes found.");
                }
                return (content);
            }
        }
        public static List<string> loadservernote(IGuildUser user)
        {
            using (var context = new DataContext())
            {
                var content = context.notes.AsEnumerable()
                    .Where(b => (b.Type.Equals(3)) && b.nServId.Equals(user.GuildId))
                    .Select(x => $"({x.NoteID}) {Format.Bold($"{x.Name}:")} {x.Content}")
                    .ToList();

                if(content.Count() == 0)
                {
                    content.Add("No server notes found.");
                }
                return (content);
            }
        }

        public static int getTload(IGuildUser user)
        {
            using (var context = new DataContext())
            {
                var content = context.Tournaments.AsEnumerable()
                    .Where(b => b.TservID.Equals(user.GuildId))
                    .ToList();

                int content2 = 0;

                if(content.Count() == 0)
                {
                    content2 = 0 ;
                }
                else if(content.First().joinable)
                {
                    content2 = 2;
                }
                else
                {
                    content2 = 1;
                }
                return (content2);
            }
        }

        public static ulong getRoleMessage(IGuildUser user)
        {
            using(var context = new DataContext())
            {
                var content = context.roleMessages.AsEnumerable()
                    .Where(x => x.guildId.Equals(user.GuildId))
                    .ToList();

                if(content.Count() == 0)
                {
                    return(0);
                } 
                else
                {
                    return content.First().MessageId;
                }
            }
        }

        public static ulong getRole(IGuildUser user, string reaction)
        {
            using(var context = new DataContext())
            {
                var content = context.roleReactions.AsEnumerable()
                    .Where(x => x.guildId.Equals(user.GuildId) && x.reaction.ToString().Equals(reaction.ToString()))
                    .ToList();

                if(content.Count() == 0)
                {
                    return(0);
                } 
                else
                {
                    return content.Select(x => x.roleId).First();
                }
            }
        }

        public static Dictionary<ulong, string> LoadReactionRoles(ulong guildId)
        {
            using(var context = new DataContext())
            {
                var returnDict = new Dictionary<ulong, string>();

                var roles = context.roleReactions.AsEnumerable()
                    .Where(x => x.guildId.Equals(guildId)).ToList();

                foreach(var role in roles)
                {
                    returnDict.Add(role.roleId, role.reaction);
                }
                return returnDict;
            }
        }

    }
}
