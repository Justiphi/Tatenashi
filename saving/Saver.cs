using Discord;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace Justibot
{
    public class Saver
    {

        public static void SavePerm(IGuildUser user, string permTC, bool activ, ulong args, string mode)
        {
            using (var db = new DataContext())
            {

                var perms = db.ServerPerms
                    .Where(b => b.PServId.Equals(user.GuildId) && b.SPerm.Equals(permTC))
                    .ToList();

                if(perms.Count == 0)
                {
                    ulong serverID = user.GuildId;
                    var permTS = new ServerPerm { PServId = serverID, SPerm = permTC, PermActive = activ, PermArg = args, Pmode = mode }; 
                    db.ServerPerms.Add(permTS);
                    db.SaveChangesAsync();                    
                }
                else
                {
                    var EPerm = perms.First();
                    EPerm.PermActive = activ;
                    EPerm.PermArg = args;
                    EPerm.Pmode = mode;
                    db.SaveChangesAsync();
                }
            }
            return;
        }

        public static void addVersion(DateTime dater, string version)
        {
            using (var db = new DataContext())
            {
                var giveaway = db.versionChecks
                    .ToList();

                if(giveaway.Count == 0)
                {
                    versionControl control = new versionControl{time = dater, version = version};
                    db.Add(control);
                    db.SaveChangesAsync();
                }
                else
                {
                    var usersxps = giveaway.First();
                    usersxps.time = dater;
                    usersxps.version = version;
                    db.Update(usersxps);
                    db.SaveChangesAsync();
                }
            }
            return;   
        }

        public static void SaveGiveaway(IGuildUser user, string prizes, bool activ, ulong args)
        {
            ulong serverID = user.GuildId;
            using (var db = new DataContext())
            {

                var perms = db.Givaways
                    .Where(b => b.GservID.Equals(user.GuildId))
                    .ToList();

                if(perms.Count == 0)
                {
                    var permTS = new Giveaway { GservID = serverID, prize = prizes, GActive = activ, Gchannel = args,}; 
                    db.Givaways.Add(permTS);
                    db.SaveChangesAsync();

                    var giving = db.Givaways.Where(b => b.GservID == serverID).Include(x => x.Entries).First();
                    var giver = new Entry{ entrentID = user.Id, ishost = true, Giveaway = giving};
                    giving.Entries.Add(giver);
                    db.SaveChangesAsync();                    
                }
                else
                {
                    var EPerm = perms.First();
                    EPerm.GActive = activ;
                    EPerm.Gchannel = args;
                    EPerm.prize = prizes;
                    db.SaveChangesAsync();
                    
                    var giving = db.Givaways.Where(b => b.GservID == serverID).Include(x => x.Entries).First();
                    Entry host = new Entry{ entrentID = user.Id, ishost = true, Giveaway = giving};
                    giving.Entries.Add(host);
                    db.SaveChangesAsync();
                    
                }
            }
            return;
        }

        public static void enterGiveaway(IGuildUser user)
        {
            ulong serverID = user.Guild.Id;
            using (var db = new DataContext())
            {
                var perms = db.Givaways
                    .Where(b => b.GservID.Equals(user.GuildId))
                    .ToList();

                if(perms.Count == 0)
                {
                    return;                
                }
                else
                {
                    var giving = db.Givaways.Where(b => b.GservID == serverID).Include(x => x.Entries).First();
                    Entry host = new Entry{ entrentID = user.Id, ishost = false, Giveaway = giving};
                    giving.Entries.Add(host);
                    db.SaveChangesAsync();
                }
            }
            return;
        }

        public static void clearEntries(IGuildUser user)
        {
            ulong serverID = user.Guild.Id;
            using (var db = new DataContext())
            {
                var giveaway = db.Givaways
                    .Where(b => b.GservID.Equals(user.GuildId))
                    .Include(x => x.Entries)
                    .ToList();

                if(giveaway.Count == 0)
                {
                    return;                
                }
                else
                {
                    var EPerm = db.Givaways.Where(b => b.GservID == serverID).Include(x => x.Entries).First();
                    if(EPerm.Entries.Count > 1)
                    {
                        db.RemoveRange(EPerm.Entries.Where(b => b.ishost == false));
                        db.SaveChangesAsync();
                    }
                }
            }
            return;   
        }

        public static void addglobalxp(IGuildUser user, int xp)
        {
            ulong serverID = user.Guild.Id;
            using (var db = new DataContext())
            {
                var giveaway = db.globalusersxp
                    .Where(b => b.User.Equals(user.Id))
                    .ToList();

                if(giveaway.Count == 0)
                {
                    globaluserxp server = new globaluserxp{User = user.Id, Xp = xp};
                    db.Add(server);
                    db.SaveChangesAsync();
                }
                else
                {
                    var usersxps = giveaway.First();
                    usersxps.Xp = xp;
                    db.Update(usersxps);
                    db.SaveChangesAsync();
                }
            }
            return;   
        }

        public static void AddingXp(IGuildUser user, int sxp, int uxp)
        {
            ulong serverID = user.GuildId;
            using (var db = new DataContext())
            {

                var perms = db.serversxp
                    .Where(b => b.xServId.Equals(user.GuildId))
                    .Include(x => x.usersXp)
                    .ToList();

                if(perms.Count == 0)
                {
                    var permTS = new serverxp { xServId = serverID, Xp = sxp}; 
                    db.serversxp.Add(permTS);
                    db.SaveChangesAsync();

                    var giving = db.serversxp.Where(b => b.xServId == serverID).Include(x => x.usersXp).First();
                    var giver = new serveruserxp{ User = user.Id, Xp = uxp};
                    giving.usersXp.Add(giver);
                    db.SaveChangesAsync();                    
                }
                else
                {
                    var EPerm = perms.First();
                    EPerm.Xp = sxp;
                    db.Update(EPerm);
                    db.SaveChangesAsync();
                    
                    var giving2 = EPerm.usersXp.Where(x => x.User == user.Id).ToList();
                    if(giving2.Count == 0)
                    {
                        serveruserxp host = new serveruserxp{ User = user.Id, Xp = uxp};
                        EPerm.usersXp.Add(host);
                        db.SaveChangesAsync();
                    }
                    else
                    {
                        serveruserxp usersx = giving2.First();
                        usersx.Xp = uxp;
                        db.Update(usersx);
                        db.SaveChangesAsync();
                    }
                    
                }
            }
            return;
        }

        public static void resetxp(IGuildUser user)
        {
            ulong serverID = user.Guild.Id;
            using (var db = new DataContext())
            {
                var perms = db.serversxp
                    .Where(b => b.xServId.Equals(user.GuildId))
                    .Include(b => b.usersXp)
                    .ToList();
                
                if(perms.Count != 0)
                {
                    var servers = perms.First();
                    db.RemoveRange(servers.usersXp.ToList());
                    db.Remove(servers);
                    db.SaveChangesAsync();
                }
                else
                {
                    return;
                }
            }
            return;
        }

        public static void clearxp(IGuildUser user)
        {
            ulong serverID = user.Guild.Id;
            using (var db = new DataContext())
            {
                var perms = db.serversxp
                    .Where(b => b.xServId.Equals(user.GuildId))
                    .Include(b => b.usersXp)
                    .ToList();
                
                if(perms.Count != 0)
                {
                    var servers = perms.First();
                    db.RemoveRange(servers.usersXp.ToList());
                    db.SaveChangesAsync();
                }
                else
                {
                    return;
                }
            }
            return;
        }

        public static void wipexp()
        {
            using (var db = new DataContext())
            {
                var perms = db.serverusersxp
                    .ToList();
                
                if(perms.Count != 0)
                {
                    db.RemoveRange(perms);
                    db.SaveChangesAsync();
                }
            }
            using (var db = new DataContext())
            {
                var perms = db.serversxp
                    .ToList();
                
                if(perms.Count != 0)
                {
                    db.RemoveRange(perms);
                    db.SaveChangesAsync();
                }
            }
            using (var db = new DataContext())
            {
                var perms = db.globalusersxp
                    .ToList();
                
                if(perms.Count != 0)
                {
                    db.RemoveRange(perms);
                    db.SaveChangesAsync();
                }
            }
            return;
        }

        public static void saveReward(IGuildUser user, ulong role, int level)
        {
            using (var db = new DataContext())
            {

                var perms = db.rewards
                    .Where(b => b.rserver.Equals(user.GuildId) && b.rewardrole.Equals(role))
                    .ToList();

                if(perms.Count == 0)
                {
                    ulong serverID = user.GuildId;
                    var permTS = new reward { rserver = serverID, rewardrole = role, rewardlvl = level }; 
                    db.rewards.Add(permTS);
                    db.SaveChangesAsync();                    
                }
                else
                {
                    var EPerm = perms.First();
                    EPerm.rewardlvl = level;
                    db.SaveChangesAsync();
                }
            }
            return;
        }

        public static void blacklist(IGuildUser user, ulong role)
        {
            using (var db = new DataContext())
            {

                var perms = db.blacklists
                    .Where(b => b.bserver.Equals(user.GuildId))
                    .ToList();

                if(perms.Count == 0)
                {
                    ulong serverID = user.GuildId;
                    var permTS = new blacklist { bserver = serverID}; 
                    db.blacklists.Add(permTS);
                    db.SaveChangesAsync();                    
                }
                else
                {
                    var EPerm = perms.First();
                    EPerm.bserver = user.GuildId;
                    db.SaveChangesAsync();
                }
            }
            return;
        }

        public static void unblacklist(IGuildUser user, ulong role)
        {
            using (var db = new DataContext())
            {

                var perms = db.blacklists
                    .Where(b => b.bserver.Equals(user.GuildId))
                    .ToList();

                if(perms.Count == 0)
                {
                }
                else
                {
                    var EPerm = perms.First();
                    db.Remove(EPerm);
                    db.SaveChangesAsync();
                }
                return;
            }
        }

        public static bool deleteReward(IGuildUser user, ulong role)
        {
            using (var db = new DataContext())
            {

                var perms = db.rewards
                    .Where(b => b.rserver.Equals(user.GuildId) && b.rewardrole.Equals(role))
                    .ToList();

                if(perms.Count == 0)
                {
                    return(false);
                }
                else
                {
                    var EPerm = perms.First();
                    db.Remove(EPerm);
                    db.SaveChangesAsync();
                    return(true);
                }
            }
        }

        public static void clearAllEntries(IGuildUser user)
        {
            using (var db = new DataContext())
            {
                var giveaway = db.Givaways
                    .Where(b => b.GservID.Equals(user.GuildId))
                    .Include(x => x.Entries)
                    .ToList();

                if(giveaway.Count == 0)
                {
                    return;                
                }
                else
                {
                    var EPerm = giveaway.First();
                    db.RemoveRange(EPerm.Entries);
                    db.SaveChangesAsync();
                }
            }
            return;   
        }


        public static void saveRole(IGuildUser user, ulong args)
        {
            using (var db = new DataContext())
            {

                var perms = db.staffRoles
                    .Where(b => b.RServId.Equals(user.GuildId) && b.PermArg.Equals(args))
                    .ToList();

                if(perms.Count == 0)
                {
                    ulong serverID = user.GuildId;
                    var permTS = new staffRole { RServId = serverID, PermArg = args }; 
                    db.staffRoles.Add(permTS);
                    db.SaveChangesAsync();                    
                }
                else
                {
                    var EPerm = perms.First();
                    EPerm.PermArg = args;
                    db.SaveChangesAsync();
                }
            }
            return;
        }

        public static bool deleteRole(IGuildUser user, ulong args)
        {
            using (var db = new DataContext())
            {

                var perms = db.staffRoles
                    .Where(b => b.RServId.Equals(user.GuildId) && b.PermArg.Equals(args))
                    .ToList();

                if(perms.Count == 0)
                {
                    return(false);
                }
                else
                {
                    var EPerm = perms.First();
                    db.Remove(EPerm);
                    db.SaveChangesAsync();
                    return(true);
                }
            }
        }

        public static void saveTrack(IGuildUser user, string track)
        {
            using (var db = new DataContext())
            {

                var perms = db.musics
                    .Where(b => b.Tserver.Equals(user.GuildId) && b.Tname.Equals(track))
                    .ToList();

                if(perms.Count == 0)
                {
                    ulong serverID = user.GuildId;
                    var permTS = new music { Tserver = serverID, Tname = track }; 
                    db.musics.Add(permTS);
                    db.SaveChangesAsync();                    
                }
            }
            return;
        }
        
        public static void deleteTrack(IGuildUser user, string track)
        {
            using (var db = new DataContext())
            {

                var perms = db.musics
                    .Where(b => b.Tserver.Equals(user.GuildId) && b.Tname.Equals(track))
                    .First();

                db.Remove(perms);
                db.SaveChangesAsync();
            }
            return;
        }
        public static void clearTracks(IGuildUser user)
        {
            using (var db = new DataContext())
            {

                var perms = db.musics
                    .Where(b => b.Tserver.Equals(user.GuildId))
                    .ToList();

                db.Remove(perms);
                db.SaveChangesAsync();
            }
            return;
        }

        public static void addAdmin(ulong adminId)
        {
            using (var db = new DataContext())
            {
                var adminToAdd = new admin { staffMember = adminId };
                db.admins.Add(adminToAdd);
                db.SaveChangesAsync();
            }
        }

        public static void removeAdmin(ulong adminId)
        {
            using (var db = new DataContext())
            {
                var adminToAdd = db.admins
                    .Where(x => x.staffMember == adminId)
                    .First();
                db.admins.Remove(adminToAdd);
                db.SaveChangesAsync();
            }
        }

        public static void SavePrefix(IGuildUser user, char prefix)
        {
            using (var db = new DataContext())
            {

                var perms = db.prefixes
                    .Where(b => b.prefixGuild.Equals(user.GuildId))
                    .ToList();

                if(perms.Count == 0)
                {
                    ulong serverID = user.GuildId;
                    var permTS = new ServPrefix { prefixGuild = serverID, prefix = prefix }; 
                    db.prefixes.Add(permTS);
                    db.SaveChangesAsync();                    
                }
                else
                {
                    var EPerm = perms.First();
                    EPerm.prefix = prefix;
                    db.SaveChangesAsync();
                }
            }
            return;
        }

        public static void SaveWelcomes(IGuildUser user, string prefix)
        {
            using (var db = new DataContext())
            {

                var perms = db.welcomeMessages
                    .Where(b => b.welcomeGuild.Equals(user.GuildId))
                    .ToList();

                if(perms.Count == 0)
                {
                    ulong serverID = user.GuildId;
                    var permTS = new WelcomeMessage { welcomeGuild = serverID, message = prefix }; 
                    db.welcomeMessages.Add(permTS);
                    db.SaveChangesAsync();                    
                }
                else
                {
                    var EPerm = perms.First();
                    EPerm.message = prefix;
                    db.SaveChangesAsync();
                }
            }
            return;
        }

        public static void SaveLeaves(IGuildUser user, string prefix)
        {
            using (var db = new DataContext())
            {

                var perms = db.leavingMessages
                    .Where(b => b.leavingGuild.Equals(user.GuildId))
                    .ToList();

                if(perms.Count == 0)
                {
                    ulong serverID = user.GuildId;
                    var permTS = new LeavingMessage { leavingGuild = serverID, message = prefix }; 
                    db.leavingMessages.Add(permTS);
                    db.SaveChangesAsync();                    
                }
                else
                {
                    var EPerm = perms.First();
                    EPerm.message = prefix;
                    db.SaveChangesAsync();
                }
            }
            return;
        }

        public static string Saveusernote(IGuildUser user, string name, string content, int type, bool Owner)
        {
            using (var db = new DataContext())
            {

                var notes = db.notes
                    .Where(b => b.User.Equals(user.Id))
                    .ToList();
                if(notes.Count >= Settings.maxUserNotes && !(Owner))
                {
                    return("Failed, Notes limit reached.");
                }
                else
                {
                    var noteTS = new Note{Name = name, Type = type, Content = content, User = user.Id, nServId = user.GuildId};
                    db.notes.Add(noteTS);
                    db.SaveChangesAsync();
                    return("Saved");
                }
                
            }
        }

        public static string Saveservernote(IGuildUser user, string name, string content)
        {
            using (var db = new DataContext())
            {

                var notes = db.notes
                    .Where(b => b.User.Equals(user.Id))
                    .ToList();
                if(notes.Count >= Settings.maxServerNotes)
                {
                    return("Failed, Notes limit reached.");
                }
                else
                {
                    var noteTS = new Note{Name = name, Type = 3, Content = content, User = user.Id, nServId = user.GuildId};
                    db.notes.Add(noteTS);
                    db.SaveChangesAsync();
                    return("Saved");
                }
                
            }
        }

        public static string Savestaffnote(IGuildUser user, string name, string content)
        {
            using (var db = new DataContext())
            {

                var notes = db.notes
                    .Where(b => b.User.Equals(user.Id))
                    .ToList();
                if(notes.Count >= Settings.maxStaffNotes)
                {
                    return("Failed, Notes limit reached.");
                }
                else
                {
                    var noteTS = new Note{Name = name, Type = 4, Content = content, User = user.Id, nServId = user.GuildId};
                    db.notes.Add(noteTS);
                    db.SaveChangesAsync();
                    return("Saved");
                }
                
            }
        }

        public static string Deletenote(IGuildUser user, int id)
        {
            using (var db = new DataContext())
            {

                var notes = db.notes
                    .Where(b => b.User.Equals(user.Id) && b.NoteID.Equals(id))
                    .ToList();

                if(notes.Count == 0)
                {
                    return("failed");
                }
                else
                {
                    db.notes.RemoveRange(notes);
                    db.SaveChangesAsync();
                    return("Saved");
                }
                
            }
        }

        public static string AdminDeletenote(IGuildUser user, int id)
        {
            using (var db = new DataContext())
            {

                var notes = db.notes
                    .Where(b => b.nServId.Equals(user.GuildId) && (b.Type.Equals(3) || b.Type.Equals(4)) && b.NoteID.Equals(id))
                    .ToList();

                if(notes.Count == 0)
                {
                    return("failed");
                }
                else
                {
                    db.notes.RemoveRange(notes);
                    db.SaveChangesAsync();
                    return("Saved");
                }
                
            }
        }


        public static void SaveTournament(IGuildUser user, string prizes, bool activ, ulong args, bool join)
        {
            ulong serverID = user.GuildId;
            using (var db = new DataContext())
            {

                var perms = db.Tournaments
                    .Where(b => b.TservID.Equals(user.GuildId))
                    .ToList();

                if(perms.Count == 0)
                {
                    var permTS = new Tournament { TservID = serverID, Title = prizes, Tactive = activ, Tchannel = args, joinable = join, Thost= user.Id }; 
                    db.Tournaments.Add(permTS);
                    db.SaveChangesAsync();                    
                }
                else
                {
                    var EPerm = perms.First();
                    EPerm.Tactive = activ;
                    EPerm.Tchannel = args;
                    EPerm.Title = prizes;
                    EPerm.Thost = user.Id;
                    EPerm.joinable = join;
                    db.SaveChangesAsync();
                    
                }
            }
            return;
        }

        public static void enterTournament(IGuildUser user)
        {
            ulong serverID = user.Guild.Id;
            using (var db = new DataContext())
            {
                var perms = db.Tournaments
                    .Where(b => b.TservID.Equals(user.GuildId))
                    .ToList();

                if(perms.Count == 0)
                {
                    return;                
                }
                else
                {
                    var giving = db.Tournaments.Where(b => b.TservID == serverID).Include(x => x.players).First();
                    int team = 1;
                    foreach(var play in giving.players.OrderBy(x => x.teamNo))
                    {
                        if(team <= play.teamNo)
                        {
                            team++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    TPlayer host = new TPlayer{ playerID = user.Id, state = 2, Tournament = giving, lost = false, teamNo = team, isLeader = true };
                    giving.players.Add(host);
                    db.SaveChangesAsync();
                }
            }
            return;
        }


    }
}
