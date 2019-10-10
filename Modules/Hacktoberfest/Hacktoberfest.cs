using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Justibot.Services;
using System.Threading.Tasks;
using System.Web;

namespace Justibot.Modules.Help
{
    [Group("Hackfest")]
    public class HacktoberModule : ModuleBase
    {

        // [Command("CommandName")] //single word
        // [Summary("Description of command")] //can be multiple words
        // public async Task MethodName() //method name should be related to command name
        // {
        //     string response;

        //     //code to create response

        //     await ReplyAsync(response);
        // }

        [Command("urban")]
        [Summary("Look up a word on urbandictionary.com")]
        public async Task Urban(string keyword)
        {
            var url = "https://www.urbandictionary.com/define.php?term="+HttpUtility.UrlEncode(keyword);
 
            await ReplyAsync(url);
        }

    }
}