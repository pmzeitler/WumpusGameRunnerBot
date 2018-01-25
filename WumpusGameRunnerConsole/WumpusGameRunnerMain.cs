using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DSharpPlus;
using DSharpPlus.EventArgs;

//client id: 406197905146511360
//token for test usage: NDA2MTk3OTA1MTQ2NTExMzYw.DUvdFA.7-ixnIzKK3WT-it4-uEDUQ4fVd0
// link to add to a server:
// https://discordapp.com/oauth2/authorize?client_id=406197905146511360&scope=bot&permissions=0

namespace com.PhoebeZeitler.WumpusGameRunnerConsole
{
    // this structure will hold data from config.json
    public struct ConfigJson
    {
        [JsonProperty("token")]
        public string Token { get; private set; }

        [JsonProperty("prefix")]
        public string CommandPrefix { get; private set; }
    }

    class WumpusGameRunnerConsole
    {
        static DiscordClient discord;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            // first, let's load our configuration file
            var json = "";
            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync();

            // next, let's load the values from that file
            // to our client's configuration
            var cfgjson = JsonConvert.DeserializeObject<ConfigJson>(json);
            var cfg = new DiscordConfiguration
            {
                Token = cfgjson.Token,
                TokenType = TokenType.Bot,

                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            };





            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "NDA2MTk3OTA1MTQ2NTExMzYw.DUvdFA.7-ixnIzKK3WT-it4-uEDUQ4fVd0",
                TokenType = TokenType.Bot
            });

            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("henge"))
                    await e.Message.RespondAsync("a happy chicken!");
            };

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
