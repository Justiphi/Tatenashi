using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Runtime.InteropServices;
using System;
using Discord.WebSocket;

namespace Justibot.Modules.Public
{
    [Group("perms")]
    public class PermsModule : ModuleBase
    {

        [Command("status")]
        public async Task permstats()
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            IGuildUser activeuser = Context.User as IGuildUser;
            SocketGuild activeguild = activeuser.Guild as SocketGuild;

            string welcomes;
            string leavings;
            string logs;
            string adverts;
            string giveawayss;
            string epsilons;
            string bcss;
            string levelss;
            string notess;
            string joins;
            string bunkers;
            string betas;
            string prefixs;
            string wolvs;

            var results = Justibot.Loader.LoadPerm(activeuser, "WELCOME");
            if(results.Item1)
            {
                welcomes = $"Enabled in channel: {await Context.Guild.GetTextChannelAsync(results.Item2)} \n" +
                $"Message: {Services.WelcomeService.checkWelcomes(activeguild)} \n" +
                $"With mode: {results.Item3}";
            }
            else
            {
                welcomes = "Disabled";
            }

            var results2 = Justibot.Loader.LoadPerm(activeuser, "LEAVING");
            if(results2.Item1)
            {
                leavings = $"Enabled in channel: {await Context.Guild.GetTextChannelAsync(results2.Item2)} \n" +
                $"Message: {Services.LeavingService.checkLeaves(activeguild)} \n" +
                $"With mode: {results2.Item3}";
            }
            else
            {
                leavings = "Disabled";
            }

            var results3 = Justibot.Loader.LoadPerm(activeuser, "LOG");
            if(results3.Item1)
            {
                logs = $"Enabled in channel: {await Context.Guild.GetTextChannelAsync(results3.Item2)} \n" +
                $"With mode: {results3.Item3}";
            }
            else
            {
                logs = "Disabled";
            }

            var results4 = Justibot.Loader.LoadPerm(activeuser, "ADVERTISING");
            if(results4.Item1)
            {
                if(results4.Item3 == "REPOST")
                {
                    adverts = $"Enabled \n" +
                    $"Reposting adverts to {await Context.Guild.GetTextChannelAsync(results4.Item2)}";
                }
                else
                {
                    adverts = $"Enabled \n" +
                    $"Deleting adverts";
                }
            }
            else
            {
                adverts = "Disabled";
            }

            var results5 = Justibot.Loader.LoadPerm(activeuser, "GIVEAWAY");
            if(results5.Item1)
            {
                if(results5.Item3 == "CLOSED")
                {
                    giveawayss = $"Enabled \n" +
                    $"For staff use in {await Context.Guild.GetTextChannelAsync(results5.Item2)}";
                }
                else
                {
                    giveawayss = $"Enabled \n" +
                    $"For public use in {await Context.Guild.GetTextChannelAsync(results5.Item2)}";
                }
            }
            else
            {
                giveawayss = "Disabled";
            }

            var results6 = Justibot.Loader.LoadPerm(activeuser, "EPSILON");
            if(results6.Item1)
            {
                epsilons = $"Enabled";
            }
            else
            {
                epsilons = "Disabled";
            }

            var results7 = Justibot.Loader.LoadPerm(activeuser, "BCS");
            if(results7.Item1)
            {
                bcss = $"Enabled";
            }
            else
            {
                bcss = "Disabled";
            }

            var results8 = Justibot.Loader.LoadPerm(activeuser, "EXP");
            if(results8.Item1)
            {
                levelss = $"Enabled";
            }
            else
            {
                levelss = "Disabled";
            }

            var results9 = Justibot.Loader.LoadPerm(activeuser, "NOTES");
            if(results9.Item1)
            {
                if(results9.Item3 == "CLOSED")
                {
                    notess = $"Enabled \n" +
                    $"server notes useable by staff only";
                }
                else
                {
                    notess = $"Enabled \n" +
                    $"server notes usable by public";
                }
            }
            else
            {
                notess = "Disabled";
            }

            var results10 = Justibot.Loader.LoadPerm(activeuser, "JOINROLE");
            if(results10.Item1)
            {
                joins = $"Enabled with role: {Context.Guild.GetRole(results10.Item2)}";
            }
            else
            {
                joins = "Disabled";
            }

