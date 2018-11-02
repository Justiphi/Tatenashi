using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Justibot.Services;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Justibot.Modules.Meme
{
    public class xpModule : ModuleBase
    {

        [Command("clearlevels")]
        public async Task clearlevels()
        {
            var exp = Justibot.Loader.LoadPerm(Context.User as IGuildUser, "EXP");
            if (exp.Item1 == true)
            {
                IGuildUser activeuser = Context.User as IGuildUser;
                var application = await Context.Client.GetApplicationInfoAsync();
                if (activeuser.GuildPermissions.Has(GuildPermission.Administrator) || activeuser.Id == application.Owner.Id)
                {
                    Justibot.Saver.clearxp(activeuser);
                    XpService.resetserver(Context.Guild);
                }
            }
        }

        [Command("wipeserverlevels")]
        public async Task resetlevels()
        {
            var exp = Justibot.Loader.LoadPerm(Context.User as IGuildUser, "EXP");
            if (exp.Item1 == true)
            {
                IGuildUser activeuser = Context.User as IGuildUser;
                var application = await Context.Client.GetApplicationInfoAsync();
                if (activeuser.GuildPermissions.Has(GuildPermission.Administrator) || activeuser.Id == application.Owner.Id)
                {
                    Justibot.Saver.resetxp(activeuser);
                    XpService.wipeserver(Context.Guild);
                }
            }
        }

        [Group("rank")]
        public class rankModule : ModuleBase
        {
            [Command]
            public async Task rank([OptionalAttribute] IGuildUser user)
            {
                var exp = Justibot.Loader.LoadPerm(Context.User as IGuildUser, "EXP");
                if (exp.Item1 == true)
                {
                    if (user == null)
                    {
                        user = Context.User as IGuildUser;
                    }
                    var results = XpService.getuserxp(user);
                    var level = XpService.checkLevel(results.Value);
                    var avatar = user.GetAvatarUrl();
                    if (avatar == null)
                    {
                        avatar = "https://cdn.discordapp.com/embed/avatars/0.png";
                    }
                    if (avatar.Contains("/a_"))
                    {
                        avatar = $"{avatar.Remove(avatar.Length - 12)}gif?size=128";
                    }
                    var color = new Color(255, 239, 0);

                    var embed = new EmbedBuilder()
                        .WithThumbnailUrl(avatar)
                        .WithColor(color)
                        .WithTitle($"{user.Username}'s server rank")
                        .AddField(x => { x.Name = "Position:"; x.Value = results.Key; x.WithIsInline(true); })
                        .AddField(x => { x.Name = "level:"; x.Value = level.Key; x.WithIsInline(true); })
                        .AddField(x => { x.Name = "Exp:"; x.Value = $"{results.Value}/{level.Value}"; x.WithIsInline(true); })
                        .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
                }
            }

            [Command("global")]
            public async Task globalrank([OptionalAttribute] IGuildUser user)
            {
                if (user == null)
                {
                    user = Context.User as IGuildUser;
                }
                var results = XpService.getglobalxp(user);
                var level = XpService.checkLevel(results.Value);
                var avatar = user.GetAvatarUrl();
                if (avatar == null)
                {
                    avatar = "https://cdn.discordapp.com/embed/avatars/0.png";
                }
                if (avatar.Contains("/a_"))
                {
                    avatar = $"{avatar.Remove(avatar.Length - 12)}gif?size=128";
                }
                var color = new Color(255, 239, 0);

                var embed = new EmbedBuilder()
                    .WithThumbnailUrl(avatar)
                    .WithColor(color)
                    .WithTitle($"{user.Username}'s global rank")
                    .AddField(x => { x.Name = "Position:"; x.Value = results.Key; x.WithIsInline(true); })
                    .AddField(x => { x.Name = "level:"; x.Value = level.Key; x.WithIsInline(true); })
                    .AddField(x => { x.Name = "Exp:"; x.Value = $"{results.Value}/{level.Value}"; x.WithIsInline(true); })
                    .Build();

                await Context.Channel.SendMessageAsync("", false, embed);
            }

            [Command("server")]
            public async Task serverrank()
            {
                var exp = Justibot.Loader.LoadPerm(Context.User as IGuildUser, "EXP");
                if (exp.Item1 == true)
                {
                    var guild = Context.Guild;
                    var results = XpService.getserverxp(guild);
                    var level = XpService.checkLevel(results.Value);
                    var avatar = guild.IconUrl;
                    if (avatar == null)
                    {
                        avatar = "https://cdn.discordapp.com/embed/avatars/0.png";
                    }
                    if (avatar.Contains("/a_"))
                    {
                        avatar = $"{avatar.Remove(avatar.Length - 12)}gif?size=128";
                    }
                    var color = new Color(255, 239, 0);

                    var embed = new EmbedBuilder()
                        .WithThumbnailUrl(avatar)
                        .WithColor(color)
                        .WithTitle($"{guild.Name}'s rank")
                        .AddField(x => { x.Name = "Position:"; x.Value = results.Key; x.WithIsInline(true); })
                        .AddField(x => { x.Name = "level:"; x.Value = level.Key; x.WithIsInline(true); })
                        .AddField(x => { x.Name = "Exp:"; x.Value = $"{results.Value}/{level.Value}"; x.WithIsInline(true); })
                        .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
                }
            }
        }

        [Command("setreward")]
        public async Task setreward(IRole role, int lvl)
        {
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

        [Command("removereward")]
        public async Task removereward(IRole role)
        {
            var exp = Justibot.Loader.LoadPerm(Context.User as IGuildUser, "EXP");
            if (exp.Item1 == true)
            {
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.GuildPermissions.Has(GuildPermission.Administrator) || activeuser.Id == application.Owner.Id)
                {
                    XpService.removeReward(role, activeuser);
                    bool check = Justibot.Saver.deleteReward(activeuser, role.Id);
                    if (check)
                    {
                        await ReplyAsync($"{activeuser.Mention} removed the role **{role.Name}** from the rewards list.");
                    }
                    else
                    {
                        await ReplyAsync($"Failed to remove. {role.Name} not found in reward list.");
                    }
                }
            }
        }

        [Command("rewards")]
        public async Task listrewards()
        {
            var exp = Justibot.Loader.LoadPerm(Context.User as IGuildUser, "EXP");
            if (exp.Item1 == true)
            {
                var rewards = XpService.rewardlist(Context.User as IGuildUser);
                string lists = "";
                foreach (var award in rewards)
                {
                    string role = Context.Guild.GetRole(award.Item1).Name;
                    lists += $"**{role}** is given at level: **{award.Item2}**! \n";
                }

                var color = new Color(255, 239, 0);

                var embed = new EmbedBuilder()
                    .WithTitle($"{Context.Guild.Name} rewards")
                    .WithDescription(lists)
                    .WithColor(color)
                    .Build();

                await Context.Channel.SendMessageAsync("", false, embed);
            }
        }

        [Group("leaderboard")]
        public class leaderModule : ModuleBase
        {
            [Command]
            public async Task leaders()
            {
                var exp = Justibot.Loader.LoadPerm(Context.User as IGuildUser, "EXP");
                if (exp.Item1 == true)
                {
                    var leading = XpService.userLeads(Context).Result;
                    int i = 1;
                    KeyValuePair<int, int> level;
                    string leaders = "";
                    foreach (var leader in leading)
                    {
                        level = XpService.checkLevel(leader.Value);
                        leaders += $"{i}: {(await Context.Guild.GetUserAsync(leader.Key) as IGuildUser).Username} \t lvl: {level.Key} \t {leader.Value}/{level.Value}exp \n";
                        i++;
                    }
                    var color = new Color(255, 239, 0);

                    var embed = new EmbedBuilder()
                        .WithTitle($"{Context.Guild.Name}")
                        .WithDescription(leaders)
                        .WithColor(color)
                        .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
                }
            }

            [Command("global")]
            public async Task globalleaders()
            {
                var leading = XpService.globalLeads(Context).Result;
                int i = 1;
                KeyValuePair<int, int> level;
                string leaders = "";
                var guildys = (Context.Client as DiscordSocketClient).Guilds;
                IGuildUser auser = null;
                foreach (var leader in leading)
                {
                    foreach (var g in guildys)
                    {
                        auser = (await (g as IGuild).GetUserAsync(leader.Key));
                        if (auser != null)
                        {
                            break;
                        }
                    }
                    level = XpService.checkLevel(leader.Value);
                    leaders += $"{i}: {auser.Username} \t lvl: {level.Key} \t {leader.Value}/{level.Value}exp \n";
                    i++;
                }
                var color = new Color(255, 239, 0);

                var embed = new EmbedBuilder()
                    .WithTitle($"Global Leaderboard")
                    .WithDescription(leaders)
                    .WithColor(color)
                    .Build();

                await Context.Channel.SendMessageAsync("", false, embed);

            }

            [Command("server")]
            public async Task serverleaders()
            {
                var exp = Justibot.Loader.LoadPerm(Context.User as IGuildUser, "EXP");
                if (exp.Item1 == true)
                {
                    var leading = XpService.serverLeads(Context).Result;
                    int i = 1;
                    KeyValuePair<int, int> level;
                    string leaders = "";
                    foreach (var leader in leading)
                    {
                        level = XpService.checkLevel(leader.Value);
                        leaders += $"{i}: {(await Context.Client.GetGuildAsync(leader.Key) as IGuild).Name} \t lvl: {level.Key} \t {leader.Value}/{level.Value}exp \n";
                        i++;
                    }
                    var color = new Color(255, 239, 0);

                    var embed = new EmbedBuilder()
                        .WithTitle($"Server Leaderboard")
                        .WithDescription(leaders)
                        .WithColor(color)
                        .Build();

                    await Context.Channel.SendMessageAsync("", false, embed);
                }
            }
        }



    }
}