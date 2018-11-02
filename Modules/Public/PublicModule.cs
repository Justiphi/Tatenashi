using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Justibot;

namespace Justibot.Modules.Public
{
    public class PublicModule : ModuleBase
    {
        [Command("say")]
        [Summary("Echos the provided input")]
        public async Task Say([Remainder] string input)
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            var activeuser = Context.User as IGuildUser;
            if (activeuser.GuildPermissions.Has(GuildPermission.ManageMessages) || activeuser.Id == application.Owner.Id)
            {
                await ReplyAsync(input);
                await Context.Message.DeleteAsync();
            }
            else
            {
                await ReplyAsync("You do not have permission to use this command.");
            }
        }

        [Command("sayin")]
        [Summary("Echos the provided input")]
        public async Task Say(ITextChannel channel, [Remainder] string input)
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            var activeuser = Context.User as IGuildUser;
            if (activeuser.GuildPermissions.Has(GuildPermission.ManageMessages) || activeuser.Id == application.Owner.Id)
            {
                await channel.SendMessageAsync(input);
            }
            else
            {
                await ReplyAsync("You do not have permission to use this command.");
            }
        }

        [Command("echo")]
        [Summary("Echos the provided input")]
        public async Task Echo([Remainder] string input)
        {
            var user = Context.User as IGuildUser;
            if (!(Context.Message.ToString().ToUpper()).Contains("DISCORD.GG/") || (user.GuildPermissions.Has(GuildPermission.ManageGuild)))
            {
                await ReplyAsync($"{Context.User.Mention} {input}");
            }
            else
            {
                var results = Justibot.Loader.LoadPerm(user, "ADVERTISING");
                if (results.Item1 == true)
                {
                    var message = await ReplyAsync("Advertising is blocked on this server, link has been blocked.");
                    respond(message);
                }
                else
                {
                    await ReplyAsync($"{Context.User.Mention} {input}");
                }
            }
        }

        private async void respond(IUserMessage response)
        {
            await Task.Delay(3000);
            if (response.Channel.GetMessageAsync(response.Id) != null)
            {
                try
                {
                    await response.DeleteAsync();
                }
                catch
                { }
            }
        }

        [Command("info")]
        public async Task embed()
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            int usercount = Justibot.Loader.usercount(Context);
            string message = (
                $"- {Format.Bold("Library:")} Discord.Net ({DiscordConfig.Version})\n" +
                $"- {Format.Bold("Runtime:")} {RuntimeInformation.FrameworkDescription} {RuntimeInformation.OSArchitecture}"
            );
            string message2 = (
                $"- {Format.Bold("Official Server:")} [https://discord.gg/gS6zZ3B](https://discord.gg/gS6zZ3B) \n" +
                $"- {Format.Bold("Official Website/Documentation:")} [http://tatenashi.com](http://tatenashi.com)"
            );
            string message3 = (
                $"- {Format.Bold("Heap Size:")} {GetHeapSize()} MB\n" +
                $"- {Format.Bold("Guilds")}: {(Context.Client as DiscordSocketClient).Guilds.Count}\n" +
                $"- {Format.Bold("Channels:")} {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Channels.Count)}" +
                $"- {Format.Bold("Users:")} {usercount}"
            );
            string message4 = (
                $"- {Format.Bold("Author:")} {application.Owner.Username} (ID {application.Owner.Id})\n" +
                $"- {Format.Bold("Uptime:")} {GetUptime()} \n" +
                $"- {Format.Bold("Time since update:")} {GetUpdatetime()} \n" +
                $"- {Format.Bold("Bot Build:")} {Context.Client.CurrentUser.Username} {Justibot.Data.version}"
            );
            var avatar = application.IconUrl;
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
                .AddField(x => { x.Name = "Info:"; x.Value = message; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Build:"; x.Value = message4; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Stats:"; x.Value = message3; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Sites:"; x.Value = message2; x.WithIsInline(false); })
                .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }


        [Command("userinfo")]
        [Summary("displays info about a user")]
        public async Task userinfo([OptionalAttribute] IGuildUser user)
        {
            string nickname = "none";
            IGuildUser self = Context.User as IGuildUser;
            var uID = self.Id;
            var userN = self.Username;
            var time = self.CreatedAt;
            var joined = self.JoinedAt;
            var mention = self.Mention;
            var Descrim = self.Discriminator;
            var avatar = self.GetAvatarUrl();
            var avatar2 = self.GetAvatarUrl();
            if (self.Nickname != null && user == null)
            {
                nickname = self.Nickname;
            }

            if (user != null)
            {
                uID = user.Id;
                userN = user.Username;
                time = user.CreatedAt;
                joined = user.JoinedAt;
                mention = user.Mention;
                Descrim = user.Discriminator;
                avatar = user.GetAvatarUrl();
                avatar2 = user.GetAvatarUrl();
                if (user.Nickname != null)
                {
                    nickname = user.Nickname;
                }
            }
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
                .AddField(x => { x.Name = "Nickname:"; x.Value = nickname; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Descrim:"; x.Value = Descrim; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Created at:"; x.Value = time.ToString().Remove(time.ToString().Length - 6); x.WithIsInline(true); })
                .AddField(x => { x.Name = "Joined at:"; x.Value = joined.ToString().Remove(joined.ToString().Length - 6); x.WithIsInline(true); x.IsInline = true; })
                .AddField(x => { x.Name = "Avatar:"; x.Value = footer; x.WithIsInline(false); })
                .Build();

            await Context.Channel.SendMessageAsync($"{mention}", false, embed);
        }

        [Command("serverinfo")]
        [Summary("displays info about the server")]
        public async Task ServerInfo()
        {
            var guild = Context.Guild as SocketGuild;
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

        private static string GetUptime()
            => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");

        private static string GetUpdatetime()
            => (DateTime.Now - Justibot.Loader.checkUpdate().Key).ToString(@"dd\.hh\:mm\:ss");

        private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();

        [Command("donate")]
        [Summary("Dontate to help with server costs!")]
        public async Task donate()
        {
            await ReplyAsync("https://www.patreon.com/justice586");
        }

        [Command("cool")]
        public async Task cool()
        {
            var msg = await ReplyAsync("( ͡° ͜ʖ ͡°)>⌐■-■");
            await Task.Delay(1500);
            await msg.ModifyAsync(x => { x.Content = "( ͡⌐■ ͜ʖ ͡-■)"; });
        }

        [Command("invite")]
        public async Task invite()
        {
            await ReplyAsync("To invite Tatenashi to your server (requires manage server) use: \n" +
            "https://discordapp.com/oauth2/authorize?client_id=277908880359686145&scope=bot&permissions=305261631 \n\n" +
            "To join the Tatenashi Official Discord server, join:" +
            "https://discord.gg/gS6zZ3B");
        }

        [Command("avatar")]
        [Summary("Shows the bots avatar")]
        public async Task avatar()
        {
            IUser admin = Context.User;
            var application = await Context.Client.GetApplicationInfoAsync();
            if ((Justibot.Loader.checkAdmin(admin.Id)) || (admin == application.Owner))
            {
                await Context.Channel.SendFileAsync("Resources/Images/Avatar.png");
            }
            await ReplyAsync($"Avatar created by {Format.Bold("Hecatiartz")} \n" +
            "You can order custom work from him by visiting: https://www.fiverr.com/hecatiartz");
        }

        [Command("checkAdmin")]
        public async Task addStaff(IUser admin)
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            if ((Justibot.Loader.checkAdmin(admin.Id)) || (admin.Id == application.Owner.Id))
            {
                await ReplyAsync($"{admin.Mention} is an admin for {Format.Bold($"{Context.Client.CurrentUser.Username}!")}");
            }
            else
            {
                await ReplyAsync($"{admin.Mention} is not an admin for {Format.Bold($"{Context.Client.CurrentUser.Username}")}.");
            }
        }

        [Command("Kachow")]
        public async Task kachow()
        {
            int treeNumber = Data.rand.Next(Data.kachow.Length);
            string postedTree = Data.kachow[treeNumber];
            await Context.Channel.SendFileAsync(postedTree);
        }

        [Command("interact")]
        public async Task interact(IUser user)
        {
            int treeNumber = Data.rand.Next(Data.interactions.Length);
            string postedTree = Data.interactions[treeNumber];
            await Context.Channel.SendMessageAsync($"{Context.User.Mention} {postedTree} {user.Mention}!");
            Services.statsService.interactRecieved(Context.Guild.Id);
        }

        [Command("8ball")]
        public async Task eightball()
        {
            int treeNumber = Data.rand.Next(Data.eightball.Length);
            string postedTree = Data.eightball[treeNumber];
            await Context.Channel.SendMessageAsync($"{Context.User.Mention} {postedTree}!");
        }

        [Command("stats")]
        public async Task statsembed()
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            int usercount = Justibot.Loader.usercount(Context);
            var commands = Services.statsService.getCommands(Context.Guild.Id);
            var inters = Services.statsService.getInteracts(Context.Guild.Id);
            var xps = Services.statsService.getXp(Context.Guild.Id);
            var mess = Services.statsService.getMessages(Context.Guild.Id);
            string message = (
                $"- {Format.Bold("Library:")} Discord.Net ({DiscordConfig.Version})\n" +
                $"- {Format.Bold("Runtime:")} {RuntimeInformation.FrameworkDescription} {RuntimeInformation.OSArchitecture}"
            );
            string message3 = (
                $"- {Format.Bold("Heap Size:")} {GetHeapSize()} MB\n" +
                $"- {Format.Bold("Guilds")}: {(Context.Client as DiscordSocketClient).Guilds.Count}\n" +
                $"- {Format.Bold("Channels:")} {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Channels.Count)}" +
                $"- {Format.Bold("Users:")} {usercount}"
            );
            string message4 = (
                $"- {Format.Bold("Uptime:")} {GetUptime()} \n" +
                $"- {Format.Bold("Time since update:")} {GetUpdatetime()} \n"
            );
            string message2 = (
                $"- {Format.Bold("This server:")} {commands.Item1} \n" +
                $"- {Format.Bold("Global:")} {commands.Item2} \n"
            );
            string message5 = (
                $"- {Format.Bold("This server:")} {xps.Item1} \n" +
                $"- {Format.Bold("Global:")} {xps.Item2} \n"
            );
            string message6 = (
                $"- {Format.Bold("This server:")} {inters.Item1} \n" +
                $"- {Format.Bold("Global:")} {inters.Item2} \n"
            );
            string message7 = (
                $"- {Format.Bold("This server:")} {mess.Item1} \n" +
                $"- {Format.Bold("Global:")} {mess.Item2} \n"
            );
            var avatar = application.IconUrl;
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
                .AddField(x => { x.Name = "Info:"; x.Value = message; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Uptime:"; x.Value = message4; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Stats:"; x.Value = message3; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Commands Used:"; x.Value = message2; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Interactions Used:"; x.Value = message6; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Xp Gained:"; x.Value = message5; x.WithIsInline(true); })
                .AddField(x => { x.Name = "messages recieved:"; x.Value = message7; x.WithIsInline(true); })
                .WithDescription("Note: Commands, interactions and xp gained is measured over the current uptime.")
                .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
