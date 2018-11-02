using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Justibot.Modules.BCS
{
    [Group("BCS")]
    public class BCSModule : ModuleBase
    {

        [Command()]
        [Alias("Tags")]
        public async Task tags()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "BCS");
            if (epsilonmode.Item1 == true)
            {
                await ReplyAsync("learnC#, edx-C#, C#server, programming, async, doesntwork, longcode, web, msdn, bots, slack, reference, hackr, linux, codingden");
            }
        }
    
		[Command("learnC#")]
        public async Task learnCSharp()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "BCS");
            if (epsilonmode.Item1 == true)
            {
                await ReplyAsync("Refer to link: https://channel9.msdn.com/Series/C-Fundamentals-for-Absolute-Beginners for help!");
            }
        }

        [Command("edx-C#")]
        public async Task edxCSharp()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "BCS");
            if (epsilonmode.Item1 == true)
            {
                await ReplyAsync("Refer to link: https://www.edx.org/course/programming-c-microsoft-dev204x-3 for help!");
            }
        }

        [Command("C#server")]
        public async Task CSharpserver()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "BCS");
            if (epsilonmode.Item1 == true)
            {
                await ReplyAsync("To get additional help with C#, join: https://discord.gg/rcPrdMb");
            }
        }

        [Command("Programming")]
        public async Task programmingserver()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "BCS");
            if (epsilonmode.Item1 == true)
            {
                await ReplyAsync("To get additional help with programming, join: https://discord.gg/010z0Kw1A9ql5c1Qe");
            }
        }

        [Command("Async")]
        public async Task programmingasync()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "BCS");
            if (epsilonmode.Item1 == true)
            {
                await ReplyAsync("To get additional help with async programming practices, join: https://msdn.microsoft.com/en-us/magazine/jj991977.aspx");
            }
        }

        [Command("doesntwork")]
        public async Task doesntwork()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "BCS");
            if (epsilonmode.Item1 == true)
            {
                await ReplyAsync("doesntwork: If your code \"doesn't run\", that can mean one of literally a million things. In order for us to help you, you need to provide an error report. If you don't have an error, learn how to debug your code. Breakpoints and all of Visual Studio's debugging tools are available to you, you should learn how to use them. We can't help you with only you saying \"it doesn't work\".");
            }
        }


        [Command("longcode")]
        public async Task longcode()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "BCS");
            if (epsilonmode.Item1 == true)
            {
                await ReplyAsync("longcode: Don't spam the chat with code. Instead, use a service like http://hastebin.com/ to upload your code if it's longer than 5 lines.");
            }
        }

        [Command("web")]
        public async Task html()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "BCS");
            if (epsilonmode.Item1 == true)
            {
                await ReplyAsync("For help with web development, see: https://www.w3schools.com/.");
            }
        }

        [Command("msdn")]
        public async Task msdn()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "BCS");
            if (epsilonmode.Item1 == true)
            {
                await ReplyAsync("Microsoft Developer Network: https://msdn.microsoft.com/en-us/default.aspx");
            }
        }

        [Command("bots")]
        public async Task bots()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "BCS");
            if (epsilonmode.Item1 == true)
            {
                await ReplyAsync("For help developing discord bots, join: https://discord.gg/discord-api");
            }
        }

        [Command("slack")]
        public async Task slack()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "BCS");
            if (epsilonmode.Item1 == true)
            {
                await ReplyAsync("The BCS project nights slack channel: https://to-bcs.slack.com/messages/C575FKGEP/");
            }
        }

        [Command("reference")]
        public async Task reference()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "BCS");
            if (epsilonmode.Item1 == true)
            {
                await ReplyAsync("A programming reference can be found at: https://www.tutorialspoint.com/computer_programming_tutorials.htm");
            }
        }

        [Command("hackr")]
        public async Task hackr()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "BCS");
            if (epsilonmode.Item1 == true)
            {
                await ReplyAsync("more tutorials can be found by visiting: https://hackr.io/");
            }
        }
        
        [Command("linux")]
        public async Task linux()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "BCS");
            if (epsilonmode.Item1 == true)
            {
                await ReplyAsync("For linux help, join: https://discord.gg/discord-linux");
            }
        }

        [Command("codingden")]
        public async Task codingden()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var epsilonmode = Justibot.Loader.LoadPerm(activeuser, "BCS");
            if (epsilonmode.Item1 == true)
            {
                await ReplyAsync("A discord programming server can be found by joining: https://discord.gg/rXMFcwk");
            }
        }
        
    }
}