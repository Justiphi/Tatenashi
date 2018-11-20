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

            //create client and add what serverity of information to log
            client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Info
            });

            //create template for logged messages
            client.Log += (message) =>
            {
                Console.WriteLine($"{System.DateTime.Now.ToString()} {message}");
                return Task.CompletedTask;
            };

            //when client is ready, load important information to memory for quick access and set game to help message
            client.Ready += async () =>
            {
                await client.SetGameAsync("Use +help");
                PrefixService.LoadPrefixs(client);
                WelcomeService.LoadWelcomes(client);
                LeavingService.Loadleaving(client);
                XpService.Loadxps(client);

                //start timer for xp module
                XpService.timerStart();

                //if version from data file has changed, reset update time (used to track time since update)
                if (Justibot.Loader.checkUpdate().Value != Justibot.Data.version)
                {
                    Justibot.Saver.addVersion(DateTime.Now, Justibot.Data.version);
                }
            };

            //log the client into discord with the bot token
            await client.LoginAsync(TokenType.Bot, Configuration.config.Token);
            await client.StartAsync();

            //when a user joins a guild, check appropriate permissions and react accordingly
            client.UserJoined += async (user) =>
            {
                //check welcome module
                var results = Justibot.Loader.LoadPerm(user, "WELCOME");
                var guild = user.Guild as IGuild;
                //if welcome module enabled send message
                if (results.Item1 == true)
                {
                    ulong result2 = results.Item2;
                    //get designated channel for welcome messages
                    SocketTextChannel welcomeChannel = user.Guild.GetChannel(result2) as SocketTextChannel;

                    //if welcome message is available for guild use it, otherwise use default
                    bool check = welcomedict.welcomes.TryGetValue(guild.Id, out string welcome);
                    if (!check)
                    {
                        welcome = "Welcome, **[user]** has joined **[server]!!!** \n" +
                        "Have a good time!!!";
                    }
                    //fill in placeholder text with appropriate information
                    welcome = welcome.Replace("[user]", user.Username);
                    welcome = welcome.Replace("[server]", guild.Name);

                    //if permission argument is plain send message in plain text otherwise send in embeded message
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
                //if joining role is enabled give new user the role
                var joinRoles = Justibot.Loader.LoadPerm(user, "JOINROLE");
                if (joinRoles.Item1 == true)
                {
                    IRole joinerRole = user.Guild.GetRole(joinRoles.Item2) as IRole;
                    await user.AddRoleAsync(joinerRole);
                }
                //if log permission enabled send message to log channel
                var results2 = Justibot.Loader.LoadPerm(user, "LOG");
                if (results2.Item1 == true)
                {
                    ulong result2 = results2.Item2;
                    SocketTextChannel logChannel = user.Guild.GetChannel(result2) as SocketTextChannel;

                    if (results2.Item3 == "PLAIN")
                    {
                        await logChannel.SendMessageAsync($"{Format.Bold($"{user.Username}")}#{user.Discriminator}, ID:<{user.Id}> has joined the server. \n" +
                            $"There are now {user.Guild.MemberCount} members.");
                    }
                    else
                    {
                        string avatar = user.GetAvatarUrl();
                        Color color = new Color(0, 255, 0);
                        //if no avatar available use deafult
                        if (avatar == null)
                        {
                            avatar = "https://cdn.discordapp.com/embed/avatars/0.png";
                        }
                        //check if avatar is gif (gif avatars contain /a_ in their URLs
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

                        await logChannel.SendMessageAsync("", false, embed);
                    }
                }
                //if bunker mode is enabled on server instantly kick new user
                if (Settings.bunker.Contains(guild.Id))
                {
                    await user.Guild.DefaultChannel.SendMessageAsync($"{Format.Bold($"{user.Username}")} was kicked by bunker mode!");
                    await user.KickAsync("Bunker mode enabled");
                }
            };

            //when user leaves guild check appropriate permissions and act accordingly
            client.UserLeft += async (user) =>
            {
                //check leaving permission
                var results = Justibot.Loader.LoadPerm(user, "LEAVING");
                if (results.Item1 == true)
                {
                    ulong result2 = results.Item2;
                    var guild = user.Guild as IGuild;
                    //get channel to send leaving message to
                    SocketTextChannel leaveChannel = user.Guild.GetChannel(result2) as SocketTextChannel;

                    //if leaving message available set it, else use default message
                    bool check = welcomedict.leaves.TryGetValue(guild.Id, out string leave);
                    if (!check)
                    {
                        leave = "**[mention]** has left **[server]**, goodbye.";
                    }
                    //replace placeholder text with appropriate items
                    leave = leave.Replace("[user]", user.Username);
                    leave = leave.Replace("[mention]", user.Mention);
                    leave = leave.Replace("[server]", guild.Name);

                    //if leaving permission argument is plain, send plain text otherwise send as an embed message
                    if (results.Item3 == "PLAIN")
                    {
                        await leaveChannel.SendMessageAsync(leave);
                    }
                    else
                    {
                        string avatar = user.GetAvatarUrl();
                        Color color = new Color(255, 0, 0);
                        //if no avatar available use deafult
                        if (avatar == null)
                        {
                            avatar = "https://cdn.discordapp.com/embed/avatars/0.png";
                        }
                        //check if avatar is gif (gif avatars contain /a_ in their URLs
                        if (avatar.Contains("/a_"))
                        {
                            avatar = $"{avatar.Remove(avatar.Length - 12)}gif?size=128";
                        }

                        var embed = new EmbedBuilder()
                        .WithColor(color)
                        .WithTitle($"{Format.Bold($"{client.CurrentUser.Username}:")}")
                        .WithDescription(leave)
                        .WithThumbnailUrl(avatar)
                        .Build();

                        await leaveChannel.SendMessageAsync("", false, embed);
                    }
                }

                //if log module is enabled, send message to log channel bassed on argument (plain text or embed)
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
                        //if no avatar available use deafult
                        if (avatar == null)
                        {
                            avatar = "https://cdn.discordapp.com/embed/avatars/0.png";
                        }
                        //check if avatar is gif (gif avatars contain /a_ in their URLs
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

            //if user messages a channel the bot can see, check permissions
            client.MessageReceived += async (message) =>
            {
                var guild = (message.Channel as SocketTextChannel)?.Guild;
                var user = (message.Author as IGuildUser);
                if (guild != null)
                {
                    //if user isnt bot, give xp to user
                    if (user.IsBot == false)
                    {
                        var guild2 = guild as IGuild;
                        XpService.AddXp(user, guild2);
                    }
                    //if message contains a discord invite link, check if use has manage guild permissions
                    if ((message.ToString().ToUpper()).Contains("DISCORD.GG/") && (!(user.IsBot)))
                    {
                        //if user does not have manage guild permission check if advertising module is enabled
                        if (!(user.GuildPermissions.Has(GuildPermission.ManageGuild)) || !(Justibot.Loader.isStaff(user)))
                        {
                            var results = Justibot.Loader.LoadPerm(user, "ADVERTISING");
                            if (results.Item1 == true)
                            {
                                //if module is set to delete, remove message and asks user to not advertise in the server
                                ulong result2 = results.Item2;
                                if (results.Item3 == "DELETE")
                                {
                                    await message.DeleteAsync();
                                    var response = await message.Channel.SendMessageAsync($"{user.Mention} please do not advertise in this server");
                                    respond(response);
                                }
                                //if module is set to repost, send copy of message to advertising channel and ask user to only post advertisements to channel
                                else if (results.Item3 == "REPOST")
                                {
                                    SocketTextChannel advertisingChannel = guild.GetChannel(result2) as SocketTextChannel;
                                    if (message.Channel != advertisingChannel)
                                    {
                                        await advertisingChannel.SendMessageAsync($"{user.Mention} advertised message: \n{message.ToString()}");
                                        await message.DeleteAsync();
                                        var response = await message.Channel.SendMessageAsync($"{user.Mention} please keep advertising to {advertisingChannel.Mention}, your message has been shifted there.");
                                        respond(response);
                                    }
                                }
                            }
                        }
                    }
                }
                
            };
            //initialize commands and commandhandler
            handler = new CommandHandler();
            await handler.InitCommands(client);
            //prevent application shutdown
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            //if log message contains failed message restart bot
            if(msg.ToString().Contains("Failed to resume previous session"))
            {
                restartBot();
            }
            return Task.CompletedTask;
        }

        //removes message after 3 seconds
        private async void respond(RestUserMessage response)
        {
            await Task.Delay(3000);
            if (response.Channel.GetMessageAsync(response.Id) != null)
            {
                //if message is deleted prior, to 3 second time, ignore error, otherwise delete message
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
            //dispose of current client and create a new one
            client.Dispose();
            new Program().Start().GetAwaiter().GetResult();
        }

    }
}
