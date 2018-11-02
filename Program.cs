using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Justibot.Modules.Public;
using Justibot.Services;
using Discord.Rest;

namespace Justibot
{
    public class Program
    {
        public static void Main(string[] args) =>
            new Program().Start().GetAwaiter().GetResult();

        private DiscordSocketClient client;
        private CommandHandler handler;

        PublicModule module = new PublicModule();

        DateTime startTime = DateTime.Now;

        public async Task Start()
        {
            Configuration.configure();

            client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Info
            });

            client.Log += (message) =>
            {
                Console.WriteLine($"{System.DateTime.Now.ToString()} {message}");
                return Task.CompletedTask;
            };

            client.Ready += async () =>
            {
                await client.SetGameAsync("Use +help");
                PrefixService.LoadPrefixs(client);
                WelcomeService.LoadWelcomes(client);
                LeavingService.Loadleaving(client);
                XpService.Loadxps(client);
                XpService.timerStart();
                if (Justibot.Loader.checkUpdate().Value != Justibot.Data.version)
                {
                    Justibot.Saver.addVersion(DateTime.Now, Justibot.Data.version);
                }
            };

            await client.LoginAsync(TokenType.Bot, Configuration.config.Token);
            await client.StartAsync();

            client.UserJoined += async (user) =>
            {
                var results = Justibot.Loader.LoadPerm(user, "WELCOME");
                var guild = user.Guild as IGuild;
                if (results.Item1 == true)
                {
                    ulong result2 = results.Item2;
                    SocketTextChannel welcomeChannel = user.Guild.GetChannel(result2) as SocketTextChannel;

                    bool check = welcomedict.welcomes.TryGetValue(guild.Id, out string welcome);
                    if (!check)
                    {
                        welcome = "Welcome, **[user]** has joined **[server]!!!** \n" +
                        "Have a good time!!!";
                    }
                    welcome = welcome.Replace("[user]", user.Username);
                    welcome = welcome.Replace("[server]", guild.Name);

                    if (results.Item3 == "PLAIN")
                    {
                        await welcomeChannel.SendMessageAsync(welcome);
                    }
                    else
                    {
                        string avatar = user.GetAvatarUrl();
                        if (avatar == null)
                        {
                            avatar = "https://cdn.discordapp.com/embed/avatars/0.png";
                        }
                        Color color = new Color(0, 255, 0);
                        if (avatar.Contains("/a_"))
                        {
                            avatar = $"{avatar.Remove(avatar.Length - 12)}gif?size=128";
                        }

                        var embed = new EmbedBuilder()
                        .WithColor(color)
                        .WithTitle($"{Format.Bold($"{client.CurrentUser.Username}:")}")
                        .WithDescription(welcome)
                        .WithThumbnailUrl(avatar)
                        .Build();

                        await welcomeChannel.SendMessageAsync(user.Mention, false, embed);
                    }
                }
                var joinRoles = Justibot.Loader.LoadPerm(user, "JOINROLE");
                if (joinRoles.Item1 == true)
                {
                    IRole joinerRole = user.Guild.GetRole(joinRoles.Item2) as IRole;
                    await user.AddRoleAsync(joinerRole);
                }
                var results2 = Justibot.Loader.LoadPerm(user, "LOG");
                if (results2.Item1 == true)
                {
                    ulong result2 = results2.Item2;
                    SocketTextChannel welcomeChannel = user.Guild.GetChannel(result2) as SocketTextChannel;

                    if (results2.Item3 == "PLAIN")
                    {
                        await welcomeChannel.SendMessageAsync($"{Format.Bold($"{user.Username}")}#{user.Discriminator}, ID:<{user.Id}> has joined the server. \n" +
                            $"There are now {user.Guild.MemberCount} members.");
                    }
                    else
                    {
                        string avatar = user.GetAvatarUrl();
                        Color color = new Color(0, 255, 0);
                        if (avatar == null)
                        {
                            avatar = "https://cdn.discordapp.com/embed/avatars/0.png";
                        }
                        if (avatar.Contains("/a_"))
                        {
                            avatar = $"{avatar.Remove(avatar.Length - 12)}gif?size=128";
                        }

                        var embed = new EmbedBuilder()
                        .WithColor(color)
                        .WithTitle($"{Format.Bold($"{client.CurrentUser.Username}:")}")
                        .WithDescription($"{Format.Bold($"{user.Username}")}#{user.Discriminator}, ID:<{user.Id}> has joined the server. \n" +
                            $"There are now {user.Guild.MemberCount} members.")
                        .WithThumbnailUrl(avatar)
                        .Build();

                        await welcomeChannel.SendMessageAsync("", false, embed);
                    }
                }
                if (Settings.bunker.Contains(guild.Id))
                {
                    await user.Guild.DefaultChannel.SendMessageAsync($"{Format.Bold($"{user.Username}")} was kicked by bunker mode!");
                    await user.KickAsync();
                }
            };

