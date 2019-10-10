using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Justibot.Services;

namespace Justibot.Modules.Help
{
    [Group("Hackfest")]
    public class HacktoberModule : ModuleBase
    {

        [Command("RollSixDice")] //single word
        [Summary("Roll a six dice")] //can be multiple words
        public async Task RollSixDice() //method name should be related to command name
        {
            string response;

            //code to create response
            Random random = new Random();
            response = $"The result was {random.Next(1, 7).ToString()}";

            await ReplyAsync(response);
        }

    }
}