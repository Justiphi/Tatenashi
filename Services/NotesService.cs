using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Discord.Audio;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;

namespace Justibot.Services
{

    static class notedict
    {
        public static ConcurrentDictionary<KeyValuePair<ulong, ulong>, IMessage> notes = new ConcurrentDictionary<KeyValuePair<ulong, ulong>, IMessage>();
    }

    public class notesService
    {
        public static void addNote(IMessage note, IGuildUser user)
        {
            notedict.notes.AddOrUpdate(new KeyValuePair<ulong, ulong>(user.Guild.Id, user.Id), note, (k,v) => note);
        }

        public static bool removeNote(IGuildUser user)
        {
            var message3 = notedict.notes.Where(x => (x.Key.Key == user.Guild.Id)).ToList();
            var message4 = message3.Where(x => x.Key.Value == user.Id).ToList();
            if(message3.Count == 0 && message4.Count == 0)
            {
                return false;
            }
            else
            {
                var message = message3.First().Value;
                notedict.notes.TryRemove(new KeyValuePair<ulong, ulong>(user.Guild.Id, user.Id), out message);
                message.DeleteAsync();
                return true;
            }

        }


    }
}