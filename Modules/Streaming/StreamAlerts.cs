using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Justibot;


namespace justibot_server.Modules.Streaming
{
    [Group("StreamAlerts")]
    public class StreamAlerts : ModuleBase
    {

        [Command("Add")]
        [Summary("Add a user for alerting a guild on stream go live")]
        public async Task AddStreamAlert(IUser user, IChannel channel, [Remainder] string message)
        {

            Saver.SaveStreamAlert(user.Id, Context.Guild.Id, channel.Id, message);

            await ReplyAsync("Added!");
        }

        [Command("Remove")]
        [Summary("stops allerting stream go live for a user in a guild")]
        public async Task RemoveStreamAlert(IUser user, IChannel channel, [Remainder] string message)
        {

            bool removed = Saver.RemoveStreamAlert(user.Id, Context.Guild.Id);

            if (removed)
            {
                await ReplyAsync("Removed!");
            }
            else
            {
                await ReplyAsync("Unable to find record to remove!");
            }

        }

    }
}
