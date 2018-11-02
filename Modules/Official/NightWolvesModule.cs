using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Justibot;

namespace Justibot.Modules.NightWolves
{
    public class NightWolvesModule : ModuleBase
    {

        [Command("alias")]
        public async Task aliai([OptionalAttribute]IUser user)
        {
            if (user == null)
            {
                user = Context.User;
            }
            IGuildUser activeuser = Context.User as IGuildUser;
            IGuildUser user2 = user as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "WOLVES");
            var application = await Context.Client.GetApplicationInfoAsync();
            if (epsilonmode.Item1 == true)
            {
                if (user == Context.User || activeuser.GuildPermissions.Has(GuildPermission.ManageNicknames) || Context.User.Id == application.Owner.Id)
                {
                    int treeNumber = Data.rand.Next(Data.aliai.Length);
                    string postedTree = Data.aliai[treeNumber];
                    await user2.ModifyAsync(x => x.Nickname = postedTree);
                }
                else
                {
                    await ReplyAsync("You do not have permission to use this command.");
                }
            }
        }

        [Command("dog")]
        public async Task dog()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "WOLVES");
            if (epsilonmode.Item1 == true)
            {
                int treeNumber = Data.rand.Next(Data.dogList.Length);
                string postedTree = Data.dogList[treeNumber];
                await Context.Channel.SendFileAsync(postedTree);
            }
        }

    }
}