using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;

namespace Justibot
{
    public class DataContext : DbContext
    {
        public DbSet<ServerPerm> ServerPerms { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TPlayer> TPlayers { get; set; }
        public DbSet<Tmatch> TMatches { get; set; }
        public DbSet<Giveaway> Givaways { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<staffRole> staffRoles { get; set; }
        public DbSet<music> musics { get; set; }
        public DbSet<admin> admins { get; set; }
        public DbSet<ServPrefix> prefixes { get; set; }
        public DbSet<Note> notes { get; set; }
        public DbSet<WelcomeMessage> welcomeMessages { get; set; }
        public DbSet<LeavingMessage> leavingMessages { get; set; }
        public DbSet<serverxp> serversxp { get; set; }
        public DbSet<serveruserxp> serverusersxp { get; set; }
        public DbSet<globaluserxp> globalusersxp { get; set; }
        public DbSet<reward> rewards { get; set; }
        public DbSet<versionControl> versionChecks { get; set; }
        public DbSet<blacklist> blacklists { get; set; }
        public DbSet<roleMessage> roleMessages { get; set; }
        public DbSet<roleReaction> roleReactions { get; set; }
        public DbSet<streamAlert> streamAlerts { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(Services.Configuration.config.ConnectionString, ServerVersion.AutoDetect(Services.Configuration.config.ConnectionString));
        }
    }

    public class ServerPerm
    {
        public int ServerPermID { get; set; }
        public ulong PServId { get; set; }
        public string SPerm { get; set; }
        public bool PermActive {get; set; }
        public ulong PermArg { get; set;} //channel
        public string Pmode { get; set; }
    }

    public class ServPrefix
    {
        public int ServPrefixID {get; set; }
        public ulong prefixGuild { get; set; }
        public char prefix { get; set; }
    }

    public class WelcomeMessage
    {
        public int WelcomeMessageID {get; set; }
        public ulong welcomeGuild { get; set; }
        public string message { get; set; }
    }

    public class versionControl
    {
        public int versionControlID {get; set; }
        public string version { get; set; }
        public DateTime time { get; set; }
    }

    public class LeavingMessage
    {
        public int LeavingMessageID {get; set; }
        public ulong leavingGuild { get; set; }
        public string message { get; set; }
    }

    public class Tournament
    {
        public int TournamentId { get; set; }
        public ulong TservID { get; set; }
        public bool Tactive { get; set; }
        public string Title { get; set; }
        public ulong Tchannel { get; set; }
        public ulong Thost { get; set; }
        public bool joinable { get; set; }
        public int Tmax { get; set; }

        public List<TPlayer> players { get; set; }
        public List<Tmatch> matches { get; set; }
    }

    public class TPlayer
    {
        public int TPlayerID { get; set; }
        public ulong playerID { get; set; }
        public bool isLeader { get; set; }
        public int teamNo { get; set; }
        public int state { get; set; } //1= invited, 2= joined
        public bool lost { get; set; }

        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }
    }

    public class Tmatch
    {
        public int TmatchID { get; set; }
        public int Tteam1ID { get; set; }
        public int Tteam2ID { get; set; }
        public int Troundno { get; set; }
        public int Tmatchno { get; set; }

        public Tournament Tournament { get; set; }
    }

    public class Giveaway
    {
        public int GiveawayId { get; set; }
        public ulong GservID { get; set; }
        public bool GActive { get; set; }
        public string prize { get; set; }
        public ulong Gchannel { get; set; }

        public List<Entry> Entries { get; set; }
    }

    public class Entry
    {
        public int EntryID { get; set; }
        public ulong entrentID { get; set; }
        public bool ishost { get; set; }

        public int GiveawayId { get; set; }
        public Giveaway Giveaway { get; set; }
    }

    public class staffRole
    {
        public int staffRoleID { get; set; }
        public ulong RServId { get; set; }
        public ulong PermArg { get; set;} //role
    }

    public class music
    {
        public int musicId { get; set; }
        public string Tname { get; set; }
        public ulong Tserver { get; set; }
    }

    public class admin
    {
        public int adminId { get; set; }
        public ulong staffMember { get; set; }
    }

    public class Note
    {
        public int NoteID { get; set; }
        public ulong nServId { get; set; }
        public string Name { get; set; }
        public int Type {get; set; }
        public ulong User { get; set;} 
        public string Content { get; set; }
    }

    public class serveruserxp
    {
        public int serveruserxpID { get; set; }
        public int Xp { get; set; } 
        public ulong User { get; set;} 

        public serverxp Server { get; set; }
    }
    
    public class serverxp
    {
        public int serverxpID { get; set; }
        public ulong xServId { get; set; }
        public int Xp { get; set; } 

        public List<serveruserxp> usersXp { get; set; }
    }

    public class globaluserxp
    {
        public int globaluserxpID { get; set; }
        public int Xp { get; set; } 
        public ulong User { get; set; } 
    }

    public class reward
    {
        public int rewardID { get; set; }
        public int rewardlvl { get; set; }
        public ulong rserver { get; set; }
        public ulong rewardrole { get; set; }
    }

    public class blacklist
    {
        public int blacklistID { get; set; }
        public ulong bserver { get; set; }
    }

    public class roleMessage
    {
        public int roleMessageId {get; set;}
        public ulong MessageId {get; set;}
        public ulong guildId {get; set;}
    }
    public class roleReaction
    {
        public int roleReactionId {get; set;}
        public string reaction {get; set;}
        public ulong roleId {get; set;}
        public ulong guildId {get; set;}
    }
    public class streamAlert
    {
        public ulong userId {get; set;}
        public ulong guildId {get; set;}
        public ulong channelId {get; set;}
        public string message {get; set;}
    }
}