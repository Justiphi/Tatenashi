using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Justibot.Services;
using System.Linq;


namespace Justibot.Modules.OwnerModule
{
    //sets owner module prefix
    [Group("o")]
    public class OwnerModule : ModuleBase
    {
        //creates invite link to server with specified server id
        [Command("invite")]
        public async Task CreateInvite(ulong id)
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            if ((Justibot.Loader.checkAdmin(Context.User.Id)) || (Context.User.Id == application.Owner.Id))
            {
                var RequestedGuild = await Context.Client.GetGuildAsync(id);
                if (RequestedGuild != null)
                {
                    IInviteMetadata GuildDefault =
                        await (await RequestedGuild.GetDefaultChannelAsync() as ITextChannel)
                            .CreateInviteAsync();
                    await Context.Channel.SendMessageAsync("Invite link: " + GuildDefault.Url);
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Server not found");
                }
            }
        }

        //displays information about a server based on server id
        [Command("serverinfo")]
        [Summary("displays info about the server")]
        public async Task ServerInfo(ulong id)
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            if ((Justibot.Loader.checkAdmin(Context.User.Id)) || (Context.User.Id == application.Owner.Id))
            {
                var guild = await Context.Client.GetGuildAsync(id) as SocketGuild;
                var owner = guild.Owner.Username;
                var avatar = guild.IconUrl;
                var avatar2 = guild.IconUrl;
                var usersCount = guild.MemberCount;
                var botCount = guild.Users.Count(x => x.IsBot); ;
                var humanCount = (usersCount - botCount);
                var time = guild.CreatedAt;
                var verification = guild.VerificationLevel;
                var channels = guild.Channels.Count;
                var roles = guild.Roles.Count;

                if (avatar == null)
                {
                    avatar = "none";
                    avatar2 = avatar = "https://cdn.discordapp.com/embed/avatars/0.png";
                }

                if (avatar.Contains("/a_"))
                {
                    avatar = $"{avatar.Remove(avatar.Length - 12)}gif?size=128";
                    avatar2 = $"{avatar.Remove(avatar.Length - 12)}gif?size=128";
                }

                string message = (
                    $"- {Format.Bold("count:")} {usersCount} \n" +
                    $"\t- Humans:{humanCount} \n" +
                    $"\t- Bots:{botCount}"
                );
                string other = (
                    $"- {Format.Bold("Channel count:")} {channels} \n" +
                    $"- {Format.Bold("Role count:")} {roles} \n" +
                    $"- {Format.Bold("Verification Level:")} {verification}"
                );
                string footer;
                if (avatar == "none")
                {
                    footer = (
                        $"[{avatar}]({avatar})"
                    );
                }
                else
                {
                    footer = avatar;
                }

                Color color = new Color(0, 225, 255);

                var embed = new EmbedBuilder()
                    .WithThumbnailUrl(avatar2)
                    .WithTitle($"Server Info for {guild.Name}")
                    .AddField(x => { x.Name = "ID:"; x.Value = guild.Id; x.WithIsInline(true); })
                    .AddField(x => { x.Name = "Owner:"; x.Value = owner; x.WithIsInline(true); })
                    .AddField(x => { x.Name = "Created at:"; x.Value = time.ToString().Remove(time.ToString().Length - 6); ; x.WithIsInline(true); })
                    .AddField(x => { x.Name = "Members:"; x.Value = message; x.WithIsInline(true); })
                    .AddField(x => { x.Name = "Region:"; x.Value = guild.VoiceRegionId; x.WithIsInline(true); })
                    .AddField(x => { x.Name = "Other:"; x.Value = other; x.WithIsInline(true); })
                    .AddField(x => { x.Name = "Icon:"; x.Value = footer; x.WithIsInline(false); })
                    .WithColor(color)
                    .Build();

                await Context.Channel.SendMessageAsync("", false, embed);
            }
        }

        //forces bot to leave a server based on server id
        [Command("leaveGuild")]
        [RequireOwner]
        public async Task LeaveGuild(ulong id)
        {
            var client = Context.Client as DiscordSocketClient;
            await client.GetGuild(id).LeaveAsync();
        }

        [Command("cutxp")]
        [RequireOwner]
        public async Task cutxp()
        {
            if (!xpdict.xpblacklist.Contains(Context.Guild.Id))
            {
                xpdict.xpblacklist.Add(Context.Guild.Id);
                var activeuser = Context.User as IGuildUser;
                Justibot.Saver.blacklist(activeuser, Context.Guild.Id);
                await ReplyAsync("global xp can no longer be earned here");
            }
            else
            {
                await ReplyAsync("Server was already blacklisted");
            }
        }

        [Command("allowxp")]
        [RequireOwner]
        public async Task allowxp()
        {
            if (xpdict.xpblacklist.Contains(Context.Guild.Id))
            {
                xpdict.xpblacklist.Remove(Context.Guild.Id);
                var activeuser = Context.User as IGuildUser;
                Justibot.Saver.unblacklist(activeuser, Context.Guild.Id);
                await ReplyAsync("global xp can now be earned here");
            }
            else
            {
                await ReplyAsync("Server was not blacklisted");
            }
        }

        [Command("wipexp")]
        [RequireOwner]
        public async Task wipexp()
        {
            Justibot.Saver.wipexp();
            Services.xpdict.globalxp.Clear();
            Services.xpdict.userxp.Clear();
            Services.xpdict.serverxp.Clear();
            Services.xpdict.Localxp.Clear();

            await ReplyAsync("All xp has been wiped!");
        }

        [Command("writeline")]
        [RequireOwner]
        public void ConsoleWrite([Remainder] string write)
        {
            Console.WriteLine(write);
        }


        [Command("guildlist")]
        public async Task GuildList()
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            if ((Justibot.Loader.checkAdmin(Context.User.Id)) || (Context.User.Id == application.Owner.Id))
            {
                string guildList = "";
                var guilds = (Context.Client as DiscordSocketClient).Guilds;
                foreach (var g in guilds)
                {
                    guildList += $"Name: {g.Name}\n ID: {g.Id} | \n";
                }
                File.WriteAllText("guildlist.txt", guildList);
                await Context.Channel.SendFileAsync("guildlist.txt", null, false, null);
            }
        }

        [Command("getDB")]
        [RequireOwner]
        public async Task GetDB()
        {
            await Context.Channel.SendFileAsync("bin/debug/netcoreapp1.0/justibot.db");
        }

        [Command("sayin")]
        [RequireOwner]
        public async Task talk(ulong id, [RemainderAttribute]string message)
        {
            ITextChannel channel = null;
            var guilds = (Context.Client as DiscordSocketClient).Guilds;
            foreach (var g in guilds)
            {
                channel = g.GetTextChannel(id);
                if (channel != null)
                {
                    break;
                }
            }
            if (channel == null)
            {
                await ReplyAsync("channel not found");
            }
            else
            {
                await channel.SendMessageAsync(message);
                await ReplyAsync("message sent");
            }
        }

        [Command("announce")]
        [RequireOwner]
        public async Task Announce([RemainderAttribute]string message)
        {
            ITextChannel channel;
            var guilds = (Context.Client as DiscordSocketClient).Guilds;
            foreach (var g in guilds)
            {
                channel = g.DefaultChannel as ITextChannel;
                await channel.SendMessageAsync($"{Format.Bold($"{Context.Client.CurrentUser.Username} Announcement:")} {message}");
            }
        }

        [Command("restart")]
        [RequireOwner]
        public async Task Restart()
        {
            await ReplyAsync("Restarting...");

        }

        [Command("rename")]
        [RequireOwner]
        public async Task rename([RemainderAttribute]string name)
        {
            var dClient = Context.Client as DiscordSocketClient;
            await dClient.CurrentUser.ModifyAsync(x => x.Username = name);
            await ReplyAsync($"Username set to {Format.Bold($"{name}!")}");
        }

        [Command("addAdmin")]
        [RequireOwner]
        public async Task addStaff(IUser admin)
        {
            if (Justibot.Loader.checkAdmin(admin.Id))
            {
                await ReplyAsync($"{admin.Mention} is already an admin for {Format.Bold($"{Context.Client.CurrentUser.Username}!")}");
            }
            else
            {
                Justibot.Saver.addAdmin(admin.Id);
                await ReplyAsync($"{admin.Mention} has been made an admin for {Format.Bold($"{Context.Client.CurrentUser.Username}!")}");
            }
        }

        [Command("removeAdmin")]
        [RequireOwner]
        public async Task removeStaff(IUser admin)
        {
            if (Justibot.Loader.checkAdmin(admin.Id))
            {
                Justibot.Saver.removeAdmin(admin.Id);
                await ReplyAsync($"{admin.Mention} is no longer an admin for {Format.Bold($"{Context.Client.CurrentUser.Username}")}.");
            }
            else
            {
                await ReplyAsync($"{admin.Mention} is already not an admin for {Format.Bold($"{Context.Client.CurrentUser.Username}")}.");
            }
        }

        [Command("shutDown")]
        [RequireOwner]
        public async Task shutDown()
        {
            await ReplyAsync("shutting down...");
            Environment.Exit(0);
        }

        [Command("getRoles")]
        [RequireOwner]
        public async Task getRoles(ulong id)
        {
            var guild = await Context.Client.GetGuildAsync(id) as SocketGuild;
            string rolelist = "Roles: \n";
            foreach (var role in guild.Roles)
            {
                if (role.Name != "@everyone")
                {
                    rolelist += $"{role.Name}: {role.Id} \n";
                }
            }
            await ReplyAsync(rolelist);
        }

        [Command("addRole")]
        [RequireOwner]
        public async Task addRoles(ulong id, ulong role)
        {
            var guild = await Context.Client.GetGuildAsync(id) as IGuild;
            var roly = guild.GetRole(role);
            var user = await guild.GetUserAsync(Context.User.Id) as IGuildUser;
            await user.AddRoleAsync(roly);
            await ReplyAsync("Role has been added!");
        }

        [Command("removeRole")]
        [RequireOwner]
        public async Task removeRoles(ulong id, ulong role)
        {
            var guild = await Context.Client.GetGuildAsync(id) as IGuild;
            var roly = guild.GetRole(role);
            var user = await guild.GetUserAsync(Context.User.Id) as IGuildUser;
            await user.RemoveRoleAsync(roly);
            await ReplyAsync("Role has been added!");
        }

        [Command("givexp")]
        [RequireOwner]
        public async Task givexp(IGuildUser user, int xp)
        {
            Justibot.Services.XpService.plusXp(user, xp);
            await ReplyAsync($"{user.Username} has been given {xp}xp");
        }

        [Command("whois")]
        [RequireOwner]
        public async Task whois(ulong id)
        {
            var guilds = (Context.Client as DiscordSocketClient).Guilds;
            IGuildUser user = null;
            foreach (var g in guilds)
            {
                user = (await (g as IGuild).GetUserAsync(id));
                if (user != null)
                {
                    break;
                }
            }
            if (user == null)
            {
                await ReplyAsync("user does not exist");
            }
            else
            {
                var uID = user.Id;
                var userN = user.Username;
                var time = user.CreatedAt;
                var Descrim = user.Discriminator;
                var avatar = user.GetAvatarUrl();
                var avatar2 = user.GetAvatarUrl();

                if (avatar == null)
                {
                    avatar = "none";
                    avatar2 = "https://cdn.discordapp.com/embed/avatars/0.png";
                }

                if (avatar.Contains("/a_"))
                {
                    avatar = $"{avatar.Remove(avatar.Length - 12)}gif?size=128";
                    avatar2 = $"{avatar.Remove(avatar.Length - 12)}gif?size=128";
                }

                string footer;
                if (avatar != "none")
                {
                    footer = (
                        $"[{avatar}]({avatar})"
                    );
                }
                else
                {
                    footer = avatar;
                }

                Color color = new Color(0, 225, 255);

                var embed = new EmbedBuilder()
                    .WithThumbnailUrl(avatar2)
                    .WithColor(color)
                    .WithTitle("User Info:")
                    .AddField(x => { x.Name = "Name:"; x.Value = userN; x.WithIsInline(true); })
                    .AddField(x => { x.Name = "ID:"; x.Value = uID; x.WithIsInline(true); })
                    .AddField(x => { x.Name = "Descrim:"; x.Value = Descrim; x.WithIsInline(true); })
                    .AddField(x => { x.Name = "Created at:"; x.Value = time.ToString().Remove(time.ToString().Length - 6); x.WithIsInline(true); })
                    .AddField(x => { x.Name = "Avatar:"; x.Value = footer; x.WithIsInline(false); })
                    .Build();

                await Context.Channel.SendMessageAsync("", false, embed);
            }
        }

        [Command("whoowns")]
        [RequireOwner]
        public async Task whoowns(ulong id)
        {
            var guild = await Context.Client.GetGuildAsync(id) as SocketGuild;
            if (guild == null)
            {
                await ReplyAsync("guild does not exist");
            }
            else
            {
                IGuildUser user = guild.Owner;

                var uID = user.Id;
                var userN = user.Username;
                var time = user.CreatedAt;
                var Descrim = user.Discriminator;
                var avatar = user.GetAvatarUrl();
                var avatar2 = user.GetAvatarUrl();

                if (avatar == null)
                {
                    avatar = "none";
                    avatar2 = "https://cdn.discordapp.com/embed/avatars/0.png";
                }

                if (avatar.Contains("/a_"))
                {
                    avatar = $"{avatar.Remove(avatar.Length - 12)}gif?size=128";
                    avatar2 = $"{avatar.Remove(avatar.Length - 12)}gif?size=128";
                }

                string footer;
                if (avatar != "none")
                {
                    footer = (
                        $"[{avatar}]({avatar})"
                    );
                }
                else
                {
                    footer = avatar;
                }

                Color color = new Color(0, 225, 255);

                var embed = new EmbedBuilder()
                    .WithThumbnailUrl(avatar2)
                    .WithColor(color)
                    .WithTitle("User Info:")
                    .AddField(x => { x.Name = "Name:"; x.Value = userN; x.WithIsInline(true); })
                    .AddField(x => { x.Name = "ID:"; x.Value = uID; x.WithIsInline(true); })
                    .AddField(x => { x.Name = "Descrim:"; x.Value = Descrim; x.WithIsInline(true); })
                    .AddField(x => { x.Name = "Created at:"; x.Value = time.ToString().Remove(time.ToString().Length - 6); x.WithIsInline(true); })
                    .AddField(x => { x.Name = "Avatar:"; x.Value = footer; x.WithIsInline(false); })
                    .Build();

                await Context.Channel.SendMessageAsync("", false, embed);
            }

        }

        [Command("setreward")]
        [RequireOwner]
        public async Task setreward(ulong role2, int lvl)
        {
            IRole role = Context.Guild.GetRole(role2);
            var exp = Justibot.Loader.LoadPerm(Context.User as IGuildUser, "EXP");
            if (exp.Item1 == true)
            {
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.GuildPermissions.Has(GuildPermission.Administrator) || activeuser.Id == application.Owner.Id)
                {
                    XpService.addReward(role, activeuser, lvl);
                    Justibot.Saver.saveReward(activeuser, role.Id, lvl);
                    await ReplyAsync($"{activeuser.Mention} set the role **{role.Name}** as a level {lvl} reward.");
                }
            }
        }

        [Group("giveaway")]
        public class giveaway : ModuleBase
        {

            [Command("new")]
            [RequireOwner]
            public async Task Gnew(IUser host, [Remainder] string prize)
            {
                var activeuser = host as IGuildUser;
                var prefix = PrefixService.getPrefix(Context.Guild.Id);
                var application = await Context.Client.GetApplicationInfoAsync();
                var activator = true;
                var gives = Justibot.Loader.LoadGiveaway(activeuser);
                var results = Justibot.Loader.LoadPerm(activeuser, "GIVEAWAY");
                ITextChannel channel = await Context.Guild.GetTextChannelAsync(results.Item2) as ITextChannel;
                ulong channel2 = (await Context.Guild.GetTextChannelAsync(gives.Item2) as ITextChannel).Id;
                if (Context.Channel.Id == results.Item2)
                {
                    if (gives.Item1 == true)
                    {
                        await ReplyAsync("giveaway already in progress, cannot start a new one.");
                    }
                    else
                    {
                        if (results.Item1 == true)
                        {
                            if (results.Item3 == "OPEN")
                            {
                                Justibot.Saver.SaveGiveaway(activeuser, prize, activator, results.Item2);
                                await channel.SendMessageAsync($"{activeuser.Mention} has started a giveaway for {Format.Bold($"{prize}!!!")} Use `{prefix}giveaway enter` to be in to win!!! \n" +
                                $"Host ({activeuser.Mention}) is entered by default");
                            }
                            else if (results.Item3 == "CLOSED" && (activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || application.Owner == Context.User))
                            {
                                Justibot.Saver.SaveGiveaway(activeuser, prize, activator, channel.Id);
                                await channel.SendMessageAsync($"{activeuser.Mention} has started a giveaway for {Format.Bold($"{prize}!!!")} Use `{prefix}giveaway enter` to be in to win!!! \n" +
                                $"Host ({activeuser.Mention}) is entered by default");
                            }
                            else
                            {
                                await ReplyAsync("you do not have permission to do start a giveaway.");
                            }
                        }
                        else
                        {
                            await ReplyAsync("giveaways are currently not enabled.");
                        }
                    }
                }
            }

            [Command("enter")]
            [RequireOwner]
            public async Task enterg(IUser entry)
            {
                bool isentered = false;
                var activeuser = entry as IGuildUser;
                var gives = Justibot.Loader.LoadGiveaway(activeuser);
                string prize = gives.Item3;
                if (Context.Channel.Id == gives.Item2)
                {
                    if (gives.Item1 == true)
                    {
                        isentered = Justibot.Loader.checkentry(activeuser);
                        if (isentered == false)
                        {
                            Justibot.Saver.enterGiveaway(activeuser);
                            await ReplyAsync($"{activeuser.Mention} has been entered!");
                        }
                        else
                        {
                            await ReplyAsync("You have already entered.");
                        }
                    }
                    else
                    {
                        await ReplyAsync("No giveaways are active to enter.");
                    }
                }
            }
        }

    }
}