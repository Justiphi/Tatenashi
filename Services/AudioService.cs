using Discord;
using Discord.Audio;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace justibot_server.Services
{
    public class AudioService : IDisposable
    {
        private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();

        public void Dispose()
        {
            Task.WaitAll(ConnectedChannels.Values.Select(x => x.StopAsync()).ToArray());
            ConnectedChannels.Clear();
        }

        public async Task JoinAudioAsync(IVoiceChannel target)
        {
            if (ConnectedChannels.ContainsKey(target.GuildId))
            {
                return;
            }

            var audioClient = await target.ConnectAsync();

            ConnectedChannels.TryAdd(target.GuildId, audioClient);
        }

        public async Task LeaveAudio(IGuild guild)
        {
            if (ConnectedChannels.TryRemove(guild.Id, out var audioClient))
            {
                await audioClient.StopAsync();
            }
        }

        public async Task SendAudioFromYoutubeLinkAsync(IGuild guild, string ytLnk)
        {
            if (!ConnectedChannels.TryGetValue(guild.Id, out var audioClient))
            {
                return;
            }

            using (var yt = CreateProcess("youtube-dl.exe", $"-q -f m4a/bestaudio -o - \"{ytLnk}\""))
            using (var ffmpeg = CreateProcess("ffmpeg.exe", $"-loglevel -8 -i - -hide_banner -ac 2 -f s16le -ar 48000 pipe:1"))
            {
                var audioOutput = audioClient.CreatePCMStream(AudioApplication.Music);
                var download = yt.StandardOutput.BaseStream.CopyToAsync(ffmpeg.StandardInput.BaseStream);
                var recode = ffmpeg.StandardOutput.BaseStream.CopyToAsync(audioOutput);

                await download;
                await audioOutput.FlushAsync();
                await recode;
            }
        }

        private Process CreateProcess(string fileName, string arguments)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true
            });
        }
    }
}