            var results11 = Justibot.Loader.LoadPerm(activeuser, "BUNKER");
            if(results11.Item1)
            {
                if (Settings.bunker.Contains(Context.Guild.Id)){
                    bunkers = $"Enabled: ON";
                }
                else
                {
                    bunkers = $"Enabled: OFF";
                }
            }
            else
            {
                bunkers = "Disabled";
            }

            var results12 = Justibot.Loader.LoadPerm(activeuser, "BETA");
            if(results12.Item1)
            {
                betas = $"Enabled";
            }
            else
            {
                betas = "Disabled";
            }

            if(Services.PrefixService.getPrefix(activeguild.Id) == '+')
            {
                prefixs = "Disabled \n" +
                "Default: +";
            }
            else
            {
                prefixs = "enabled \n" +
                $"Current: {Services.PrefixService.getPrefix(activeguild.Id)}";
            }

            var results13 = Justibot.Loader.LoadPerm(activeuser, "WOLVES");
            if(results13.Item1)
            {
                wolvs = $"Enabled";
            }
            else
            {
                wolvs = "Disabled";
            }

            

            string message = (
                $"- {Format.Bold("Welcome:")} {welcomes}\n\n" +
                $"- {Format.Bold("Leaving:")} {leavings}"
            );
            string message2 = (
                $"- {Format.Bold("Epsilon:")} {epsilons} \n" +
                $"- {Format.Bold("BCS:")} {bcss} \n" +
                $"- {Format.Bold("Beta:")} {betas} \n" +
                $"- {Format.Bold("NightWolves:")} {wolvs}"
            );
            string message3 = (
                $"- {Format.Bold("Giveaways:")} {giveawayss}\n\n" +
                $"- {Format.Bold("Xp")}: {levelss}\n\n" +
                $"- {Format.Bold("Notes:")} {notess}"
            );
            string message4 = (
                $"- {Format.Bold("Logs:")} {logs}\n\n" +
                $"- {Format.Bold("JoinRole:")} {joins} \n\n" +
                $"- {Format.Bold("Bunker:")} {bunkers} \n\n" +
                $"- {Format.Bold("Advertising:")} {adverts}"
            );
            var avatar = Context.Guild.IconUrl;
            Color color = new Color(0, 225, 255);

            var author = new EmbedAuthorBuilder()
                .WithName($"{Context.Client.CurrentUser.Username}\n");

            if (avatar.Contains("/a_"))
            {
                avatar = $"{avatar.Remove(avatar.Length - 12)}gif?size=128";
            }

