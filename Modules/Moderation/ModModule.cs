using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Runtime.InteropServices;
using Justibot.Services;

namespace Justibot.Modules.Moderation
{
    public class ModModule : ModuleBase
    {
        //initiates kicking of a user 
        [Command("kick")]
        [RequireBotPermissionAttribute(GuildPermission.KickMembers)]
        [Summary("Kicks a user from the server")]
        public async Task kick(IGuildUser UserToKick)
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            var activeuser = Context.User as IGuildUser;
            //checks if initiating user has right to kick member or is the application owner
            if (activeuser.GuildPermissions.Has(GuildPermission.KickMembers) || activeuser.Id == application.Owner.Id)
            {
                if (Context.Message.MentionedUserIds.Count() < 1)
                {
                    await Context.Channel.SendMessageAsync(Context.User.Mention + " That's not a valid user!");
                }
                else
                {
                    try
                    {
                        await UserToKick.KickAsync();
                        await Context.Channel.SendMessageAsync(UserToKick.Username + " was kicked by " + Context.User.Mention + "!");
                    }
                    catch
                    {
                        await Context.Channel.SendMessageAsync(Context.User.Mention + " I do not have permission to kick that user!");
                    }
                }
            }
            else
            {
                await ReplyAsync("You do not have permission to use this command");
            }
        }

