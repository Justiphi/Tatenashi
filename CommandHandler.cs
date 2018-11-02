using System.Threading.Tasks;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Justibot.Services;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Justibot
{
    public class CommandHandler
    {
        private CommandService commands = new CommandService();
        private readonly IServiceCollection _map = new ServiceCollection();
        private DiscordSocketClient client;
        private PrefixService prefixService = new PrefixService();

        public IServiceProvider _services;

        public async Task InitCommands(DiscordSocketClient _client)
        {
            client = _client;
            // Repeat this for all the service classes
            // and other dependencies that your commands might need.
            _map.AddSingleton(prefixService);
            _map.AddSingleton(new WelcomeService());
            _map.AddSingleton(new LeavingService());
            _map.AddSingleton(new XpService());

            // Either search the program and add all Module classes that can be found:
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
            // Or add Modules manually if you prefer to be a little more explicit:
            //await _commands.AddModuleAsync<SomeModule>();

            // When all your required services are in the collection, build the container.
            // Tip: There's an overload taking in a 'validateScopes' bool to make sure
            // you haven't made any mistakes in your dependency graph.
            _services = _map.BuildServiceProvider();

            // Subscribe a handler to see if a message invokes a command.
            client.MessageReceived += HandleCommand;
        }

        public async Task HandleCommand(SocketMessage parameterMessage)
        {
            var user = parameterMessage.Author as IGuildUser;
            var message = parameterMessage as SocketUserMessage;
            var guild = (parameterMessage.Channel as SocketTextChannel)?.Guild;

            if (message == null) return;

            char prefix;
            int argPos = 0;
            if (guild != null)
            {
                Services.statsService.messageRecieved(guild.Id);
                prefix = prefixService.getPrefix2(guild.Id);

                if (!(message.HasMentionPrefix(client.CurrentUser, ref argPos) || (message.HasCharPrefix(prefix, ref argPos)) || (message.HasCharPrefix('+', ref argPos) && message.ToString().ToUpper() == "+HELP"))) return;

                var context = new CommandContext(client, message);

                var result = await commands.ExecuteAsync(context, argPos, _services);
                Services.statsService.commandRecieved(guild.Id);
            }

            if ((message.Channel as IDMChannel) != null)
            {
                if ((message.HasCharPrefix('+', ref argPos) && message.ToString().ToUpper().StartsWith("+HELP")) || (message.HasCharPrefix('+', ref argPos) && message.ToString().ToUpper() == "+INVITE"))
                {
                    var context = new CommandContext(client, message);
                    var result = await commands.ExecuteAsync(context, argPos, _services);
                    Services.statsService.commandRecieved(guild.Id);
                }

                Services.statsdict.messagesSeen = Services.statsdict.messagesSeen+1;

                var home = client.GetGuild(288252004055384065);
                if (home != null)
                {
                    var chan = home.GetTextChannel(351208692525432832);
                    var user2 = parameterMessage.Author as IUser;
                    var message2 = parameterMessage as SocketUserMessage;

                    var embed = new EmbedBuilder()
                        .WithColor(new Color(0, 255, 0))
                        .WithAuthor((new EmbedAuthorBuilder()
                            .WithName(user2.Username)
                            .WithIconUrl(user2.GetAvatarUrl())))
                        .AddField(x => { x.Name = $"<{user2.Id.ToString()}> | DM"; x.Value = message2; })
                        .WithCurrentTimestamp()
                        .Build();

                    await chan.SendMessageAsync("", false, embed);
                }
            }

            //if (!result.IsSuccess)
            //    await message.Channel.SendMessageAsync($"**Error:** {result.ErrorReason}");
        }

    }

}