            var embed = new EmbedBuilder()
                .WithThumbnailUrl(avatar)
                .WithColor(color)
                .WithAuthor(author)
                .AddField(x => { x.Name = "Custom Prefix:"; x.Value = prefixs; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Greetings:"; x.Value = message; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Moderation:"; x.Value = message4; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Modules:"; x.Value = message3; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Creator enabled:"; x.Value = message2; x.WithIsInline(true); })
                .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Group("joinRole")]
        public class roleModule : ModuleBase
        {
            [Command("enable")]
            [Summary("adds/updates server permissions for module usage")]
            [RequireBotPermissionAttribute(GuildPermission.ManageRoles)]
            public async Task addRole(IRole role)
            {
                bool saving = true;
                bool activator = true;
                ulong channelid = role.Id;
                string mode = "nill";
                string permission = "JOINROLE";
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if ((activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) && activeuser.GuildPermissions.Has(GuildPermission.ManageRoles)) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} enabled permission: {Format.Bold($"{permission}")} for role: {role}!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }

            }


            [Command("disable")]
            [Summary("adds/updates server permissions for module usage")]
            [RequireBotPermissionAttribute(GuildPermission.ManageRoles)]
            public async Task removeWelcome()
            {
                bool saving = true;
                bool activator = false;
                string mode = "nill";
                string permission = "JOINROLE";
                ulong channelid = 123;
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if ((activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) && activeuser.GuildPermissions.Has(GuildPermission.ManageRoles)) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} disabled permission: {Format.Bold($"{permission}")}!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }


        [Group("welcome")]
        public class welcomeModule : ModuleBase
        {
            [Command("enable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task addWelcome(string moder, [OptionalAttribute] ITextChannel channel)
            {
                bool saving = true;
                bool activator = true;
                ulong channelid = 123;
                string mode = "nill";
                string permission = "WELCOME";
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    switch (moder.ToUpper())
                    {
                        case "EMBED":
                            mode = "EMBED";
                            moder = "embeded";
                            break;
                        case "PLAIN":
                            mode = "PLAIN";
                            moder = "plain";
                            break;
                        default:
                            saving = false;
                            await ReplyAsync("invalid mode code, please use Embed/Plain.");
                            break;
                    }

                    if (channel == null)
                    {
                        channelid = Context.Channel.Id;
                    }
                    else
                    {
                        channelid = channel.Id;
                    }

                    ITextChannel channel2 = await Context.Guild.GetTextChannelAsync(channelid) as ITextChannel;

                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} enabled {moder} permission: {Format.Bold($"{permission}")} in channel: {channel2.Mention}!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }

            }


            [Command("disable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task removeWelcome()
            {
                bool saving = true;
                bool activator = false;
                string mode = "nill";
                string permission = "WELCOME";
                ulong channelid = Context.Channel.Id;

                ITextChannel channel2 = await Context.Guild.GetTextChannelAsync(channelid) as ITextChannel;
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} disabled permission: {Format.Bold($"{permission}")}!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }

        [Group("leaving")]
        public class leaveModule : ModuleBase
        {
            [Command("enable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task addLeave(string moder, [OptionalAttribute] ITextChannel channel)
            {
                bool saving = true;
                bool activator = true;
                ulong channelid = 123;
                string mode = "nill";
                string permission = "LEAVING";
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    switch (moder.ToUpper())
                    {
                        case "EMBED":
                            mode = "EMBED";
                            moder = "embeded";
                            break;
                        case "PLAIN":
                            mode = "PLAIN";
                            moder = "plain";
                            break;
                        default:
                            saving = false;
                            await ReplyAsync("invalid mode code, please use Embed/Plain.");
                            break;
                    }

                    if (channel == null)
                    {
                        channelid = Context.Channel.Id;
                    }
                    else
                    {
                        channelid = channel.Id;
                    }

                    ITextChannel channel2 = await Context.Guild.GetTextChannelAsync(channelid) as ITextChannel;

                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} enabled {moder} permission: {Format.Bold($"{permission}")} in channel: {channel2.Mention}!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }

            }


            [Command("disable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task removeGoodbye()
            {
                bool saving = true;
                bool activator = false;
                string mode = "nill";
                string permission = "LEAVING";
                ulong channelid = Context.Channel.Id;

                ITextChannel channel2 = await Context.Guild.GetTextChannelAsync(channelid) as ITextChannel;
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} disabled permission: {Format.Bold($"{permission}")}!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }

        [Group("giveaway")]
        class giveawayModule : ModuleBase
        {
            [Command("enable")]
            public async Task enableG(string mode, [OptionalAttribute] ITextChannel channel)
            {
                bool saving = true;
                bool activator = true;
                ulong channelid = 123;
                string permission = "GIVEAWAY";
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    if (activator == true)
                    {

                        switch (mode.ToUpper())
                        {
                            case "OPEN":
                                saving = true;
                                break;
                            case "CLOSED":
                                saving = true;
                                break;
                            default:
                                saving = false;
                                await ReplyAsync("Invalid mode please use open/closed.");
                                return;
                        }

                        if (channel == null)
                        {
                            channelid = Context.Channel.Id;
                        }
                        else
                        {
                            channelid = channel.Id;
                        }

                        ITextChannel channel2 = await Context.Guild.GetTextChannelAsync(channelid) as ITextChannel;

                        if (saving == true)
                        {
                            Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode.ToUpper());
                            if (mode.ToUpper() == "OPEN")
                            {
                                await ReplyAsync($"{Context.User.Mention} has enabled giveaways in channel: {channel2.Mention} for public use!");
                            }
                            else if (mode.ToUpper() == "CLOSED")
                            {
                                await ReplyAsync($"{Context.User.Mention} has enabled giveaways in channel: {channel2.Mention} for staff use!");
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }

            [Command("disable")]
            public async Task disableGive()
            {
                bool activator = false;
                ulong channelid = 123;
                string permission = "GIVEAWAY";
                string mode2 = "nill";

                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    ulong hostID = Justibot.Loader.GetGHost(activeuser);
                    IGuildUser hoster = Context.Guild.GetUserAsync(hostID) as IGuildUser;
                    var gives = Justibot.Loader.LoadGiveaway(activeuser);
                    if (gives.Item1 == true)
                    {
                        await ReplyAsync($"Cannot disable giveaway perm while a giveaway is being hosted. (current giveaway hosted by {hoster.Mention}.)");
                    }
                    else
                    {
                        channelid = Context.Guild.DefaultChannelId;

                        ITextChannel channel2 = await Context.Guild.GetTextChannelAsync(channelid) as ITextChannel;
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode2);
                        await ReplyAsync($"{Context.User.Mention} has disabled giveaways!");
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }


        [Group("addSilence")]
        public class silenceModule : ModuleBase
        {
            [Command("enable")]
            [Summary("adds/updates server permissions for module usage")]
            [RequireBotPermissionAttribute(GuildPermission.ManageRoles)]
            public async Task addSilence(IRole role)
            {
                bool saving = true;
                bool activator = true;
                ulong channelid = role.Id;
                string mode = "nill";
                string permission = "SILENCE";
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if ((activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) && activeuser.GuildPermissions.Has(GuildPermission.ManageRoles)) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} enabled permission: {Format.Bold($"{permission}")} for role: {role}!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }

            }


            [Command("disable")]
            [Summary("adds/updates server permissions for module usage")]
            [RequireBotPermissionAttribute(GuildPermission.ManageRoles)]
            public async Task removeSilence()
            {
                bool saving = true;
                bool activator = false;
                string mode = "nill";
                string permission = "SILENCE";
                ulong channelid = 123;
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if ((activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) && activeuser.GuildPermissions.Has(GuildPermission.ManageRoles)) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} disabled permission: {Format.Bold($"{permission}")}!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }



        [Group("Staff")]
        public class staffModule : ModuleBase
        {
            [Command("enable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task addStaff()
            {
                bool saving = true;
                bool activator = true;
                string mode = "nill";
                ulong channelid = 123;
                string permission = "STAFF";
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if ((activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) && activeuser.GuildPermissions.Has(GuildPermission.ManageRoles)) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} enabled permission: {Format.Bold($"{permission}")}, staff roles can now be added!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }

            }


            [Command("disable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task removeStaff()
            {
                bool saving = true;
                bool activator = false;
                string mode = "nill";
                string permission = "STAFF";
                ulong channelid = 123;
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if ((activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id)))
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} disabled permission: {Format.Bold($"{permission}")}, bot no longer can use staff functions.");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }

        [Group("BCS")]
        [RequireOwnerAttribute]
        public class BCSModule : ModuleBase
        {
            [Command("enable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task addBCS()
            {
                bool saving = true;
                bool activator = true;
                string mode = "nill";
                ulong channelid = 123;
                string permission = "BCS";
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.Id == application.Owner.Id)
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} enabled {Format.Bold("BCS")} mode, BCS commands are now available!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }

            }


            [Command("disable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task removeBCS()
            {
                bool saving = true;
                bool activator = false;
                string mode = "nill";
                string permission = "BCS";
                ulong channelid = 123;
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.Id == application.Owner.Id)
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} disabled {Format.Bold($"BCS")} mode, BCS commands are no longer available!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }


        [Group("Epsilon")]
        [RequireOwnerAttribute]
        public class epsilonModule : ModuleBase
        {
            [Command("enable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task addEpsilon()
            {
                bool saving = true;
                bool activator = true;
                string mode = "nill";
                ulong channelid = 123;
                string permission = "EPSILON";
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.Id == application.Owner.Id)
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} enabled {Format.Bold("Epsilon Phi")} mode, Please enjoy Justibot Phi!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }

            }


            [Command("disable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task removeEpsilon()
            {
                bool saving = true;
                bool activator = false;
                string mode = "nill";
                string permission = "EPSILON";
                ulong channelid = 123;
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.Id == application.Owner.Id)
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} disabled {Format.Bold($"Epsilon Phi")} mode, bot is no longer Justibot Phi!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }

        [Group("levels")]
        public class levelModule : ModuleBase
        {
            [Command("enable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task addlevels()
            {
                bool saving = true;
                bool activator = true;
                string mode = "nill";
                ulong channelid = 123;
                string permission = "EXP";
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if ((activeuser.GuildPermissions.Has(GuildPermission.Administrator)) || (activeuser.Id == application.Owner.Id) || (Justibot.Loader.checkAdmin(activeuser.Id)))
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} enabled {Format.Bold("Leveling")}, server can now gain xp!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }

            }


            [Command("disable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task removeEpsilon()
            {
                bool saving = true;
                bool activator = false;
                string mode = "nill";
                string permission = "EXP";
                ulong channelid = 123;
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if ((activeuser.GuildPermissions.Has(GuildPermission.Administrator)) || (activeuser.Id == application.Owner.Id) || (Justibot.Loader.checkAdmin(activeuser.Id)))
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} disabled {Format.Bold($"Leveling")}, server can no longer gain xp. (global user xp still works)");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }

        [Group("music")]
        public class musicModule : ModuleBase
        {
            [Command("enable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task addmusic([OptionalAttribute] ITextChannel channel)
            {
                bool saving = true;
                bool activator = true;
                string mode = "nill";
                ulong channelid = 123;
                string permission = "MUSIC";
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    if (channel == null)
                    {
                        channelid = Context.Channel.Id;
                    }
                    else
                    {
                        channelid = channel.Id;
                    }

                    ITextChannel channel2 = await Context.Guild.GetTextChannelAsync(channelid) as ITextChannel;

                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} enabled permission: {Format.Bold($"{permission}")}, Music is now enabled in {channel2.Mention}!");
                        await ReplyAsync("WARNING: music is currently disabled by admin due to bugs.");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }

            }


            [Command("disable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task removeMusic()
            {
                bool saving = true;
                bool activator = false;
                string mode = "nill";
                string permission = "MUSIC";
                ulong channelid = 123;
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if ((activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id)))
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} disabled permission: {Format.Bold($"{permission}")}, Music is now disabled!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }


        [Group("advertising")]
        public class advertModule : ModuleBase
        {
            [Command("enable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task addAdvert(string moder, [OptionalAttribute] ITextChannel channel)
            {
                bool saving = true;
                bool activator = true;
                ulong channelid = 123;
                string mode = "nill";
                string permission = "ADVERTISING";
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    switch (moder.ToUpper())
                    {
                        case "REPOST":
                            mode = "REPOST";
                            moder = "reposting";
                            break;
                        case "DELETE":
                            mode = "DELETE";
                            moder = "deleting";
                            break;
                        default:
                            saving = false;
                            await ReplyAsync("invalid mode code, please use Repost/Delete.");
                            return;
                    }

                    if (channel == null)
                    {
                        channelid = Context.Channel.Id;
                    }
                    else
                    {
                        channelid = channel.Id;
                    }

                    ITextChannel channel2 = await Context.Guild.GetTextChannelAsync(channelid) as ITextChannel;

                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        if (mode == "REPOST")
                        {
                            await ReplyAsync($"{Context.User.Mention} enabled {moder} permission: {Format.Bold($"{permission}")} in channel: {channel2.Mention}!");
                        }
                        else if (mode == "DELETE")
                        {
                            await ReplyAsync($"{Context.User.Mention} enabled {moder} permission: {Format.Bold($"{permission}")}!");
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }

            }


            [Command("disable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task removeAdvert()
            {
                bool saving = true;
                bool activator = false;
                string mode = "nill";
                string permission = "ADVERTISING";
                ulong channelid = Context.Channel.Id;

                ITextChannel channel2 = await Context.Guild.GetTextChannelAsync(channelid) as ITextChannel;
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} disabled permission: {Format.Bold($"{permission}")}!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }


        [Group("Notes")]
        class noteModule : ModuleBase
        {
            [Command("enable")]
            public async Task enableN(string mode)
            {
                bool saving = true;
                bool activator = true;
                ulong channelid = 123;
                string permission = "NOTES";
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    switch (mode.ToUpper())
                    {
                        case "OPEN":
                            saving = true;
                            break;
                        case "CLOSED":
                            saving = true;
                            break;
                        default:
                            saving = false;
                            await ReplyAsync("Invalid mode please use open/closed.");
                            return;
                    }
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode.ToUpper());
                        if (mode.ToUpper() == "OPEN")
                        {
                            await ReplyAsync($"{Context.User.Mention} has enabled notes for public use!");
                        }
                        else if (mode.ToUpper() == "CLOSED")
                        {
                            await ReplyAsync($"{Context.User.Mention} has enabled server notes for staff use!");
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }

            [Command("disable")]
            public async Task disableGive()
            {
                bool activator = false;
                ulong channelid = 123;
                string permission = "NOTES";
                string mode2 = "nill";

                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    ulong hostID = Justibot.Loader.GetGHost(activeuser);
                    IGuildUser hoster = Context.Guild.GetUserAsync(hostID) as IGuildUser;
                    Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode2);
                    await ReplyAsync($"{Context.User.Mention} has disabled server notes!");

                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }


        [Group("bunker")]
        public class bunkerModule : ModuleBase
        {
            [Command("enable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task addStaff()
            {
                bool saving = true;
                bool activator = true;
                string mode = "nill";
                ulong channelid = 123;
                string permission = "BUNKER";
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} enabled permission: {Format.Bold($"{permission}")}, Bunker mode can now be enabled!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }

            }


            [Command("disable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task removeStaff()
            {
                bool activator = false;
                string mode = "nill";
                string permission = "BUNKER";
                ulong channelid = 123;
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                var guild = Context.Guild;
                if ((activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id)))
                {
                    if (Settings.bunker.Contains(activeuser.GuildId))
                    {
                        await ReplyAsync("Bunker mode cannot be deactivated while bunker mode is active, please disable bunker mode and try again.");
                    }
                    else
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} disabled permission: {Format.Bold($"{permission}")}, Bunker mode can no longer be enabled.");
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }

        [Group("log")]
        public class logModule : ModuleBase
        {
            [Command("enable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task addlog(string moder, [OptionalAttribute] ITextChannel channel)
            {
                bool saving = true;
                bool activator = true;
                ulong channelid = 123;
                string mode = "nill";
                string permission = "LOG";
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    switch (moder.ToUpper())
                    {
                        case "EMBED":
                            mode = "EMBED";
                            moder = "embeded";
                            break;
                        case "PLAIN":
                            mode = "PLAIN";
                            moder = "plain";
                            break;
                        default:
                            saving = false;
                            await ReplyAsync("invalid mode code, please use Embed/Plain.");
                            break;
                    }

                    if (channel == null)
                    {
                        channelid = Context.Channel.Id;
                    }
                    else
                    {
                        channelid = channel.Id;
                    }

                    ITextChannel channel2 = await Context.Guild.GetTextChannelAsync(channelid) as ITextChannel;

                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} enabled {moder} permission: {Format.Bold($"{permission}")} in channel: {channel2.Mention}!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }

            }


            [Command("disable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task removelog()
            {
                bool saving = true;
                bool activator = false;
                string mode = "nill";
                string permission = "LOG";
                ulong channelid = Context.Channel.Id;

                ITextChannel channel2 = await Context.Guild.GetTextChannelAsync(channelid) as ITextChannel;
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id || Justibot.Loader.checkAdmin(activeuser.Id))
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} disabled permission: {Format.Bold($"{permission}")}!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }

        [Group("Beta")]
        [RequireOwnerAttribute]
        public class betaModule : ModuleBase
        {
            [Command("enable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task addBeta()
            {
                bool saving = true;
                bool activator = true;
                string mode = "nill";
                ulong channelid = 123;
                string permission = "BETA";
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.Id == application.Owner.Id)
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} enabled {Format.Bold("Beta")} features, early access features are now available!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }

            }


            [Command("disable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task removeBeta()
            {
                bool saving = true;
                bool activator = false;
                string mode = "nill";
                string permission = "BETA";
                ulong channelid = 123;
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.Id == application.Owner.Id)
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} disabled {Format.Bold($"Beta")} features, can no longer use early access features!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }


        [Group("NightWolves")]
        [RequireOwnerAttribute]
        public class wolvesModule : ModuleBase
        {
            [Command("enable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task addwolves()
            {
                bool saving = true;
                bool activator = true;
                string mode = "nill";
                ulong channelid = 123;
                string permission = "WOLVES";
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.Id == application.Owner.Id)
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} enabled the {Format.Bold("NightWolves")} module, NightWolves commands are now available!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }

            }


            [Command("disable")]
            [Summary("adds/updates server permissions for module usage")]
            public async Task removeWolves()
            {
                bool saving = true;
                bool activator = false;
                string mode = "nill";
                string permission = "WOLVES";
                ulong channelid = 123;
                var application = await Context.Client.GetApplicationInfoAsync();
                var activeuser = Context.User as IGuildUser;
                if (activeuser.Id == application.Owner.Id)
                {
                    if (saving == true)
                    {
                        Justibot.Saver.SavePerm(activeuser, permission.ToUpper(), activator, channelid, mode);
                        await ReplyAsync($"{Context.User.Mention} disabled the {Format.Bold($"NightWolves")} module, NightWolves commands are no longer available!");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }

    }
}