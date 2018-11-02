using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Justibot.Services;

namespace Justibot.Modules.Help
{
    [Group("Help")]
    public class HelpModule : ModuleBase
    {

        [Command]
        [Summary("Displays Commands")]
        public async Task Help()
        {
            char Prefix = '+';
            if(Context.Guild != null)
            {
                Prefix = PrefixService.getPrefix(Context.Guild.Id);
            }

            string message = (
                $"{Format.Bold($"{Format.Underline("Prefix:")} {Prefix}")}"
            );
            string message1 = (
                $"{Prefix} {Format.Bold("Ping:")} Returns Pong! \n" +
                $"{Prefix} {Format.Bold("Echo:")} Repeats what you say. \n" +
                $"{Prefix} {Format.Bold("Dice:")} Rolls a dice with a set amount of sides. Minimum 2, maximum 255, leave blank for 6. \n" +
                $"{Prefix} {Format.Bold("Coin:")} Coin toss."
            );
            string message2 = (
                $"{Prefix} {Format.Bold("Kick/Ban:")} Kicks/Bans a user from the server. \n" +
                $"{Prefix} {Format.Bold("Purge x:")} Deletes x amount of messages from a channel. (25 by default, 100 max) \n" +
                $"{Prefix} {Format.Bold("Mute/Unmute:")} Mutes/Unmutes a user in the server."
            );
            string message3 = (
                $"{Prefix} {Format.Bold("Info:")} Displays bots info. \n" +
                $"{Prefix} {Format.Bold("Userinfo:")} Returns information on mentioned user, leave blank for yourself. (username ID and time created) \n" +
                $"{Prefix} {Format.Bold("Serverinfo:")} Returns information on the server."
            );
            string message4 = (
                $"{Prefix} {Format.Bold("Documentation:")} [http://tatenashi.com](http://tatenashi.com) \n" +
                $"{Prefix} {Format.Bold("Official Server:")} [https://discord.gg/gS6zZ3B](https://discord.gg/gS6zZ3B) \n" +
                $"{Prefix} {Format.Bold("Contact:")} [http://tatenashi.com/home/contact](http://tatenashi.com/home/contact)" 
            );
            string message5 = (
                $"{Prefix} {Format.Bold("Help notes:")} Help for notes module. \n" +
                $"{Prefix} {Format.Bold("Help Ranks:")} Help for xp module "
            );

            Color color = new Color(0, 225, 100);

            var embed = new EmbedBuilder()
                .WithTitle("Commands:")
                .WithColor(color)
                .AddField(x => { x.Name = "Current Prefix:"; x.Value = message; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Basic:"; x.Value = message1; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Moderator:"; x.Value = message2; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Info:"; x.Value = message3; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Other help commands:"; x.Value = message5; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Links:"; x.Value = message4; x.WithIsInline(true); })
                .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("epsilon")]
        [Summary("Displays Commands")]
        public async Task epsilonHelp()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "EPSILON");
            if (epsilonmode.Item1 == true)
            {
                var Prefix = PrefixService.getPrefix(Context.Guild.Id);

                string message = (
                    $"{Prefix} {Format.Bold("whatASave:")} Returns random message (command won by Storm!) \n" +
                    $"{Prefix} {Format.Bold("Tree:")} Shows a picture of a tree (Storm/Revan Joke) Requested by Storm \n" +
                    $"{Prefix} {Format.Bold("penis:")} Try it and find out ;) \n" +
                    $"{Prefix} {Format.Bold("jesus:")} Summons a representation of Jesus Christ under the request of Rohan \n" +
                    $"{Prefix} {Format.Bold("Wisdom:")} Enlightens channel with the daily wisdom of Mike Nolan \n" +
                    $"{Prefix} {Format.Bold("Toetem:")} shows the first Epsilon God to reveal itself"
                );
                string message2 = (
                    $"{Format.Bold($"{Format.Underline("Prefix:")} {Prefix}")}"
                );

                Color color = new Color(0, 225, 100);

                var embed = new EmbedBuilder()
                    .WithTitle("Commands:")
                    .WithColor(color)
                    .AddField(x => { x.Name = "Current Prefix:"; x.Value = message2; x.WithIsInline(true); })
                    .AddField(x => { x.Name = "Basic:"; x.Value = message; x.WithIsInline(true); })
                    .Build();

                await Context.Channel.SendMessageAsync("", false, embed);
            }
        }

