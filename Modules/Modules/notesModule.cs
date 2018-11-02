using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;
using Justibot.Services;

namespace Justibot.Modules.Modules
{
    [Group("notes")]
    public class NotesModule : ModuleBase
    {

        [Command("new")]
        [Alias("add")]
        public async Task addnote(string setting, string name, [Remainder] string input)
        {
            var activeuser = Context.User as IGuildUser;
            var application = await Context.Client.GetApplicationInfoAsync();
            bool isOwner = false;
            if (activeuser.Id == application.Owner.Id)
            {
                isOwner = true;
            }
            if (input.Length > Settings.maxNoteLength && !(isOwner))
            {
                await ReplyAsync($"Note content too long, please keep within {Settings.maxNoteLength} characters.");
                return;
            }
            if (name.Length > Settings.maxNameLength && !(isOwner))
            {
                await ReplyAsync($"Note title content too long, please keep within {Settings.maxNameLength} characters.");
                return;
            }
            string check = "void";
            var results = Justibot.Loader.LoadPerm(activeuser, "NOTES");
            switch (setting.ToUpper())
            {
                case "PUBLIC":
                    check = Justibot.Saver.Saveusernote(activeuser, name, input, 1, isOwner);
                    await ReplyAsync(check);
                    break;
                case "PRIVATE":
                    check = Justibot.Saver.Saveusernote(activeuser, name, input, 2, isOwner);
                    await ReplyAsync(check);
                    break;
                case "SERVER":
                    if (results.Item1 == true)
                    {
                        if (results.Item3 == "OPEN" || activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || isOwner)
                        {
                            check = Justibot.Saver.Saveservernote(activeuser, name, input);
                            await ReplyAsync(check);
                        }
                    }
                    else
                    {
                        await ReplyAsync("Failed to save, server notes are disabled");
                    }
                    break;
                case "STAFF":
                    if (results.Item1 == true)
                    {
                        if (activeuser.GuildPermissions.Has(GuildPermission.ManageGuild) || isOwner)
                        {
                            check = Justibot.Saver.Savestaffnote(activeuser, name, input);
                            await ReplyAsync(check);
                        }
                        else
                        {
                            await ReplyAsync("You do not have permissions to save staff notes.");
                        }
                    }
                    else
                    {
                        await ReplyAsync("Failed to save, server notes are disabled");
                    }
                    break;
                default:
                    await ReplyAsync("Invalid setting mode, please use public, private, server or staff");
                    return;
                }
            }


        
        [Command("delete")]
        public async Task deletenote(int input)
        {
            string check = Justibot.Saver.Deletenote(Context.User as IGuildUser, input);
            if (check == "Saved")
            {
                await ReplyAsync("Note successfully deleted");
            }
            else
            {
                await ReplyAsync("Note has not been deleted, (does not exist? / do not have permission?) try again.");
            }
        }

        [Command("close")]
        public async Task closenote()
        {
            IGuildUser activeuser = Context.User as IGuildUser;
            var message = notesService.removeNote(activeuser);
            if(message == false)
            {
                await ReplyAsync("Did not find open message to close.");
            }
            else
            {
                await ReplyAsync("Note closed");
            }
        }

        [Command("admindelete")]
        public async Task admindeletenote(int input)
        {
            string check = "failed";
            if ((Context.User as IGuildUser).GuildPermissions.Has(GuildPermission.ManageGuild))
            {
                check = Justibot.Saver.AdminDeletenote(Context.User as IGuildUser, input);
            }
            if (check == "Saved")
            {
                await ReplyAsync("Note successfully deleted");
            }
            else
            {
                await ReplyAsync("Note has not been deleted, (does not exist? / do not have permission?) try again.");
            }
        }

        [Command("view")]
        public async Task viewnote([OptionalAttribute] IUser user)
        {
            List<string> privatenotes;
            List<string> publicnotes;
            List<string> servernotes;
            List<string> staffnotes;
            bool isUser = false;
            if (user == null)
            {
                user = Context.User;
            }
            if (user.Id == Context.User.Id)
            {
                isUser = true;
            }

            publicnotes = Justibot.Loader.loadpublicnote(user as IGuildUser);
            privatenotes = Justibot.Loader.loadprivatenote(Context.User as IGuildUser);
            servernotes = Justibot.Loader.loadservernote(Context.User as IGuildUser);
            staffnotes = Justibot.Loader.loadstaffnote(Context.User as IGuildUser);


            string message = String.Join("\n", publicnotes);
            string message1 = String.Join("\n", privatenotes);
            string message2 = String.Join("\n", servernotes);
            string message3 = String.Join("\n", staffnotes);

            IUserMessage messag = null;

            Color color = new Color(122, 255, 202);
            if (isUser && (Context.User as IGuildUser).GuildPermissions.Has(GuildPermission.ManageGuild))
            {
                var embed = new EmbedBuilder()
                    .WithTitle($"{user.Username} notes:")
                    .WithColor(color)
                    .AddField(x => { x.Name = "Public Notes:"; x.Value = message; })
                    .AddField(x => { x.Name = "Private Notes:"; x.Value = message1; })
                    .AddField(x => { x.Name = "Server Notes:"; x.Value = message2; })
                    .AddField(x => { x.Name = "Staff Notes:"; x.Value = message3; })
                    .Build();
                
                messag = await Context.Channel.SendMessageAsync("", false, embed);
            }
            else if (isUser)
            {
                var embed = new EmbedBuilder()
                    .WithTitle($"{user.Username} notes:")
                    .WithColor(color)
                    .AddField(x => { x.Name = "Public Notes:"; x.Value = message; })
                    .AddField(x => { x.Name = "Private Notes:"; x.Value = message1; })
                    .AddField(x => { x.Name = "Server Notes:"; x.Value = message2; })
                    .Build();

                messag = await Context.Channel.SendMessageAsync("", false, embed);
            }
            else
            {
                var embed = new EmbedBuilder()
                    .WithTitle($"{user.Username} notes:")
                    .WithColor(color)
                    .AddField(x => { x.Name = "Public Notes:"; x.Value = message; })
                    .Build();

                messag = await Context.Channel.SendMessageAsync("", false, embed);
            }
            Justibot.Services.notesService.addNote((messag as IMessage), (Context.User as IGuildUser));

        }

    }
}