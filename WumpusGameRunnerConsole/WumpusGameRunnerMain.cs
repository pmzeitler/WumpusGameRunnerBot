using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;

//client id: 406197905146511360
// link to add to a server:
// https://discordapp.com/oauth2/authorize?client_id=406197905146511360&scope=bot&permissions=0

namespace com.PhoebeZeitler.WumpusGameRunnerConsole
{
    public delegate void ModuleRegistration(CommandsNextModule Commands);


    // this structure will hold data from config.json
    public struct ConfigJson
    {
        [JsonProperty("token")]
        public string Token { get; private set; }

        [JsonProperty("prefix")]
        public string CommandPrefix { get; private set; }

        [JsonProperty("appIdentifier")]
        public string AppIdentifier { get; private set; }
    }

    class WumpusGameRunnerConsole
    {
        private DiscordClient discord;
        public CommandsNextModule Commands { get; set; }

        private string appIdentifier;

        //private static ModuleRegistration loadedModules = null;

        static void Main(string[] args)
        {
            WumpusGameRunnerConsole prog = new WumpusGameRunnerConsole();
            prog.MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /**
        static void RegisterModule(ModuleRegistration module)
        {
            loadedModules += module;
        }

        private void LoadModules()
        {
            loadedModules?.Invoke(this.Commands);
        }

        **/

        async Task MainAsync(string[] args)
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

            //set appIdentifier from config
            this.appIdentifier = cfgjson.AppIdentifier;
            
            discord = new DiscordClient(cfg);

            // up next, let's set up our commands
            var ccfg = new CommandsNextConfiguration
            {
                // let's use the string prefix defined in config.json
                StringPrefix = cfgjson.CommandPrefix,

                // enable responding in direct messages
                EnableDms = true,

                // enable mentioning the bot as a command prefix
                EnableMentionPrefix = true
            };

            // and hook them up
            this.Commands = this.discord.UseCommandsNext(ccfg);

            // let's hook some command events, so we know what's 
            // going on
            this.Commands.CommandExecuted += this.Commands_CommandExecuted;
            this.Commands.CommandErrored += this.Commands_CommandErrored;

            //LoadModules();
            //This is a temporary fix while I learn & figure out more about DI and the modular system...
            this.Commands.RegisterCommands<GoldenSkyStoriesModule.GoldenSkyStoriesModule>();

            /**
            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("henge"))
                    await e.Message.RespondAsync("a happy chicken!");
            };
            **/


            await discord.ConnectAsync();
            await Task.Delay(-1);
        }


        private Task Client_ClientError(ClientErrorEventArgs e)
        {
            // let's log the details of the error that just 
            // occured in our client
            e.Client.DebugLogger.LogMessage(LogLevel.Error, this.appIdentifier, $"Exception occured: {e.Exception.GetType()}: {e.Exception.Message}", DateTime.Now);

            // since this method is not async, let's return
            // a completed task, so that no additional work
            // is done
            return Task.CompletedTask;
        }

        private Task Commands_CommandExecuted(CommandExecutionEventArgs e)
        {
            // let's log the name of the command and user
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Info, this.appIdentifier, $"{e.Context.User.Username} successfully executed '{e.Command.QualifiedName}'", DateTime.Now);

            // since this method is not async, let's return
            // a completed task, so that no additional work
            // is done
            return Task.CompletedTask;
        }

        private async Task Commands_CommandErrored(CommandErrorEventArgs e)
        {
            // let's log the error details
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Error, this.appIdentifier, $"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}", DateTime.Now);

            // let's check if the error is a result of lack
            // of required permissions
            if (e.Exception is ChecksFailedException ex)
            {
                // yes, the user lacks required permissions, 
                // let them know

                var emoji = DiscordEmoji.FromName(e.Context.Client, ":no_entry:");

                // let's wrap the response into an embed
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Access denied",
                    Description = $"{emoji} You do not have the permissions required to execute this command.",
                    Color = new DiscordColor(0xFF0000) // red
                    // there are also some pre-defined colors available
                    // as static members of the DiscordColor struct
                };
                await e.Context.RespondAsync("", embed: embed);
            }
        }
    }
}