        //initiates banning a user from the server
        [Command("ban")]
        [RequireBotPermissionAttribute(GuildPermission.BanMembers)]
        [Summary("Bans a user from the server")]
        public async Task ban(IGuildUser UserToban)
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            var activeuser = Context.User as IGuildUser;
            //checks if initiating user has right to ban or is the application owner
            if (activeuser.GuildPermissions.Has(GuildPermission.BanMembers) || activeuser.Id == application.Owner.Id)
            {
                if (Context.Message.MentionedUserIds.Count() < 1)
                {
                    await Context.Channel.SendMessageAsync(Context.User.Mention + " That's not a valid user!");
                }
                else
                {
                    try
                    {
                        await Context.Guild.AddBanAsync(UserToban);
                        await Context.Channel.SendMessageAsync(UserToban.Username + " was banned by " + Context.User.Mention + "!");
                    }
                    catch
                    {
                        await Context.Channel.SendMessageAsync(Context.User.Mention + " I do not have permission to ban that user!");
                    }
                }
            }
            else
            {
                await ReplyAsync("You do not have permission to use this command");
            }
        }

        //initiates purging messages from the channel
        [Command("purge")]
        [Alias("prune", "clear", "cleanup")]
        [Summary("Deletes x amount of messages from a channel (25 by default, will not delete more than 100 at a time)")]
        [RequireBotPermissionAttribute(GuildPermission.ManageMessages)]
        public async Task Purge([OptionalAttribute] string x)
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            var activeuser = Context.User as IGuildUser;
            if (activeuser.GuildPermissions.Has(GuildPermission.ManageMessages) || activeuser.Id == application.Owner.Id)
            {
                int xmd = 25;
                int xm;
                if (x != null)
                {
                    if ((Int32.TryParse(x, out xm)))
                    {
                        xmd = xm;
                    }
                    else
                    {
                        await ReplyAsync("invalid value please make sure value is a number");
                        return;
                    }
                }
                if (!(Context.Channel is ITextChannel channel))
                {
                    await ReplyAsync("This command must be used from a guild text channel.");
                    return;
                }
                if (xmd <= 100 && xmd >= 0)
                {
                    if (xmd == 100)
                    {
                        var messagesToDelete = await Context.Channel.GetMessagesAsync(xmd, CacheMode.AllowDownload).FlattenAsync();
                        try
                        {
                            await channel.DeleteMessagesAsync(messagesToDelete);
                        }
                        catch
                        {
                            await ReplyAsync("Error deleting messages (messages ore probably too old, please try again with smaller number)");
                        }
                    }
                    else
                    {
                        var messagesToDelete = await Context.Channel.GetMessagesAsync(xmd + 1, CacheMode.AllowDownload).FlattenAsync();
                        try
                        {
                            await channel.DeleteMessagesAsync(messagesToDelete);
                        }
                        catch
                        {
                            await ReplyAsync("Error deleting messages (messages ore probably too old, please try again with smaller number)");
                        }
                    }
                    var confirmMessage = await Context.Channel.SendMessageAsync(Context.User.Mention + " " + xmd.ToString() + " messages have been deleted");
                    respond(confirmMessage);
                }
                else
                {
                    if (xmd > 100)
                    {
                        await Context.Channel.SendMessageAsync("number greater than 100 (unsupported) deleting 100 messages instead");
                        await Task.Delay(3000);
                        var messagesToDelete = await Context.Channel.GetMessagesAsync(100).FlattenAsync();
                        try
                        {
                            await channel.DeleteMessagesAsync(messagesToDelete);
                        }
                        catch
                        {
                            await ReplyAsync("Error deleting messages (messages ore probably too old, please try again with smaller number)");
                        }
                        var confirmMessage = await Context.Channel.SendMessageAsync(Context.User.Mention + " 100 messages have been deleted");
                        respond(confirmMessage);
                    }
                    else
                    {
                        var invalidMessage = await Context.Channel.SendMessageAsync("inavlid number please insert number between 0 and 100");
                        respond(invalidMessage);
                    }
                }
            }
            else
            {
                await ReplyAsync("You do not have permission to use this command");
            }
        }

        private async void respond(IUserMessage response)
        {
            await Task.Delay(3000);
            if(response.Channel.GetMessageAsync(response.Id) != null)
            {
                try
                {
                    await response.DeleteAsync();
                }
                catch
                {}
            }
        }
        //initiates muting a user
        [Command("Mute")]
        [Alias("silence")]
        [Summary("Mutes a user")]
        [RequireBotPermissionAttribute(GuildPermission.MuteMembers)]
        public async Task muteMem(IGuildUser loudMember)
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            var activeuser = Context.User as IGuildUser;
            //checks if the initiating user has the rights to mute users or is the application owner
            if (activeuser.GuildPermissions.Has(GuildPermission.MuteMembers) || activeuser.Id == application.Owner.Id)
            {
                if (Context.Message.MentionedUserIds.Count() < 1)
                {
                    await Context.Channel.SendMessageAsync(Context.User.Mention + " That's not a valid user!");
                }
                else
                {
                    try
                    {
                        await loudMember.ModifyAsync(x => x.Mute = true);
                        await Context.Channel.SendMessageAsync($"{loudMember.Mention} was Muted!");
                    }
                    catch
                    {
                        await Context.Channel.SendMessageAsync(Context.User.Mention + " I do not have permission to Mute that user!");
                    }
                }
            }
            else
            {
                await ReplyAsync("You do not have permission to use this command");
            }
        }

        //initiates unmuting a user
        [Command("Unmute")]
        [Summary("Unmutes a user")]
        [RequireBotPermissionAttribute(GuildPermission.MuteMembers)]
        public async Task unmuteMem(IGuildUser mutedMember)
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            var activeuser = Context.User as IGuildUser;
            //checks if initiating user has the right to unmute users or is the application owner
            if (activeuser.GuildPermissions.Has(GuildPermission.MuteMembers) || activeuser.Id == application.Owner.Id)
            {
                if (Context.Message.MentionedUserIds.Count() < 1)
                {
                    await Context.Channel.SendMessageAsync(Context.User.Mention + " That's not a valid user!");
                }
                else
                {
                    try
                    {
                        await mutedMember.ModifyAsync(x => x.Mute = false);
                        await Context.Channel.SendMessageAsync($"{mutedMember.Mention} was Unmuted!");
                    }
                    catch
                    {
                        await Context.Channel.SendMessageAsync(Context.User.Mention + " I do not have permission to Unmute that user!");
                    }
                }
            }
            else
            {
                await ReplyAsync("You do not have permission to use this command");
            }
        }

        [Command("setGame")]
        [RequireOwnerAttribute]
        public async Task game([RemainderAttribute]string game)
        {
            var dClient = Context.Client as DiscordSocketClient;
            await dClient.SetGameAsync(game);
            await ReplyAsync($"{Context.User.Mention}, game set to {Format.Bold($"{game}")}");
        }

        [Command("setPrefix")]
        public async Task prefix(char prefix)
        {
            var activeuser = Context.User as IGuildUser;
            var application = await Context.Client.GetApplicationInfoAsync();
            if (activeuser.GuildPermissions.Has(GuildPermission.Administrator) || activeuser.Id == application.Owner.Id)
            {
                PrefixService.addPrefixes(Context.Guild, prefix);
                Justibot.Saver.SavePrefix(activeuser, prefix);
                await ReplyAsync($"{Context.User.Mention}, set the prefix to {Format.Bold($"{prefix}")}");
            }
            else
            {
                await ReplyAsync("You do not have permission to use this command");
            }
        }

        //initiates the setting of the welcome message
        [Command("setWelcome")]
        public async Task welcome([RemainderAttribute]string prefix)
        {
            var activeuser = Context.User as IGuildUser;
            var application = await Context.Client.GetApplicationInfoAsync();
            //checks if initiating user has administrator rights or is application owner
            if (activeuser.GuildPermissions.Has(GuildPermission.Administrator) || activeuser.Id == application.Owner.Id)
            {
                WelcomeService.addWelcomes(Context.Guild, prefix);
                Justibot.Saver.SaveWelcomes(activeuser, prefix);
                await ReplyAsync($"{Context.User.Mention}, set the welcome message to: \n{prefix}");
            }
            else
            {
                await ReplyAsync("You do not have permission to use this command");
            }
        }

        //initiates the setting of the leaving message
        [Command("setLeaving")]
        public async Task leaving([RemainderAttribute]string prefix)
        {
            var activeuser = Context.User as IGuildUser;
            var application = await Context.Client.GetApplicationInfoAsync();
            //checks if the initiating user has adminstrator rights or is the application owner
            if (activeuser.GuildPermissions.Has(GuildPermission.Administrator) || activeuser.Id == application.Owner.Id)
            {
                LeavingService.addLeaves(Context.Guild, prefix);
                Justibot.Saver.SaveLeaves(activeuser, prefix);
                await ReplyAsync($"{Context.User.Mention}, set the leaving message to: \n{prefix}");
            }
            else
            {
                await ReplyAsync("You do not have permission to use this command");
            }
        }

        //initiates silencing a user
        [Command("silence")]
        public async Task Silence(IUser userToSilence)
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            IGuildUser user = userToSilence as IGuildUser;
            var activeuser = Context.User as IGuildUser;
            var silent = Justibot.Loader.LoadPerm(user, "SILENCE");
            if (silent.Item1 == true)
            {
                //checks if user has the rights to mute users and to manage messages, or is the application owner
                if ((activeuser.GuildPermissions.Has(GuildPermission.MuteMembers) && activeuser.GuildPermissions.Has(GuildPermission.ManageMessages)) || activeuser.Id == application.Owner.Id)
                {
                    bool isStaff = Justibot.Loader.isStaff(userToSilence as IGuildUser);
                    if (isStaff == false)
                    {
                        IRole joinerRole = user.Guild.GetRole(silent.Item2) as IRole;
                        await user.AddRoleAsync(joinerRole);
                        await ReplyAsync($"{activeuser.Mention} has silenced {userToSilence.Mention}, please appeal to a staff member via voice chat or DM to be allowed to talk again.");
                    }
                    else
                    {
                        await ReplyAsync("Cannot silence a staff member");
                    }
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command");
                }
            }
        }
        //initiates grating a user talking rights again
        [Command("talk")]
        public async Task talk(IUser userToSilence)
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            IGuildUser user = userToSilence as IGuildUser;
            var activeuser = Context.User as IGuildUser;
            var silent = Justibot.Loader.LoadPerm(user, "SILENCE");
            if (silent.Item1 == true)
            {
                //checks if user has the rights to mute users and to manage messages, or is the application owner
                if ((activeuser.GuildPermissions.Has(GuildPermission.MuteMembers) && activeuser.GuildPermissions.Has(GuildPermission.ManageMessages)) || activeuser.Id == application.Owner.Id)
                {
                    IRole joinerRole = user.Guild.GetRole(silent.Item2) as IRole;
                    await user.RemoveRoleAsync(joinerRole);
                    await ReplyAsync($"{userToSilence.Mention} may now talk again, courtesy of {activeuser.Mention}!!!");
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command");
                }
            }
        }

        //initiates promoting a user to staff
        [Command("addstaff")]
        public async Task addStaff(IRole role)
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            var activeuser = Context.User as IGuildUser;
            var silent = Justibot.Loader.LoadPerm(activeuser, "STAFF");
            if (silent.Item1 == true)
            {   
                //checks if initiating user has right to manage the guild
                if (activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id)
                {
                    Justibot.Saver.saveRole(activeuser, role.Id);
                    await ReplyAsync($"{role.Mention} is now a staff role, set by {activeuser.Mention}!");
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }

        //initiates the demotion of a user from staff to a normal user
        [Command("removestaff")]
        public async Task removeStaff(IRole role)
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            var activeuser = Context.User as IGuildUser;
            var silent = Justibot.Loader.LoadPerm(activeuser, "STAFF");
            if (silent.Item1 == true)
            {
                //checks if initiating user has right to manage the guild
                if (activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || activeuser.Id == application.Owner.Id)
                {
                    Justibot.Saver.deleteRole(activeuser, role.Id);
                    await ReplyAsync($"{role.Mention} is no longer a staff role, set by {activeuser.Mention}!");
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }

        //initiates adding a new role to the guild
        [Command("addRole")]
        public async Task addRole(IGuildUser user, IRole role)
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            var activeuser = Context.User as IGuildUser;
            //checks if the initiating user has the right to manage roles
            if (activeuser.GuildPermissions.Has(GuildPermission.ManageRoles) || activeuser.Id == application.Owner.Id)
            {
                await user.AddRoleAsync(role);
                await ReplyAsync($"{user.Mention} has been given the role: {role} by {activeuser.Mention}!");
            }
            else
            {
                await ReplyAsync("You do not have permission to use this command.");
            }

        }

        //initiates removing a role 
        [Command("removeRole")]
        public async Task removeRole(IGuildUser user, IRole role)
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            var activeuser = Context.User as IGuildUser;
            //checks if the initiating user has the right to manage roles
            if (activeuser.GuildPermissions.Has(GuildPermission.ManageRoles) || activeuser.Id == application.Owner.Id)
            {
                await user.RemoveRoleAsync(role);
                await ReplyAsync($"{user.Mention} has been revoked the role: {role} by {activeuser.Mention}!");
            }
            else
            {
                await ReplyAsync("You do not have permission to use this command.");
            }

        }

        //initiates setting bunker mode
        [Command("Bunker")]
        public async Task bunkermode(string mode)
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            var activeuser = Context.User as IGuildUser;
            if ((Justibot.Loader.LoadPerm(activeuser, "BUNKER")).Item1)
            {
                switch (mode.ToUpper())
                {
                    case "ENABLE":
                        if (Justibot.Loader.isStaff(activeuser) || activeuser.Id == application.Owner.Id)
                        {
                            if (Settings.bunker.Contains(activeuser.Guild.Id))
                            {
                                await ReplyAsync("Bunker mode is already enabled!");
                            }
                            else
                            {
                                Settings.bunker.Add(activeuser.Guild.Id);
                                await ReplyAsync("Bunker mode has been enabled!");
                            }
                        }
                        else
                        {
                            await ReplyAsync("You do not have permission to use this command");
                        }
                        break;
                    case "DISABLE":
                        if (Justibot.Loader.isStaff(activeuser) || activeuser.Id == application.Owner.Id)
                        {
                            if (Settings.bunker.Contains(activeuser.Guild.Id))
                            {
                                Settings.bunker.Remove(activeuser.Guild.Id);
                                await ReplyAsync("Bunker mode has been disabled!");
                            }
                            else
                            {
                                Settings.bunker.Add(activeuser.Guild.Id);
                                await ReplyAsync("Bunker mode is already disabled!");
                            }
                        }
                        else
                        {
                            await ReplyAsync("You do not have permission to use this command");
                        }
                        break;
                    case "CHECK":
                        if (Settings.bunker.Contains(activeuser.Guild.Id))
                        {
                            await ReplyAsync("Bunker mode is enabled!");
                        }
                        else
                        {
                            await ReplyAsync("Bunker mode is disabled!");
                        }
                        break;
                    default:
                        await ReplyAsync("Invalid parameter, please use enable/disable or check.");
                        break;
                }
            }
        }

    }
}