        [Command("Notes")]
        [Summary("Displays Commands")]
        public async Task notesHelp()
        {
            char Prefix = '+';
            if(Context.Guild != null)
            {
                Prefix = PrefixService.getPrefix(Context.Guild.Id);
            }

            string message = (
                $"{Prefix} {Format.Bold("`notes new <type> <noteName> <note content>`:")} Creates a new note of type <type> \n" +
                $"{Prefix} {Format.Bold("`notes view (user)`:")} Shows a users public notes, if left blank shows all notes associated with user. \n" +
                $"{Prefix} {Format.Bold("`notes close`:")} deletes last note the user has viewed ;) \n" +
                $"{Prefix} {Format.Bold("`notes delete <noteId>`:")} deletes the associated note if the user owns it. \n" +
                $"{Prefix} {Format.Bold("`notes admindelete <noteId>`:")} deletes a server not if user has manage server permission."
            );
            string message2 = (
                $"{Format.Bold($"{Format.Underline("Prefix:")} {Prefix}")}"
            );
            string message3 = (
                "A user can have 10 public and private notes \n" +
                "A server can have 30 server notes and 20 staff notes \n" +
                "A note title must be one word with a 25 character limit any words after this will be added to the note \n" +
                "A notes content has a 150 character limit and can contain multiple words."
            );
            string message4 = (
                $"{Prefix} {Format.Bold("Public:")} Any user can view this note, only the owner may delete it. \n" +
                $"{Prefix} {Format.Bold("Private:")} Only the owner can view this note, only the owner may delete it. \n" +
                $"{Prefix} {Format.Bold("Server:")} Any user on the server can view this note, only the owner or a staff member may delete it. \n" +
                $"{Prefix} {Format.Bold("Staff:")} Any user on the server can view this note, only the owner or a staff member may delete it. \n\n" +
                $"{Prefix} {Format.Bold("Note:")} If the notes module is set to closed only staff can create server notes, if it is open anyone may create these notes."
            );

            Color color = new Color(0, 225, 100);

            var embed = new EmbedBuilder()
                .WithTitle("Commands:")
                .WithColor(color)
                .AddField(x => { x.Name = "Current Prefix:"; x.Value = message2; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Commands:"; x.Value = message; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Note Types:"; x.Value = message4; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Limits:"; x.Value = message3; x.WithIsInline(true); })
                .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("ranks")]
        [Summary("Displays Commands")]
        public async Task levelHelp()
        {

            char Prefix = '+';
            if(Context.Guild != null)
            {
                Prefix = PrefixService.getPrefix(Context.Guild.Id);
            }

            string message = (
                $"{Prefix} {Format.Bold("Rank:")} Shows a users rank on the server (tag someone to see their rank) \n" +
                $"{Prefix} {Format.Bold("Rank Global:")} Shows a users global rank (tag someone to see their rank) \n" +
                $"{Prefix} {Format.Bold("Rank Server:")} See the servers ranking ;) \n" +
                $"{Prefix} {Format.Bold("Leaderboard:")} See the servers top 10 users. \n" +
                $"{Prefix} {Format.Bold("Leaderboard Global:")} See the top 10 users. \n" +
                $"{Prefix} {Format.Bold("Leaderboard Server:")} See the top 10 servers. \n\n" +
                $"{Prefix} {Format.Bold("Note:")} Server and user xp will not work unless the levels module is enabled."
            );
            string message2 = (
                $"{Format.Bold($"{Format.Underline("Prefix:")} {Prefix}")}"
            );

            Color color = new Color(0, 225, 100);

            var embed = new EmbedBuilder()
                .WithTitle("Commands:")
                .WithColor(color)
                .AddField(x => { x.Name = "Current Prefix:"; x.Value = message2; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Commands:"; x.Value = message; x.WithIsInline(true); })
                .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("Giveaway")]
        [Summary("Displays Commands")]
        public async Task giveHelp()
        {
            char Prefix = '+';
            if(Context.Guild != null)
            {
                Prefix = PrefixService.getPrefix(Context.Guild.Id);
            }

            string message = (
                $"{Prefix} {Format.Bold("`giveaway new <prize>`:")} Creates a new giveaway for the <prize> (user becomes host) \n" +
                $"{Prefix} {Format.Bold("`giveaway finish`:")} Ends the Giveaway (does not draw, must be used by host). \n" +
                $"{Prefix} {Format.Bold("`giveaway enter`:")} Enters user into the giveaway. \n" +
                $"{Prefix} {Format.Bold("`giveaway draw`:")} Draws the giveaway. \n" +
                $"{Prefix} {Format.Bold("`giveaway drawall <mode> (optional prize)`:")} randomly selects member who matches mode (online/offline/active/offline). \n" +
                $"{Prefix} {Format.Bold("`giveaway clear`:")} Clears all entries from the giveaway apart from the host. \n" +
                $"{Prefix} {Format.Bold("`giveaway details`:")} Shows the host, number of entries and prize."
            );
            string message2 = (
                $"{Format.Bold($"{Format.Underline("Prefix:")} {Prefix}")}"
            );

            Color color = new Color(0, 225, 100);

            var embed = new EmbedBuilder()
                .WithTitle("Commands:")
                .WithColor(color)
                .AddField(x => { x.Name = "Current Prefix:"; x.Value = message2; x.WithIsInline(true); })
                .AddField(x => { x.Name = "Commands:"; x.Value = message; x.WithIsInline(true); })
                .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }


    }
}