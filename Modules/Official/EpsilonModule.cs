using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Justibot;

namespace Justibot.Modules.Epsilon
{
    public class EpsilonModule : ModuleBase
    {

        [Command("Wisdom")]
        [Summary("Enlightens channel with the daily wisdom of Mike Nolan")]
        public async Task Wisdom()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "EPSILON");
            if (epsilonmode.Item1 == true)
            {
                int wisdomN = Data.rand.Next(Data.Wisdom.Length);
                string postedWisdom = Data.Wisdom[wisdomN];
                await ReplyAsync(postedWisdom);
            }
        }

        [Command("jesus")]
        [Summary("Summons a representation of Jesus Christ under the request of Rohan")]
        public async Task Jesus()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "EPSILON");
            if (epsilonmode.Item1 == true)
            {
                await Context.Channel.SendFileAsync("Resources/Images/Jesus.jpg");
            }
        }

        [Command("Tree")]
        [Summary("Shows a picture of a tree (Storm/Revan Joke) Requested by Storm")]
        public async Task Tree()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "EPSILON");
            if (epsilonmode.Item1 == true)
            {
                int treeNumber = Data.rand.Next(Data.treeList.Length);
                string postedTree = Data.treeList[treeNumber];
                await Context.Channel.SendFileAsync(postedTree);
            }
        }

        [Command("coffeeporn")]
        [Alias("revan")]
        [Summary("Shows a picture of a tree (Storm/Revan Joke) Requested by Storm")]
        public async Task coffee()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "EPSILON");
            if (epsilonmode.Item1 == true)
            {
                int treeNumber = Data.rand.Next(Data.coffeeList.Length);
                string postedTree = Data.coffeeList[treeNumber];
                await Context.Channel.SendFileAsync(postedTree);
            }
        }

        [Command("whatASave")]
        [Summary("What A Save! command won by Storm")]
        public async Task rl()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "EPSILON");
            if (epsilonmode.Item1 == true)
            {
                int treeNumber = Data.rand.Next(Data.saveList.Length);
                string postedTree = Data.saveList[treeNumber];
                await Context.Channel.SendMessageAsync(postedTree);
            }
        }

        [Command("penis")]
        public async Task ponyShark()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "EPSILON");
            if (epsilonmode.Item1 == true)
            {
                await Context.Channel.SendFileAsync("Resources/Images/ponyShark.png");
            }
        }

        [Command("Lemur")]
        [Alias("finnix")]
        [Summary("Shows a picture of a lemur Requested by Finnix")]
        public async Task Lemur()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "EPSILON");
            if (epsilonmode.Item1 == true)
            {
                int treeNumber = Data.rand.Next(Data.lemurList.Length);
                string postedTree = Data.lemurList[treeNumber];
                await Context.Channel.SendFileAsync(postedTree);
            }
        }

        [Command("Toetem")]
        public async Task God1()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "EPSILON");
            if (epsilonmode.Item1 == true)
            {
                int treeNumber = Data.rand.Next(Data.Toetems.Length);
                string postedTree = Data.Toetems[treeNumber];
                await Context.Channel.SendFileAsync(postedTree);
            }
        }

        [Command("Yato")]
        [Alias("Lachy")]
        public async Task Lachyboy()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "EPSILON");
            if (epsilonmode.Item1 == true)
            {
                await Context.Channel.SendFileAsync("Resources/Images/yato.png");
            }
        }

    }
}