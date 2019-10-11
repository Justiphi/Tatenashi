using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Justibot.Services;

namespace Justibot.Modules.Help
{
    [Group("Hackfest")]
    public class HacktoberModule : ModuleBase
    {
        [Command("PalindromeCheck")]
        [Summary("Checks to see if provided word is or is not a palindrome.")]
        public async Task CheckIfPalindrome(string wordToCheck)
        {
            char[] sArray = wordToCheck.ToCharArray();
            Array.Reverse(sArray);
            // If the reverse string is equal to the passed in paramenter and there is no space in the provided word.
            var wordIsPalindrome = wordToCheck == new string(sArray) && !sArray.Any(x => char.IsWhiteSpace(x));

            await ReplyAsync(wordToCheck + " is " + (wordIsPalindrome ? "" : "not ") + "a palindrom.");
        }

        [Command("YoutubeSearch")]
        [Summary("Return a link for a search on youtube for the provided value.")]
        public async Task YoutubeSearch(string searchTerm)
        {
            var returnURL = "https://www.youtube.com/results?search_query=" + HttpUtility.UrlEncode(searchTerm);

            await ReplyAsync(returnURL);
        }
        // [Command("CommandName")] //single word
        // [Summary("Description of command")] //can be multiple words
        // public async Task MethodName() //method name should be related to command name
        // {
        //     string response;

        //     //code to create response

        //     await ReplyAsync(response);
        // }

    }
}