            client.UserLeft += async (user) =>
            {
                var results = Justibot.Loader.LoadPerm(user, "LEAVING");
                if (results.Item1 == true)
                {
                    ulong result2 = results.Item2;
                    var guild = user.Guild as IGuild;
                    SocketTextChannel welcomeChannel = user.Guild.GetChannel(result2) as SocketTextChannel;

                    bool check = welcomedict.leaves.TryGetValue(guild.Id, out string welcome);

                    if (results.Item3 == "PLAIN")
                    {
                        if (!check)
                        {
                            welcome = "**[mention]** has left **[server]**, goodbye.";
                        }
                        welcome = welcome.Replace("[user]", user.Username);
                        welcome = welcome.Replace("[mention]", user.Mention);
                        welcome = welcome.Replace("[server]", guild.Name);
                        await welcomeChannel.SendMessageAsync(welcome);
                    }
                    else
                    {
                        if (!check)
                        {
                            welcome = "**[user]** has left **[server]**, goodbye.";
                        }
                        welcome = welcome.Replace("[user]", user.Username);
                        welcome = welcome.Replace("[mention]", user.Mention);
                        welcome = welcome.Replace("[server]", guild.Name);
                        string avatar = user.GetAvatarUrl();
                        Color color = new Color(255, 0, 0);
                        if (avatar == null)
                        {
                            avatar = "https://cdn.discordapp.com/embed/avatars/0.png";
                        }
                        if (avatar.Contains("/a_"))
                        {
                            avatar = $"{avatar.Remove(avatar.Length - 12)}gif?size=128";
                        }

                        var embed = new EmbedBuilder()
                        .WithColor(color)
                        .WithTitle($"{Format.Bold($"{client.CurrentUser.Username}:")}")
                        .WithDescription(welcome)
                        .WithThumbnailUrl(avatar)
                        .Build();

                        await welcomeChannel.SendMessageAsync("", false, embed);
                    }
                }

                var results2 = Justibot.Loader.LoadPerm(user, "LOG");
                if (results2.Item1 == true)
                {
                    ulong result2 = results2.Item2;
                    var guild = user.Guild as IGuild;
                    SocketTextChannel welcomeChannel = user.Guild.GetChannel(result2) as SocketTextChannel;

                    if (results2.Item3 == "PLAIN")
                    {
                        await welcomeChannel.SendMessageAsync($"{Format.Bold($"{user.Username}")}#{user.Discriminator}, ID:<{user.Id}> has left the server. \n" +
                            $"There are now {user.Guild.MemberCount} members.");
                    }
                    else
                    {
                        string avatar = user.GetAvatarUrl();
                        Color color = new Color(255, 0, 0);
                        if (avatar == null)
                        {
                            avatar = "https://cdn.discordapp.com/embed/avatars/0.png";
                        }
                        if (avatar.Contains("/a_"))
                        {
                            avatar = $"{avatar.Remove(avatar.Length - 12)}gif?size=128";
                        }

                        var embed = new EmbedBuilder()
                        .WithColor(color)
                        .WithTitle($"{Format.Bold($"{client.CurrentUser.Username}:")}")
                        .WithDescription($"{Format.Bold($"{user.Username}")}#{user.Discriminator}, ID:<{user.Id}> has left the server. \n" +
                            $"There are now {user.Guild.MemberCount} members.")
                        .WithThumbnailUrl(avatar)
                        .Build();

                        await welcomeChannel.SendMessageAsync("", false, embed);
                    }
                }

            };

            client.MessageReceived += async (message) =>
            {
                var guild = (message.Channel as SocketTextChannel)?.Guild;
                var user = (message.Author as IGuildUser);
                if (guild != null)
                {
                    if (user.IsBot == false)
                    {
                        var guild2 = guild as IGuild;
                        XpService.AddXp(user, guild2);
                    }
                    if ((message.ToString().ToUpper()).Contains("DISCORD.GG/") && (!(user.IsBot)))
                    {
                        if (!(user.GuildPermissions.Has(GuildPermission.ManageGuild)) || !(Justibot.Loader.isStaff(user)))
                        {
                            var results = Justibot.Loader.LoadPerm(user, "ADVERTISING");
                            if (results.Item1 == true)
                            {
                                ulong result2 = results.Item2;
                                if (results.Item3 == "DELETE")
                                {
                                    await message.DeleteAsync();
                                    var response = await message.Channel.SendMessageAsync($"{user.Mention} please do not advertise in this server");
                                    respond(response);
                                }
                                else if (results.Item3 == "REPOST")
                                {
                                    SocketTextChannel welcomeChannel = guild.GetChannel(result2) as SocketTextChannel;
                                    if (message.Channel != welcomeChannel)
                                    {
                                        await welcomeChannel.SendMessageAsync($"{user.Mention} advertised message: \n{message.ToString()}");
                                        await message.DeleteAsync();
                                        var response = await message.Channel.SendMessageAsync($"{user.Mention} please keep advertising to {welcomeChannel.Mention}, your message has been shifted there.");
                                        respond(response);
                                    }
                                }
                            }
                        }
                    }
                }
                
            };
            handler = new CommandHandler();
            await handler.InitCommands(client);

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            if(msg.ToString().Contains("Failed to resume previous session"))
            {
                restartBot();
            }
            return Task.CompletedTask;
        }

        private async void respond(RestUserMessage response)
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

        public void restartBot()
        {
            client.Dispose();
            new Program().Start().GetAwaiter().GetResult();
        }

    }
}
