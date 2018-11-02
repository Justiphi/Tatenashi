using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Justibot.Services;
using System.Runtime.InteropServices;

namespace Justibot.Modules.Modules
{

    [Group("giveaway")]
    public class giveaway : ModuleBase
    {

        [Command("new")]
        public async Task Gnew([Remainder] string prize)
        {
            var activeuser = Context.User as IGuildUser;
            var prefix = PrefixService.getPrefix(Context.Guild.Id);
            var application = await Context.Client.GetApplicationInfoAsync();
            var activator = true;
            var gives = Justibot.Loader.LoadGiveaway(activeuser);
            var results = Justibot.Loader.LoadPerm(activeuser, "GIVEAWAY");
            ITextChannel channel = await Context.Guild.GetTextChannelAsync(results.Item2) as ITextChannel;
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

        [Command("finish")]
        public async Task finishgive()
        {
            var activeuser = Context.User as IGuildUser;
            ulong hostID = Justibot.Loader.GetGHost(activeuser);
            IGuildUser hoster = Context.Guild.GetUserAsync(hostID) as IGuildUser;
            var gives = Justibot.Loader.LoadGiveaway(activeuser);

            if (gives.Item1 == true)
            {
                string prize = "nill";
                bool activator = false;
                var application = await Context.Client.GetApplicationInfoAsync();
                var results = Justibot.Loader.LoadPerm(activeuser, "GIVEAWAY");
                ITextChannel channel = await Context.Guild.GetTextChannelAsync(results.Item2) as ITextChannel;
                if (Context.User.Id == hostID || activeuser.GuildPermissions.Has(GuildPermission.Administrator) || Context.User == application.Owner)
                {
                    Justibot.Saver.SaveGiveaway(activeuser, prize, activator, results.Item2);
                    Justibot.Saver.clearAllEntries(activeuser);
                    await ReplyAsync($"{Context.User.Mention} has ended the giveaway");
                }
                else
                {
                    await ReplyAsync($"You do not have permission to end the giveaway hosted by {hoster.Username}.");
                }
            }
            else
            {
                await ReplyAsync("no giveaway to finish.");
            }

        }

        [Command("details")]
        public async Task GDetails()
        {
            var activeuser = Context.User as IGuildUser;
            var prefix = PrefixService.getPrefix(Context.Guild.Id);
            int entcount = Justibot.Loader.entrycount(activeuser);
            ulong hostID = Justibot.Loader.GetGHost(activeuser);
            var hoster = await Context.Guild.GetUserAsync(hostID) as IUser;
            var gives = Justibot.Loader.LoadGiveaway(activeuser);
            string prize = gives.Item3;
            ITextChannel channel = await Context.Guild.GetTextChannelAsync(gives.Item2) as ITextChannel;

            if (gives.Item1 == true)
            {
                await channel.SendMessageAsync($"{activeuser.Mention}, {hoster.Username} is hosting a giveaway for {Format.Bold($"{prize}!!!")} Use `{prefix}giveaway enter` to be in to win!!! currently {entcount} entries.");
            }
            else
            {
                await ReplyAsync("There are currently no giveaways running.");
            }
        }

        [Command("enter")]
        public async Task enterg()
        {
            bool isentered = false;
            var activeuser = Context.User as IGuildUser;
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

        [Command("draw")]
        public async Task drawGive()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var gives = Justibot.Loader.LoadGiveaway(activeuser);
            if (Context.Channel.Id == gives.Item2)
            {
                string prize = gives.Item3;
                if (gives.Item1 == true)
                {
                    ulong hostID = Justibot.Loader.GetGHost(activeuser);
                    IGuildUser hoster = await Context.Guild.GetUserAsync(hostID) as IGuildUser;
                    if (Context.User == hoster || activeuser.GuildPermissions.Has(GuildPermission.ManageGuild))
                    {
                        Random rand = Data.rand;
                        int entcount = Justibot.Loader.entrycount(activeuser);
                        int win = rand.Next(entcount);
                        ulong winnerID = Justibot.Loader.getWinner(activeuser, win);
                        IGuildUser winner = await Context.Guild.GetUserAsync(winnerID) as IGuildUser;
                        await ReplyAsync($"Congratulations {winner.Mention} for winning {Format.Bold($"{prize}!!!")} Please see {hoster.Mention} to collect your prize.");
                    }
                    else
                    {
                        await ReplyAsync($"You do not have permission to draw this Giveaway, please see {hoster.Mention} to draw.");
                    }
                }
                else
                {
                    await ReplyAsync("There is currently no giveaway active to draw from.");
                }
            }
        }

        [Command("clear")]
        public async Task clearGiveaway()
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            IGuildUser activeuser = Context.User as IGuildUser;
            var gives = Justibot.Loader.LoadGiveaway(activeuser);
            if (Context.Channel.Id == gives.Item2)
            {
                if (gives.Item1 == true)
                {
                    ulong hostID = Justibot.Loader.GetGHost(activeuser);
                    IGuildUser hoster = await Context.Guild.GetUserAsync(hostID) as IGuildUser;
                    if (Context.User == hoster || activeuser.GuildPermissions.Has(GuildPermission.Administrator) || activeuser.Id == application.Owner.Id)
                    {
                        Justibot.Saver.clearEntries(activeuser);
                        await ReplyAsync($"{activeuser.Mention} giveaway entries have been cleared!");
                    }
                    else
                    {
                        await ReplyAsync("You do not have permission to clear giveaway entries.");
                    }
                }
                else
                {
                    await ReplyAsync("There are no giveaways to clear entries for.");
                }
            }
        }

