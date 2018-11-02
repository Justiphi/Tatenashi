using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Justibot.Services;

namespace Justibot.Modules.AudioModule
{
    public class AudioModule : ModuleBase
    {
        [Command("Music")]
        public async Task JoinChannel([RemainderAttribute]string ignore)
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var gives = Justibot.Loader.LoadPerm(activeuser, "MUSIC");
            ITextChannel channel2 = await Context.Guild.GetTextChannelAsync(gives.Item2) as ITextChannel;
            if (Context.Channel == channel2)
            {
                if (gives.Item1 == true)
                {
                    await ReplyAsync("Music has been admin disabled due to bugs");
                }
            }
        }

    }
}
