using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Justibot.Services;
using System.Web;
using System.Globalization;

namespace Justibot.Modules.Help
{
    [Group("Hackfest")]
    public class HacktoberModule : ModuleBase
    {
    
        // COMMAND TEMPLATE
        // [Command("CommandName")] //single word
        // [Summary("Description of command")] //can be multiple words
        // public async Task MethodName() //method name should be related to command name
        // {
        //     string response;
        //     await ReplyAsync(response);
        // }

        // Allows for rolling multiple dice of a single type at once.
        [Command("RollDice")]
        [Summary("Roll a number of the same type of die and return the sum in the form ##d## (e.g., 3d8 will return 3-24)")]
        public async Task RollDice(string diceString)
        {
            //  At maximum, this will allow for rolling up to 1000d1000, and will cap either value
            //  to the maximum specified below.
            const uint maxCount = 1000;
            const uint maxSides = 1000;
            
            //  Our general error message
            const string baseError = "Unable to parse '{0}'.  You can try something like '3d8' (positive numbers on each side) to get a value of 3 to 24.";

            //  Forcing to lower case 'd' and removing whitespace.
            var newDice = diceString.ToLower().Trim();
            var rollPartitions = newDice.Split('d', StringSplitOptions.RemoveEmptyEntries);

            //  parse to make sure there's only 2 elements separated by 'd'.
            if(rollPartitions.Length != 2)
            {
                string error = string.Format(baseError, diceString);
                await ReplyAsync(error);
                return;
            }

            //  parse out the numbers, if possible.
            uint count, sides;
            if(!uint.TryParse(rollPartitions[0], out count) || !uint.TryParse(rollPartitions[1], out sides))
            {
                string error = string.Format(baseError, diceString);
                await ReplyAsync(error);
                return;
            }

            //  clamp the numbers down to maximum values.
            count = (count > maxCount) ? maxCount : count;
            sides = (sides > maxSides) ? maxSides : sides;

            //  roll the dice and sum them.
            var rng = new Random();
            int total = 0;
            for(int i = 0; i < count; i++)
            {
                total += (rng.Next((int)sides))+1;
            }

            //  provide the results.
            await ReplyAsync($"{newDice} => {total}");
        }
    
        // Displays the number of days in a month for the current year
        [Command("MonthLength")]
        [Summary("Command to display how many days a month has this year. Pass the month number (e.g. 1 for January, 2 for February and so on) as an argument.")]
        public async Task MonthLength(uint month)
        {
            int feb;
            int year = DateTime.Now.Year;
            if (year % 4 == 0 && ((year % 100 != 0) || (year % 400 == 0)))
                feb = 29;
            else
                feb = 28;

            Tuple<string, int>[] months = {
                Tuple.Create("January", 31),
                Tuple.Create("February", feb),
                Tuple.Create("March", 31),
                Tuple.Create("April", 30),
                Tuple.Create("May", 31),
                Tuple.Create("June", 30),
                Tuple.Create("July", 31),
                Tuple.Create("August", 31),
                Tuple.Create("September", 30),
                Tuple.Create("October", 31),
                Tuple.Create("November", 30),
                Tuple.Create("December", 31)
            };


            string response = months[month-1].Item1 + " has " + months[month-1].Item2 + " days.";
            
            await ReplyAsync(response);
        }

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

        [Command("urban")]
        [Summary("Look up a word on urbandictionary.com")]
        public async Task Urban(string keyword)
        {
            var url = "https://www.urbandictionary.com/define.php?term="+HttpUtility.UrlEncode(keyword);
 
            await ReplyAsync(url);
        }

        [Command("FantasyName")]
        [Summary("Generates a simple fantasy name.")]
        public async Task FantasyName()
        {
            var random = new Random();
            var nameSegments = new string[] { "mon", "fay", "shi", "zag", "blag", "rash", "izen", "fan", "sha", "bol", "bly", "kle", "try" };
            var randomCount = random.Next(4, 6);
            var finalName = "";

            for (var i = 0; i < randomCount; i++)
            {
                finalName += nameSegments[random.Next(nameSegments.Length - 1)];
                if (i == 1)
                    finalName += " ";
            }

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            await ReplyAsync(textInfo.ToTitleCase(finalName));
        }


        [Command("DNDWeapon")]
        [Summary("Generates a random DNDWeapon")]
        public async Task DNDWeapon()
        {
            var rn = new Random();
            var DmgNum = rn.Next(1, 3);
            var DmgSize = rn.Next(1, 6);

            var WeaponList = new List<string> { "Sword", "Bow", "Axe", "Pike", "Hammer", "Staff" };
            var WeaponName = WeaponList[rn.Next(0, 5)].ToString();
            var Weapon = $"{WeaponName} {DmgNum}d{DmgSize}";

            await ReplyAsync(Weapon);
        }

        [Command("LMGTFY")]
        [Summary("Sarcastic/funny way to search google for a question someone might ask.")]
        public async Task LetMeGoogleThatForYou(string searchTerm)
        {
            var url = "https://lmgtfy.com/?q=" + HttpUtility.UrlEncode(searchTerm);

            await ReplyAsync(url);

        }
    }
}