        [Command("drawAll")]
        public async Task drawAll(string args, [OptionalAttribute][Remainder] string prize)
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            List<SocketGuildUser> users = null;
            var results = Justibot.Loader.LoadPerm(activeuser, "GIVEAWAY");
            bool check = true;
            if (results.Item1)
            {
                if (results.Item2 == Context.Channel.Id)
                {
                    if (results.Item3 == "OPEN" || (results.Item3 == "CLOSED" && activeuser.GuildPermissions.Has(GuildPermission.ManageGuild)))
                    {
                        switch (args.ToUpper())
                        {
                            case "ALL":
                                users = (Context.Guild as SocketGuild).Users.Where(x => x.IsBot != true).ToList();
                                break;
                            case "ONLINE":
                                users = (Context.Guild as SocketGuild).Users.Where(x => (x.Status == UserStatus.Online || x.Status == UserStatus.Idle || x.Status == UserStatus.DoNotDisturb || x.Status == UserStatus.AFK || x.Status == UserStatus.Invisible) && x.IsBot != true).ToList();
                                break;
                            case "ACTIVE":
                                users = (Context.Guild as SocketGuild).Users.Where(x => (x.Status == UserStatus.Online) && x.IsBot != true).ToList();
                                break;
                            case "OFFLINE":
                                users = (Context.Guild as SocketGuild).Users.Where(x => (x.Status == UserStatus.Offline) && x.IsBot != true).ToList();
                                break;
                            case "CHANNEL":
                                users = (Context.Channel as SocketGuildChannel).Users.Where(x => x.IsBot != true).ToList();
                                break;
                            default:
                                await ReplyAsync("invalid mode code, please use All/Online/Offline/Active/Channel.");
                                check = false;
                                break;
                        }
                        if (check)
                        {
                            if (users.Count != 0)
                            {
                                SocketGuildUser winner = users[Data.rand.Next(users.Count)];
                                if (prize == null)
                                {
                                    await ReplyAsync($"{winner.Mention} has won the surprise giveaway hosted by {Context.User.Mention}!");
                                }
                                else
                                {
                                    await ReplyAsync($"{winner.Mention} has won the surprise giveaway hosted by {Context.User.Mention} for {prize}!");
                                }
                            }
                            else
                            {
                                await ReplyAsync("No users match this mode.");
                            }
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

}
