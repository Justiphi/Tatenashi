using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Justibot.Modules.Meme
{
    public class MemeModule : ModuleBase
    {

        [Command("ping")]
        [Summary("Returns Pong!")]
        public async Task ping()
        {
            await ReplyAsync($"Pong! `Latency: {(Context.Client as DiscordSocketClient).Latency}ms`");
        }

        //Command to start playing youtube video for rickrolling
        [Command("roll")]
        [Summary("Rolls meme! Requested by iMiNvoke")]
        public async Task roll()
        {
            await ReplyAsync("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }

        //Command to initate the coin toss command
        [Command("coin")]
        [Summary("Coin toss")]
        public async Task coin()
        {   //generate a random number between 0 and 1 and assigns heads and tails to each
            int rolledNumber = Data.rand.Next(2);
            if (rolledNumber == 0)
            {
                await ReplyAsync(Context.User.Mention + " Heads");
            }
            else if (rolledNumber == 1)
            {
                await ReplyAsync(Context.User.Mention + " Tails");
            }
            else
            {
                await ReplyAsync(Context.User.Mention + " Sorry an error has occured, please try again");
                Console.WriteLine("[ERROR] [" + Context.Guild.Name.ToString() + "] Invalid coin toss, threw " + rolledNumber.ToString());
            }
        }

        //Command to initate the dice roll command
        [Command("dice")]
        [Summary("rolls a 6 sided die")]
        public async Task dice(int sides = 6)
        {
            if (sides >= 2 && sides <= 255)
            {
                int rolledNumber = Data.rand.Next(sides);
                await ReplyAsync(Context.User.Mention + " You rolled " + (rolledNumber + 1).ToString());
            }
            else
            {
                await ReplyAsync($"you entered {sides}, please try again Min: 2 max: 255!");
            }
        }

        //displays the shifty eye meme image
        [Command("shifty")]
        [Summary("Shifty Eyes XD")]
        public async Task shifty()
        {
            await Context.Channel.SendFileAsync("Resources/Images/shifty.gif");
        }

        //plays youTube video with one random video from the Rave videos from Data\Data.cs@Raves
        [Command("Rave")]
        [Summary("Rave party, suggested by shley92822212")]
        public async Task Rave()
        {
            int RaveNG = Data.rand.Next(Data.rave.Length);
            string ravetune = Data.rave[RaveNG];
            await ReplyAsync(ravetune);
        }

        //redirects to leekspin
        [Command("Leeks")]
        [Summary("leek time, suggested by shley92822212")]
        public async Task Leeks()
        {
            await ReplyAsync("http://leekspin.com");
        }

    }
}