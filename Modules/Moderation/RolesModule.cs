using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Runtime.InteropServices;
using Justibot.Services;
using System.Collections.Generic;
using Discord.Rest;

namespace Justibot.Modules.Moderation
{
    public class RolesModule : ModuleBase
    {
        private static Dictionary<ulong, KeyValuePair<ulong, ulong>> MessageUserRoleList = new Dictionary<ulong, KeyValuePair<ulong, ulong>>();

        public async static void checkMessage(IMessage message, ulong userId, string reaction, IGuild guild)
        {
            KeyValuePair<ulong, ulong> item;
            try
            {
                if (message == null)
                {
                    return;
                }
                if (MessageUserRoleList.TryGetValue(message.Id, out item))
                {
                    if (item.Key == userId)
                    {
                        bool saving = true;
                        List<GuildEmote> emote = guild.Emotes.Where(x => x.Name == reaction).ToList();

                        if (emote.Count == 0)
                        {
                            var stringInfo = new System.Globalization.StringInfo(reaction);
                            if (stringInfo.LengthInTextElements != 1)
                            {
                                saving = false;
                            }
                        }

                        if (saving)
                        {
                            Saver.SaveReactionRole(guild.Id, reaction, item.Value);
                            MessageUserRoleList.Remove(message.Id);
                            var message2 = await message.Channel.SendMessageAsync("Role added to list");
                            await Task.Delay(3000);
                            try
                            {
                                await message.DeleteAsync();
                                await message2.DeleteAsync();
                            }
                            catch
                            { }
                        }
                        else
                        {
                            var message2 = await message.Channel.SendMessageAsync("Invalid Reaction, please use an emoji or emote from the current server");
                            await Task.Delay(3000);
                            try
                            {
                                await message2.DeleteAsync();
                            }
                            catch
                            { }
                        }
                    }
                }
            }
            catch
            { }
        }

        [Group("ReactionRole")]
        public class ReactionRoles : ModuleBase
        {
            [Command("Add")]
            [Summary("Add Reaction Role")]
            public async Task AddReactionRole(IRole role)
            {
                if ((Context.User as IGuildUser).GuildPermissions.Has(GuildPermission.ManageGuild))
                {
                    var messageId = Context.Message.Id;
                    var userRole = new KeyValuePair<ulong, ulong>(Context.User.Id, role.Id);
                    MessageUserRoleList.Add(messageId, userRole);
                    respond(await Context.Channel.SendMessageAsync("please react to the command message to assign reaction to the role"));
                }
            }

            [Command("Remove")]
            [Summary("Remove Reaction Role")]
            public async Task RemoveReactionRole(IRole role)
            {
                if ((Context.User as IGuildUser).GuildPermissions.Has(GuildPermission.ManageGuild))
                {
                    Saver.RemoveReactionRole(Context.Guild.Id, role.Id);
                    respond(await Context.Channel.SendMessageAsync("Rele removed from list, please use +DisplayRoles to update message"));
                }
            }

            public async void respond(IUserMessage response)
            {
                await Task.Delay(3000);
                if (response.Channel.GetMessageAsync(response.Id) != null)
                {
                    //if message is deleted prior, to 3 second time, ignore error, otherwise delete message
                    try
                    {
                        await response.DeleteAsync();
                    }
                    catch
                    { }
                }
            }
        }

        [Command("DisplayRoles")]
        [Summary("Displays role message")]
        public async Task DisplayRoleMessage()
        {

            if ((Context.User as IGuildUser).GuildPermissions.Has(GuildPermission.ManageGuild))
            {
                var roleList = Loader.LoadReactionRoles(Context.Guild.Id);

                if (roleList.Count() > 0)
                {

                    Color color = new Color(0, 225, 255);

                    var author = new EmbedAuthorBuilder()
                        .WithName($"{Context.Client.CurrentUser.Username}\n");

                    string message = "";

                    IRole roleObj;
                    foreach (var role in roleList)
                    {
                        roleObj = Context.Guild.GetRole(role.Key);
                        List<GuildEmote> emote = Context.Guild.Emotes.Where(x => x.Name == role.Value).ToList();

                        if (emote.Count > 0)
                        {
                            message += $"{emote.First()}:    {roleObj.Name} \n\n";
                        }
                        else
                        {
                            Emoji emoji = new Emoji(role.Value.ToString());
                            message += $"{emoji}:    {roleObj.Name} \n\n";
                        }
                    }

                    var embed = new EmbedBuilder()
                        .WithColor(color)
                        .WithAuthor(author)
                        .WithTitle("Roles")
                        .AddField(x => { x.Name = "Emote to get role:"; x.Value = message; x.WithIsInline(false); })
                        .Build();


                    var newMessage = await Context.Channel.SendMessageAsync("", false, embed);
                    foreach (var role in roleList)
                    {
                        List<GuildEmote> emote = Context.Guild.Emotes.Where(x => x.Name == role.Value).ToList();

                        if (emote.Count > 0)
                        {
                            await newMessage.AddReactionAsync(emote.First());
                        }
                        else
                        {
                            Emoji emoji = new Emoji(role.Value.ToString());
                            await newMessage.AddReactionAsync(emoji);
                        }
                    }
                    Saver.SaveRoleMessage(Context.Guild.Id, newMessage.Id);
                }

            }
        }

    }
}